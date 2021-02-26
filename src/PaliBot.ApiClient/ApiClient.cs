using PaliBot.ApiClient.WebClient;
using System;
using System.Threading;
using Utf8Json;

namespace PaliBot.ApiClient
{
    public class ApiClient
    {
        public event EventHandler<Session> Session;
        public event EventHandler<Exception> FetchError;
        public event EventHandler<Exception> ParseError;

        private IApiWebClient _webClient;
        private string _sessionUrl;

        private Thread sessionThread;
        private Thread parseThread;

        private object sessionJsonLock = new object();
        private volatile string sessionJson = String.Empty;

        private volatile bool _running = false;

        public ApiClient(ApiConfig config) : this(config, new ApiWebClient())
        {

        }

        internal ApiClient(ApiConfig config, IApiWebClient webClient)
        {
            _sessionUrl = $"http://127.0.0.1:{config.Port}/session";

            _webClient = webClient;

            //TODO: consider switching one or both of these threads to timer base to control timing.  need to eval load

            sessionThread = new Thread(ReadSession);
            sessionThread.IsBackground = true;

            parseThread = new Thread(ParseSession);
            parseThread.IsBackground = true;
        }

        public void Start()
        {
            _running = true;

            parseThread.Start();
            sessionThread.Start();
        }

        public void Stop()
        {
            _running = false;         
        }

        private void ReadSession()
        {
            while (_running)
            {
                try
                {
                    var json = _webClient.DownloadString(_sessionUrl);
                    if (json != null && json.Length > 0)
                    {
                        lock (sessionJsonLock)
                        {
                            sessionJson = json;
                        }
                    }
                }
                catch (Exception e)
                {
                    FetchError?.Invoke(this, e);
                }
            }
        }

        private void ParseSession()
        {
            while (_running)
            {
                string json;
                lock (sessionJsonLock)
                {
                    json = sessionJson;
                    sessionJson = null;
                }
                if (json != null && json.Length > 0)
                {
                    try
                    {
                        var session = JsonSerializer.Deserialize<Session>(json);
                        Session?.Invoke(this, session);
                    }
                    catch (Exception e)
                    {
                        ParseError?.Invoke(this, e);
                    }
                }
            }
        }
    }
}

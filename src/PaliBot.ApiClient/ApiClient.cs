using PaliBot.ApiClient.WebClient;
using System;
using System.Threading;
using Utf8Json;

namespace PaliBot.ApiClient
{
    public class ApiClient
    {
        public event EventHandler<Session> Session;

        private IApiWebClient _webClient;
        private string _sessionUrl;

        private Thread sessionThread;
        private Thread parseThread;

        private object sessionJsonLock = new object();
        private string sessionJson = String.Empty;

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

            sessionThread.Start();
            parseThread.Start();
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
                catch
                {
                    //TODO: for now.  ultimately, we should not be ignoring errors.  need to at least log them
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
                }
                if (json.Length > 0)
                {
                    try
                    {
                        var session = JsonSerializer.Deserialize<Session>(json);
                        Session(this, session);
                    }
                    catch
                    {
                        Console.Error.WriteLine("Error parsing session json");
                    }
                }
            }
        }
    }
}

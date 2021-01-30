using PaliBot.Console.Configuration;
using System;
using System.Net;
using System.Threading;

namespace PaliBot.Console.Api
{
    public class ApiClient
    {
        public event EventHandler<string> Session;

        private WebClient webClient;
        private Thread sessionThread;
        private string _sessionUrl;


        public ApiClient(ApiConfig config)
        {
            _sessionUrl = $"http://127.0.0.1:{config.Port}/session";

            webClient = new WebClient();

            sessionThread = new Thread(ReadSession);
            sessionThread.IsBackground = true;
        }

        public void Start()
        {
            sessionThread.Start();
        }

        public void Stop()
        {
            sessionThread.Abort();
        }

        private void ReadSession()
        {
            while (true)
            {
                try
                {
                    var sessionJson = webClient.DownloadString(_sessionUrl);
                    if (!String.IsNullOrEmpty(sessionJson))
                    {
                        Session(this, sessionJson);
                    }
                }
                catch
                {

                }
            }


        }
    }
}

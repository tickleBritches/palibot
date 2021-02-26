using Microsoft.Extensions.Configuration;
using PaliBot.ApiClient;

namespace PaliBot.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var apiConfig = InitOptions<ApiConfig>("api");
            var apiConfig = new ApiConfig();

            var apiClient = new ApiClient.ApiClient(apiConfig);
            var announcer = new Announcer();

            apiClient.Session += (sender, session) =>
            {
                announcer.Update(session);
                System.Console.Title = session.game_clock_display;
            };

            announcer.Announce += (sender, announcement) =>
            {
                System.Console.WriteLine(announcement);
            };

            apiClient.Start();

            System.Console.ReadKey();
        }

        private static T InitOptions<T>(string section) where T:new()
        {
            var config = InitConfig();
            return config.Get<T>();
        }

        private static IConfigurationRoot InitConfig()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            return builder.Build();
        }
    }
}

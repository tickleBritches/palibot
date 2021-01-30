using Microsoft.Extensions.Configuration;
using PaliBot.Console.Configuration;
using System;

namespace PaliBot.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiConfig = InitOptions<ApiConfig>("api");
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

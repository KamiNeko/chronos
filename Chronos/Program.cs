using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Chronos
{
    internal sealed class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            await serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigFileFullPath, optional: true, reloadOnChange: false)
                .AddJsonFile(SecretConfigFileFullPath, optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton(GetApiSettings(configuration));
            services.AddTransient<App>();
        }
        private static ApiSettings GetApiSettings(IConfigurationRoot configuration)
        {
            var apiSettings = configuration.Get<ApiSettings>();
            return DataProtector.UnprotectApiSettings(apiSettings);
        }

        private static string CurrentDirectory
        {
            get { return Directory.GetParent(typeof(Program).Assembly.Location).FullName; }
        }

        private static string ConfigFileFullPath
        {
            get { return Path.Combine(CurrentDirectory, CONFIG_FILE_NAME); }
        }

        private static string SecretConfigFileFullPath
        {
            get { return Path.Combine(CurrentDirectory, SECRET_CONFIG_FILE_NAME); }
        }

        private const string SECRET_CONFIG_FILE_NAME = "appsettings.secret.json";
        private const string CONFIG_FILE_NAME = "appsettings.json";
    }
}

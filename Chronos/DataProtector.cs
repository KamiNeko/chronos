using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Chronos
{
    internal sealed class DataProtector
    {
        public static void ProtectApiSettings(ApiSettings apiSettings)
        {
            var protector = GetDataProtector();

            apiSettings.Email = protector.Protect(apiSettings.Email);
            apiSettings.JiraCloudInstanceName = protector.Protect(apiSettings.JiraCloudInstanceName);
            apiSettings.JiraToken = protector.Protect(apiSettings.JiraToken);
            apiSettings.JiraUserId = protector.Protect(apiSettings.JiraUserId);
            apiSettings.TempoToken = protector.Protect(apiSettings.TempoToken);
            apiSettings.BasePath = protector.Protect(apiSettings.BasePath);

            string json = JsonConvert.SerializeObject(apiSettings);

            var path = SecretConfigFileFullPath;
            File.WriteAllText(path, json);
        }

        public static ApiSettings UnprotectApiSettings(ApiSettings apiSettings)
        {
            try
            {
                var protector = GetDataProtector();
                var unprotectedApiSettings = new ApiSettings
                {
                    Email = protector.Unprotect(apiSettings.Email),
                    JiraCloudInstanceName = protector.Unprotect(apiSettings.JiraCloudInstanceName),
                    JiraToken = protector.Unprotect(apiSettings.JiraToken),
                    JiraUserId = protector.Unprotect(apiSettings.JiraUserId),
                    TempoToken = protector.Unprotect(apiSettings.TempoToken),
                    BasePath = protector.Unprotect(apiSettings.BasePath),
                };

                return unprotectedApiSettings;
            }
            catch (Exception)
            {
                return new ApiSettings();
            }            
        }

        private static IDataProtector GetDataProtector()
        {
            // https://stackoverflow.com/a/56158490
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDataProtection()
                .SetApplicationName(APP_NAME)
                .PersistKeysToFileSystem(new DirectoryInfo(SECRET_KEYS_DIR_NAME));

            var services = serviceCollection.BuildServiceProvider();
            var dataProtectionProvider = services.GetService<IDataProtectionProvider>();
            return dataProtectionProvider.CreateProtector(APP_NAME);
        }

        private static string CurrentDirectory
        {
            get { return Directory.GetParent(typeof(Program).Assembly.Location).FullName; }
        }

        private static string SecretConfigFileFullPath
        {
            get { return Path.Combine(CurrentDirectory, SECRET_CONFIG_FILE_NAME); }
        }

        private const string SECRET_CONFIG_FILE_NAME = "appsettings.secret.json";
        private const string APP_NAME = "7D3CE3B7-004F-43A3-A4AF-1FCD3C2F5E20";
        private const string SECRET_KEYS_DIR_NAME = "keys";
    }
}

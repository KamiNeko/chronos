using Chronos.AtlassianApi;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Chronos.Commands
{
    internal sealed class SetupCommand
    {
        internal static Command Create()
        {
            return new Command("setup")
            {
                Description = "Configure connection to Atlassian Jira and Tempo REST API",
                Handler = CommandHandler.Create(async () =>
                {
                    await Run();
                })
            };
        }

        internal static async Task Run()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("In order to access Atlassian Jira and Tempo, we need to configure some things first. Please enter the e-mail address you use to login into your Jira account.");
            AnsiConsole.WriteLine();

            var apiSettings = new ApiSettings();

            apiSettings.Email = AnsiConsole.Prompt(
                new TextPrompt<string>("E-Mail address: ")
                    .Validate(x =>
                    {
                        if (IsValidEmail(x))
                        {
                            return ValidationResult.Success();
                        }
                        else
                        {
                            return ValidationResult.Error("[red]Invalid e-mail address[/]");
                        }
                    }));

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("Next we need to configure the [italic]cloud instance name[/] you are using. Check the URL you see in the browser, when visiting Jira: [underline italic]https://{jira-cloud-instance-name}.atlassian.net...[/]");
            AnsiConsole.WriteLine();

            apiSettings.JiraCloudInstanceName = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter the cloud instance name: "));
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine("Now we need to get an access token for the Atlassian Jira REST API. You need to navigate to [underline italic]https://id.atlassian.com/manage-profile/security/api-tokens[/]" +
                " and generate an API token. You can also let the app open the link for you. After generating the token, do not forget to copy it before closing the window.");
            AnsiConsole.WriteLine();

            if (AnsiConsole.Confirm("Open in browser?"))
            {
                OpenBrowser("https://id.atlassian.com/manage-profile/security/api-tokens");
            }

            AnsiConsole.WriteLine();

            apiSettings.JiraToken = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter the token: "));

            AnsiConsole.WriteLine();

            bool jiraConnectionSuccess = false;

            await AnsiConsole.Status()
                    .StartAsync("Alright, let's check if we can connect to the Jira API...", async ctx =>
                    {
                        jiraConnectionSuccess = await JiraApi.TestJiraConnection(apiSettings.JiraCloudInstanceName, apiSettings.Email, apiSettings.JiraToken);
                    });

            AnsiConsole.Markup("Connection test: ");

            if (!jiraConnectionSuccess)
            {
                AnsiConsole.Markup("[red bold]FAIL[/]");
                AnsiConsole.WriteLine("");
                AnsiConsole.MarkupLine("[red]Looks like the connection could not be established, please try again and make sure there is no typo.[/]");
                return;
            }

            AnsiConsole.Markup("[green bold]OK[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"Next we need to get an access token for the Tempo REST API. Navigate to [italic underline]https://{apiSettings.JiraCloudInstanceName}.atlassian.net/plugins/servlet/ac/io.tempo.jira/tempo-app#!/configuration/api-integration[/] and generate an API token. You can also let the app open the link for you. After generating the token, do not forget to copy it before closing the window.");
            AnsiConsole.WriteLine();

            if (AnsiConsole.Confirm("Open in browser?"))
            {
                OpenBrowser($"https://{apiSettings.JiraCloudInstanceName}.atlassian.net/plugins/servlet/ac/io.tempo.jira/tempo-app#!/configuration/api-integration");
            }

            AnsiConsole.WriteLine();

            apiSettings.TempoToken = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter the token: "));

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();

            bool tempoConnectionSuccess = false;

            await AnsiConsole.Status()
                    .StartAsync("Alright, let's check if we can connect to the Tempo API...", async ctx =>
                    {
                        tempoConnectionSuccess = await JiraApi.TestTempoConnection(apiSettings.TempoToken);
                    });

            AnsiConsole.Markup("Connection test: ");

            if (!tempoConnectionSuccess)
            {
                AnsiConsole.Markup("[red bold]FAIL[/]");
                AnsiConsole.WriteLine("");
                AnsiConsole.MarkupLine("[red]Looks like the connection could not be established, please try again and make sure there is no typo.[/]");
                return;
            }

            AnsiConsole.Markup("[green bold]OK[/]");
            AnsiConsole.WriteLine();

            AnsiConsole.WriteLine("Last we need your account id, this can now be obtained automatically...");

            apiSettings.JiraUserId = string.Empty;

            await AnsiConsole.Status()
                    .StartAsync("Obtaining account id...", async ctx =>
                    {
                        var apiClient = new JiraApi(apiSettings.Email, apiSettings.JiraCloudInstanceName, apiSettings.TempoToken, apiSettings.JiraToken);
                        var myself = await apiClient.GetMyself();

                        apiSettings.JiraUserId = myself.AccountId;
                    });

            AnsiConsole.WriteLine($"Your account id is: {apiSettings.JiraUserId}");

            DataProtector.ProtectApiSettings(apiSettings);

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"Data was encrypted and stored in file [italic]appsettings.secret.json[/].");
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}

using Chronos.Commands;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Chronos
{
    internal sealed class App
    {
        public App(
            ILogger<App> logger,
            ApiSettings apiSettings)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));
        }

        internal async Task Run()
        {
            logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            try
            {
                AnsiConsole.Render(new FigletText("Chronos").Color(new Color(255, 255, 255)));

                var rootCommand = new RootCommand
                {
                    Name = "chronos",
                    Description = "Atlassian Jira & Tempo Time Tracking Utility"
                };

                rootCommand.Add(SetupCommand.Create());
                rootCommand.Add(ScanFileCommand.Create());
                rootCommand.Add(ScanDirectoryCommand.Create());
                rootCommand.Add(ProcessFileCommand.Create(apiSettings));

                var commandLineArguments = Environment.GetCommandLineArgs()[1..];
                await rootCommand.InvokeAsync(commandLineArguments);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception!");
            }
        }

        private readonly ILogger logger;
        private readonly ApiSettings apiSettings;
    }
}
using Spectre.Console;
using System.IO;

namespace Chronos.Commands.Utilities
{
    internal static class FileErrorPrinting
    {
        internal static bool CheckFileExistanceAndPrintError(string filename)
        {
            if (!File.Exists(filename))
            {
                AnsiConsole.Render(new Markup("[bold red]File not found, please make sure to enter a valid filename![/]").LeftAligned());
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
                return false;
            }

            return true;
        }

        internal static bool CheckFileExtensionAndPrintError(string filename)
        {
            if (Path.GetExtension(filename).ToLower() != ".txt")
            {
                AnsiConsole.Render(new Markup("[bold red]File is not a valid text file, please make sure to enter a valid filename![/]").LeftAligned());
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
                return false;
            }

            return true;
        }
    }
}

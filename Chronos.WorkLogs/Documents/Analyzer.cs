using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chronos.WorkLogs.Analysis;
using Chronos.WorkLogs.Parsing;

namespace Chronos.WorkLogs.Documents
{
    public class Analyzer
    {
        public static string GenerateYearDirectoryPath(string basePath, int year)
        {
            return Path.Combine(basePath, year.ToString("D4"));
        }

        public static string GenerateMonthDirectoryPath(string basePath, int year, int month)
        {
            return Path.Combine(basePath, year.ToString("D4"), GetMonthDirectoryName(month));
        }

        public static string GenerateDayFilePath(string basePath, int year, int month, int day)
        {
            return Path.Combine(basePath, year.ToString("D4"), GetMonthDirectoryName(month), $"{year:D4}-{month:D2}-{day:D2}") + ".txt";
        }

        public Analyzer(string basePath)
        {
            this.basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        }

        public AnalysisResult Analyse()
        {
            var analysisResult = new AnalysisResult();

            analysisResult.Documents = ParseBasePath();

            return analysisResult;
        }

        private IEnumerable<Document> ParseBasePath()
        {
            var documents = new List<Document>();
            var directories = Directory.EnumerateDirectories(basePath);

            foreach (var directory in directories)
            {
                var directoryName = Path.GetFileName(directory);

                if (directoryName.Length != 4)
                {
                    continue;
                }

                if (!int.TryParse(directoryName, out int year))
                {
                    continue;
                }

                documents.AddRange(ParseYearDirectory(year));
            }

            return documents;
        }

        private IEnumerable<Document> ParseYearDirectory(int year)
        {
            var documents = new List<Document>();

            string yearDirectory = GenerateYearDirectoryPath(basePath, year);
            var directories = Directory.EnumerateDirectories(yearDirectory);
            var validMonthDirectoryNames = GetMonthDirectoryNames();

            foreach (var directory in directories)
            {
                var directoryName = Path.GetFileName(directory);

                if (!validMonthDirectoryNames.Contains(directoryName))
                {
                    continue;
                }

                if (!int.TryParse(directoryName.Substring(0, 2), out int parsedMonth))
                {
                    continue;
                }

                documents.AddRange(ParseMonthDirectory(year, parsedMonth));
            }

            return documents;
        }

        private IEnumerable<Document> ParseMonthDirectory(int year, int month)
        {
            var documents = new List<Document>();

            string monthDirectory = Path.Combine(basePath, year.ToString("D4"), GetMonthDirectoryName(month));
            var files = Directory.EnumerateFiles(monthDirectory);

            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                var extension = Path.GetExtension(file).ToLower();

                // Parse only text files
                if (extension != ".txt")
                {
                    continue;
                }

                // Check length
                if (filename.Length != 14)
                {
                    continue;
                }

                // Check dashes in format: YYYY-MM-DD.txt
                if (filename[4] != filename[7] || filename[7] != '-')
                {
                    continue;
                }

                // Parse year
                if (!int.TryParse(filename.Substring(0, 4), out int parsedYear))
                {
                    continue;
                }

                // Parse month
                if (!int.TryParse(filename.Substring(5, 2), out int parsedMonth))
                {
                    continue;
                }

                // Parse day
                if (!int.TryParse(filename.Substring(8, 2), out int parsedDay))
                {
                    continue;
                }

                if (parsedYear != year || parsedMonth != month)
                {
                    // TODO: Create issue for wrong ordered file
                    continue;
                }

                var document = CreateDocument(file, year, month, parsedDay);
                documents.Add(document);
            }

            return documents;
        }

        private Document CreateDocument(string filename, int year, int month, int day)
        {
            var document = new Document
            {
                Date = new DateTime(year, month, day),
                Filename = filename
            };

            var parser = new WorkLogDocumentParser(filename);
            var workLogItems = parser.Parse();

            var analyzer = new WorkLogItemsAnalizer(workLogItems);
            var analysisResult = analyzer.Analyze();

            if (workLogItems.Count == 0)
            {
                return document;
            }

            if (document.Date != workLogItems.First().Date)
            {
                // TODO: Create issue for wrong date in document
            }

            document.WorkLogItems = workLogItems;
            document.WorkLogItemAnalysis = analysisResult;

            return document;
        }

        private IEnumerable<string> GetMonthDirectoryNames()
        {
            var monthDirectoryNames = new List<string>();

            for (int i = 1; i <= 12; ++i)
            {
                monthDirectoryNames.Add(GetMonthDirectoryName(i));
            }

            return monthDirectoryNames;
        }

        private static string GetMonthDirectoryName(int month)
        {
            var date = new DateTime(1000, month, 01);
            return $"{date:MM} - {date:MMMM}";
        }

        private readonly string basePath;
    }
}

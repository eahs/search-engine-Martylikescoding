using System.Diagnostics;
using RoverSearch.Models;

namespace RoverSearch.Services;

public class SearchService
{
    private string path = @".\Data\";

    public SearchService()
    {

    }

    /// <summary>
    /// Naive search implementation
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public SearchResults Search(string query)
    {
        var sw = new Stopwatch();
        sw.Start();

        var results = new List<Result>();

        foreach (string file in Directory.GetFiles(path))
        {
            var fileContent = File.ReadAllText(file);

            if (fileContent.Contains(query, StringComparison.OrdinalIgnoreCase))
            {
                var filename = Path.GetFileName(file);
                string title = filename; // Use filename as a fallback title

                // Read the first few lines to find the title
                var titleLine = File.ReadLines(file)
                                    .FirstOrDefault(line => line.StartsWith("title:", StringComparison.OrdinalIgnoreCase));

                if (titleLine != null)
                {
                    // Extract the title from the line (e.g., "title: The Title")
                    title = titleLine.Substring("title:".Length).Trim();
                }

                results.Add(new Result { Filename = filename, Title = title });
            }
        }

        sw.Stop();

        return new SearchResults
        {
            Query = query,
            Results = results,
            Duration = sw.Elapsed
        };
    }
}

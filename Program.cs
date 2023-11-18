// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using HtmlAgilityPack;

Console.WriteLine("Hello, World!");

        string apiKey = "AIzaSyBc7NsBnfCmmRdEXNkJz2qkgDQGFN_mi4Y";
        string cx = "a020da36c059c4601";
        string query = "plagiarism software in India";

        string url = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={cx}&q={query}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                var jsonParseOptions = new JsonSerializerOptions() {
                    PropertyNameCaseInsensitive = true,
                    MaxDepth=10
                };
                
                var resultInJson = System.Text.Json.JsonSerializer.Deserialize<GoogleSearchResult>(content, jsonParseOptions);
                foreach(var item in resultInJson.Items)
                {
                    string htmlContent = await DownloadHtmlAsync(item.Link);
                    if (!string.IsNullOrEmpty(htmlContent))
                    {
                        string extractedText = ExtractTextFromHtml(htmlContent);

                        Console.WriteLine("-------------------------------");
                        Console.WriteLine(item.Link);
                        //Console.WriteLine(extractedText);
                    }

                }
            }
            else
            {
                Console.WriteLine($"Request failed with response code: {response.ToString()}");
            }
        }

static async Task<string> DownloadHtmlAsync(string url)
{
    using (HttpClient client = new HttpClient())
    {
        return await client.GetStringAsync(url);
    }
}

static string ExtractTextFromHtml(string htmlContent)
{
    HtmlDocument doc = new HtmlDocument();
    doc.LoadHtml(htmlContent);

    // Use XPath to select the elements containing the text you want to extract
    // For example, to select all <p> elements:
    var paragraphs = doc.DocumentNode.SelectNodes("//p");

    if (paragraphs != null)
    {
        // Concatenate the text from selected elements
        string extractedText = string.Join(Environment.NewLine, paragraphs.Select(p => p.InnerText));
        return extractedText;
    }
    else
    {
        return "No text found on the webpage.";
    }
}


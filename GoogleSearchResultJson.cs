public class GoogleSearchResult
{
    public List<GoogleSearchItem> Items { get; set; }
}

public class GoogleSearchItem
{
    public string Title { get; set; }
    public string Link { get; set; }
    // Add other properties you want to extract
}

namespace android.Tools;

public class TagTool
{
    public static string[] GetTags(string? tags)
    {
        if (string.IsNullOrWhiteSpace(tags))
            return Array.Empty<string>();
        return tags.Split(",", StringSplitOptions.TrimEntries);
    }

    public static string? SetTags(string? tags)
    {
        if (tags == null)
            return null;
        return string.Join(",", tags);
    }
}
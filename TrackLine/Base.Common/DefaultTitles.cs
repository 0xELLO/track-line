namespace Base.Common;

public static class DefaultTitles
{
    public const string AnimeTitle  = "Anime";
    public const string BookTitle = "Books";
    public const string FilmTitle = "Films";
    public const string SeriesTitle  = "Series";

    public static readonly List<string> HeadListTitles = new List<string>
    {
        AnimeTitle, BookTitle, FilmTitle, SeriesTitle
    };

    public const string TodoTitle = "TODO";
    public const string OnHoldTitle = "On Hold";
    public const string PlanTitle = "Plan";
    
    public static readonly List<string> SubListTitles = new List<string>
    {
        AnimeTitle, BookTitle, FilmTitle, SeriesTitle
    };
}
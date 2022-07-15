namespace App.BLL.DTO.List;

public class ExtendedListItemDTO
{
    // Used as a default translation name (eng) or in user created objects
    public string DefaultTitle { get; set; } = default!;

    // Unique indentifier
    public string Code { get; set; } = default!;
    
    // Length of the object in episodes/pages etc
    public int TotalLength { get; set; } = 1;

    // Opened to search engine/other users can find and add this object
    public bool IsPublic { get; set; } = false;
    
    // Is created by user or recieved externaly
    public bool IsCreatedByUser { get; set; } = false;
    
    
    
    // Position in sublist
    public int Position { get; set; } = default!;
    
    
    
    public int Progress { get; set; } = 0;
    
    public int TimesFinished { get; set; } = 0;
}
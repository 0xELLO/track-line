﻿using Base.Domain;

namespace App.DAL.DTO.List;

public class ListItemDTO : DomainEntityId
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
}
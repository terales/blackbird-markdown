using Blackbird.Applications.Sdk.Common;

namespace PhotoshopApp.Events.Models.Payload;

public class ItemPayload
{
    [Display("ID")]
    public string Id { get; set; }
    
    [Display("Title")]
    public string Title { get; set; }
}
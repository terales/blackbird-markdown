using Blackbird.Applications.Sdk.Common;

namespace PhotoshopApp.Models.Dto;

/// <summary>
/// Dto class for item entity
/// </summary>
public class Berry
{
    // Properties must have display attributes
    // which contain user-friendly name and description of the variable
    [Display("ID", Description = "ID of the Berry")]
    public string Id { get; set; }

    [Display("Name", Description = "Name of the Berry")]
    public string Name { get; set; }

    [Display("URL", Description = "URL to Berry's page")]
    public string Url { get; set; }
}
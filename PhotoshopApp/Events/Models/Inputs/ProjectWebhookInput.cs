using Blackbird.Applications.Sdk.Common;

namespace PhotoshopApp.Events.Models.Inputs;

public class ProjectWebhookInput
{
    [Display("Project ID")]
    public string ProjectId { get; set; }
}
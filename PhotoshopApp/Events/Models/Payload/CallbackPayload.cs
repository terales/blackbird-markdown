using Blackbird.Applications.Sdk.Common;

namespace PhotoshopApp.Events.Models.Payload;

public class CallbackPayload
{
    [Display("Data")]
    public string Data { get; set; }
}
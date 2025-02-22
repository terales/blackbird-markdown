namespace PhotoshopApp.Events.Models.Request;

public class AddWebhookRequest
{
    public string Event { get; set; }
    public string Url { get; set; }
}
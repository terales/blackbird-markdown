using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;
using PhotoshopApp.Constants;
using PhotoshopApp.RestSharp;
using PhotoshopApp.Events.Models.Inputs;
using PhotoshopApp.Events.Models.Request;

namespace PhotoshopApp.Events.Handlers.Base;

/// <summary>
/// Base handler for webhooks with project ID parameter
/// </summary>
public abstract class ProjectWebhookHandler : IWebhookEventHandler
{
    protected abstract string SubscriptionEvent { get; }
    private string ProjectId { get; }
    private AppRestClient Client { get; }

    // Handler takes parameter that is specified by user when event bird is created
    protected ProjectWebhookHandler([WebhookParameter] ProjectWebhookInput input)
    {
        ProjectId = input.ProjectId;
        Client = new();
    }
    
    /// <summary>
    /// Subscribes to a webhook event
    /// </summary>
    public Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var endpoint = $"{ApiEndpoints.Projects}/{ProjectId}{ApiEndpoints.Webhooks}";
        var request = new AppRestRequest(endpoint, Method.Post, creds);
        request.AddJsonBody(new AddWebhookRequest
        {
            // Webhook bird url that will receive webhook data
            Url = values["payloadUrl"],
            Event = SubscriptionEvent
        });

        return Client.ExecuteWithHandling(request);
    }

    /// <summary>
    /// Unsubscribes from a webhook event
    /// </summary>
    public Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var endpoint = $"{ApiEndpoints.Projects}/{ProjectId}{ApiEndpoints.Webhooks}";
        var request = new AppRestRequest(endpoint, Method.Delete, creds);
        request.AddJsonBody(new AddWebhookRequest
        {
            Url = values["payloadUrl"],
            Event = SubscriptionEvent
        });

        return Client.ExecuteWithHandling(request);
    }
}
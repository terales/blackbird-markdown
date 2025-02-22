using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;
using PhotoshopApp.Constants;
using PhotoshopApp.RestSharp;
using PhotoshopApp.Events.Models.Request;

namespace PhotoshopApp.Events.Handlers.Base;

/// <summary>
/// Base handler for parameterless webhooks
/// </summary>
public abstract class ParameterlessWebhookHandler : IWebhookEventHandler
{
    protected abstract string SubscriptionEvent { get; }
    private AppRestClient Client { get; }
    
    protected ParameterlessWebhookHandler()
    {
        Client = new();
    }

    /// <summary>
    /// Subscribes to a webhook event
    /// </summary>
    public Task SubscribeAsync(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        Dictionary<string, string> values)
    {
        var request = new AppRestRequest(ApiEndpoints.Webhooks, Method.Post, creds);
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
        var request = new AppRestRequest(ApiEndpoints.Webhooks, Method.Delete, creds);
        request.AddJsonBody(new AddWebhookRequest
        {
            Url = values["payloadUrl"],
            Event = SubscriptionEvent
        });

        return Client.ExecuteWithHandling(request);
    }
}
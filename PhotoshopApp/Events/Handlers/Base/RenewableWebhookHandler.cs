using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace PhotoshopApp.Events.Handlers.Base;

/// <summary>
/// Webhook handler that can resubscribe to events when they expired
/// </summary>
public class RenewableWebhookHandler : IWebhookEventHandler, IRenewableWebhookEventHandler
{
    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
    {
        // Subscription logic
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
    {
        // Subscription logic
    }
    
    /// <summary>
    /// Implements logic with the renew of an event
    /// </summary>
    
    // Period attribute is needed to specify the time in minutes
    // after which you need to re-subscribe to the event
    [Period(60)]
    public void RenewSubscription(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
        => SubscribeAsync(authenticationCredentialsProvider, values);
}
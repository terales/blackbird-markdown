using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using PhotoshopApp.Events.Handlers;
using PhotoshopApp.Events.Models.Payload;
using PhotoshopApp.Invocables;

namespace PhotoshopApp.Events;

/// <summary>
/// Contains list of webhooks
/// </summary>
[WebhookList]
public class WebhookList : AppInvocable
{
    #region Constructors

    public WebhookList(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    #endregion

    #region Webhooks

    /// <summary>
    /// Receives and processes data when item is created
    /// </summary>
    [Webhook("On item created", typeof(ItemCreatedHandler), Description = "On item created")]
    public Task<WebhookResponse<ItemPayload>> OnItemCreated(WebhookRequest webhookRequest)
        => HandlerWebhook<ItemPayload>(webhookRequest);    
    
    /// <summary>
    /// Receives and processes data when item is created
    /// </summary>
    [Webhook("On project item created", typeof(ProjectItemCreatedHandler), Description = "On project item created")]
    public Task<WebhookResponse<ItemPayload>> OnProjectItemCreated(WebhookRequest webhookRequest)
        => HandlerWebhook<ItemPayload>(webhookRequest);

    /// <summary>web
    /// Receives and processes a callback
    /// </summary>
    [Webhook("On callback received", Description = "On callback received")]
    public Task<WebhookResponse<CallbackPayload>> OnCallbackReceived(WebhookRequest webhookRequest)
        => HandlerWebhook<CallbackPayload>(webhookRequest);

    #endregion

    #region Utils

    private Task<WebhookResponse<T>> HandlerWebhook<T>(WebhookRequest webhookRequest) where T : class
    {
        var body = webhookRequest.Body.ToString();

        if (body is null)
            throw new PluginApplicationException("The app did not return any content");

        var data = JsonConvert.DeserializeObject<T>(body);

        if (data is null)
            throw new PluginApplicationException("The app did not return any content");

        return Task.FromResult(new WebhookResponse<T>
        {
            Result = data
        });
    }

    #endregion
}
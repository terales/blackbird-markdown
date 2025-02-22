using Blackbird.Applications.Sdk.Common.Webhooks;
using PhotoshopApp.Events.Handlers.Base;
using PhotoshopApp.Events.Models.Inputs;

namespace PhotoshopApp.Events.Handlers;

public class ProjectItemCreatedHandler : ProjectWebhookHandler
{
    protected override string SubscriptionEvent => "project.item.created";

    public ProjectItemCreatedHandler([WebhookParameter] ProjectWebhookInput input) : base(input)
    {
    }
}
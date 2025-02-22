using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhotoshopApp.RestSharp;

namespace PhotoshopApp.Invocables;

/// <summary>
/// Class with all the object required for performing web requests to the service.
/// Extends BaseInvocable class that contains context information (Flight ID, Bird ID, User credentials, etc.)
/// </summary>
public class AppInvocable : BaseInvocable
{
    #region Properties

    protected AppRestClient Client { get; }

    protected IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    #endregion

    #region Constructors

    protected AppInvocable(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new();
    }

    #endregion
}
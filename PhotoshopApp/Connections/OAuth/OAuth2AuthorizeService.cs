using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using PhotoshopApp.Constants;

namespace PhotoshopApp.Connections.OAuth;

public class OAuth2AuthorizeService : BaseInvocable, IOAuth2AuthorizeService 
{
    public OAuth2AuthorizeService(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    /// <summary>
    /// Creates OAuth url for the user
    /// </summary>
    /// <param name="values">Values needed for url creation, e.g. state</param>
    /// <returns></returns>
    public string GetAuthorizationUrl(Dictionary<string, string> values)
    {
        var parameters = new Dictionary<string, string>
        {
            { "client_id",  ApplicationConstants.ClientId},
            { "redirect_uri", InvocationContext.UriInfo.ImplicitGrantRedirectUri.ToString()},
            { "scope", ApplicationConstants.Scope },
            { "state", values["state"] }
        };
        
        // Creating url with query parameters
        return Urls.Authorize.WithQuery(parameters);
    }
}
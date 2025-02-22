using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using PhotoshopApp.Constants;

namespace PhotoshopApp.RestSharp;

public class AppRestRequest : RestRequest
{
    /// <summary>
    /// Creates request instance with all the needed headers already
    /// </summary>
    public AppRestRequest(
        string resource,
        Method method,
        IEnumerable<AuthenticationCredentialsProvider> creds) : base(resource, method)
    {
        // Extract needed API Keys or tokens and add them to headers
        var authHeader = creds.First(x => x.KeyName == CredsNames.ApiToken);
        this.AddHeader(authHeader.KeyName, authHeader.Value);
    }
}
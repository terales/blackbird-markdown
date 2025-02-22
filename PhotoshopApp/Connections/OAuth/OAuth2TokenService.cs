using System.Globalization;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;
using PhotoshopApp.Constants;
using PhotoshopApp.RestSharp;

namespace PhotoshopApp.Connections.OAuth;

/// <summary>
/// Manages user OAuth credentials
/// </summary>
public class OAuth2TokenService : BaseInvocable, IOAuth2TokenService
{
    public OAuth2TokenService(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    #region Token actions

    /// <summary>
    /// Checks if token refresh is needed
    /// </summary>
    /// <param name="values">User credential values</param>
    public bool IsRefreshToken(Dictionary<string, string> values)
        => DateTime.UtcNow > DateTime.Parse(values[CredsNames.ExpiresAtKeyName], CultureInfo.InvariantCulture);


    /// <summary>
    /// Receives authorization credentials after user passed OAuth
    /// </summary>
    public Task<Dictionary<string, string>> RequestToken(
        string state,
        string code,
        Dictionary<string, string> values,
        CancellationToken cancellationToken)
    {
        // Form data for retrieving an access token
        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", ApplicationConstants.ClientId },
            { "client_secret", ApplicationConstants.ClientSecret },
            { "redirect_uri", InvocationContext.UriInfo.AuthorizationCodeRedirectUri.ToString() },
            { "state", state },
            { "code", code }
        };

        return GetTokenData(bodyParameters, cancellationToken);
    }

    /// <summary>
    /// Refreshes an access token after it has expired
    /// </summary>
    public Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values,
        CancellationToken cancellationToken)
    {
        // Form data for refreshing an access token
        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", ApplicationConstants.ClientId },
            { "client_secret", ApplicationConstants.ClientSecret },
            { "refresh_token", values[CredsNames.RefreshToken] }
        };

        return GetTokenData(bodyParameters, cancellationToken);
    }


    /// <summary>
    /// Revokes the token
    /// </summary>
    public Task RevokeToken(Dictionary<string, string> values)
    {
        var bodyParameters = new Dictionary<string, string>
        {
            { "token", values[CredsNames.AccessToken] }
        };

        return ExecuteTokenRequest(bodyParameters, CancellationToken.None);
    }

    #endregion

    #region Utils

    /// <summary>
    /// Common method for retrieving auth data
    /// </summary>
    /// <param name="bodyParameters">Form data of the request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Auth credentials</returns>
    private async Task<Dictionary<string, string>> GetTokenData(Dictionary<string, string> bodyParameters,
        CancellationToken cancellationToken)
    {
        var response = await ExecuteTokenRequest(bodyParameters, cancellationToken);

        return ParseTokenResponse(response.Content!);
    }

    /// <summary>
    /// Parses retrieved token data into the dictionary
    /// </summary>
    /// <param name="responseContent">Request content</param>
    private Dictionary<string, string> ParseTokenResponse(string responseContent)
    {
        var resultDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

        if (resultDictionary is null)
            throw new InvalidOperationException($"Invalid response content: {responseContent}");

        var expiresIn = int.Parse(resultDictionary["expires_in"]);
        var expiresAt = DateTime.UtcNow.AddSeconds(expiresIn);

        resultDictionary[CredsNames.ExpiresAtKeyName] = expiresAt.ToString(CultureInfo.InvariantCulture);
        return resultDictionary;
    }

    /// <summary>
    /// Executes token requests
    /// </summary>
    /// <param name="bodyParameters">Form data of the request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Token request's response</returns>
    private async Task<RestResponse> ExecuteTokenRequest(Dictionary<string, string> bodyParameters,
        CancellationToken cancellationToken)
    {
        using var client = new AppRestClient();
        var request = new RestRequest(Urls.Token, Method.Post);
        bodyParameters.ToList().ForEach(x => request.AddParameter(x.Key, x.Value));

        return await client.ExecuteAsync(request, cancellationToken);
    }

    #endregion
}
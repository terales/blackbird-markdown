using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;
using PhotoshopApp.Constants;
using PhotoshopApp.Models.Dto;
using PhotoshopApp.Models.Response;
using PhotoshopApp.RestSharp;

namespace PhotoshopApp.Connections;

/// <summary>
/// Validates app credentials that were provided by the user
/// </summary>
public class ConnectionValidator : IConnectionValidator
{
    private static readonly AppRestClient Client = new();
    
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
    {
        var request = new AppRestRequest(ApiEndpoints.Berry, Method.Get, authProviders);

        try
        {
            await Client.ExecuteWithHandling<ListResponse<Berry>>(request);

            // If ExecuteWithHandling did not throw an exception
            // then request was successful and credentials are valid
            return new()
            {
                IsValid = true
            };
        }
        catch (Exception ex)
        {
            // If Exception was thrown, we mark connection credentials as invalid
            // and pass the exception message in the response
            return new()
            {
                IsValid = false,
                Message = ex.Message
            };
        }
    }
}
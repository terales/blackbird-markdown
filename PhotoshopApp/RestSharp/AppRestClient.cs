using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using PhotoshopApp.Constants;

namespace PhotoshopApp.RestSharp;

public class AppRestClient : RestClient
{
    public AppRestClient() : base(new RestClientOptions { BaseUrl = new(Urls.Api) })
    {
    }

    public async Task<T> ExecuteWithHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithHandling(request);
        return JsonConvert.DeserializeObject<T>(response.Content!)!;
    }

    public async Task<RestResponse> ExecuteWithHandling(RestRequest request)
    {
        var response = await ExecuteAsync(request);

        if (response.IsSuccessStatusCode)
            return response;
        
        throw new PluginApplicationException(response.Content);
    }
}
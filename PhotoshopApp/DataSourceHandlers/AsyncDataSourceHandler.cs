using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;
using PhotoshopApp.Constants;
using PhotoshopApp.Invocables;
using PhotoshopApp.Models.Dto;
using PhotoshopApp.Models.Response;
using PhotoshopApp.RestSharp;

namespace PhotoshopApp.DataSourceHandlers;

/// <summary>
/// Data source handler for asynchronous dynamic inputs.
/// It provides data on the UI, so that user can choose values from the dropdown instead of inputting it manually.
/// </summary>
public class AsyncDataSourceHandler : AppInvocable, IAsyncDataSourceItemHandler
{
    public AsyncDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    /// <summary>
    /// Fetches data for the dynamic inputs and returns it as a list of options.
    /// </summary>
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new AppRestRequest(ApiEndpoints.Berry, Method.Get, Creds);
        var response = await Client.ExecuteWithHandling<ListResponse<Berry>>(request);

        return response.Results
            // We need to pay attention to SearchString in the context
            // So that we return only values that match user search request
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.Id, x.Name));
    }
}
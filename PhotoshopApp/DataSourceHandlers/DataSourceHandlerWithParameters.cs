using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using PhotoshopApp.Invocables;
using PhotoshopApp.Models.Request;
using PhotoshopApp.RestSharp;

namespace PhotoshopApp.DataSourceHandlers;

/// <summary>
/// Data source handler that can take parameters from user inputs
/// </summary>
public class DataSourceHandlerWithParameters : AppInvocable, IAsyncDataSourceItemHandler
{
    // Saving input model to use it while fetching data
    private readonly DataSourceWithParametersRequest _request;

    protected DataSourceHandlerWithParameters(InvocationContext invocationContext,
        // Specifying the same model where DataSourceHandlerWithParameters was added
        [ActionParameter] DataSourceWithParametersRequest request) : base(invocationContext)
    {
        _request = request;
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        // Throwing error if parameters are not specified
        if (string.IsNullOrWhiteSpace(_request.Url))
            throw new("You should input URL first");

        if (string.IsNullOrWhiteSpace(_request.EntityId))
            throw new("You should input Entity ID first");

        // Fetching data based on provided parameters
        var request = new AppRestRequest($"{_request.Url}/{_request.EntityId}", Method.Get, Creds);
        return await Client.ExecuteWithHandling<IEnumerable<DataSourceItem>>(request);
    }
}
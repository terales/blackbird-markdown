using System.Net.Mime;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using PhotoshopApp.Constants;
using PhotoshopApp.Invocables;
using PhotoshopApp.Models.Dto;
using PhotoshopApp.Models.Request;
using PhotoshopApp.Models.Response;
using PhotoshopApp.RestSharp;

namespace PhotoshopApp.Actions;

/// <summary>
/// Contains list of actions
/// </summary>
[ActionList]
public class Actions : AppInvocable
{
    #region Constructors

    public Actions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    #endregion

    #region Actions

    /// <summary>
    /// Retrieves list of items, takes no action parameters
    /// </summary>
    /// <returns>List of created items</returns>
    [Action("List berries", Description = "List berries")]
    public async Task<ListBerriesResponse> ListItems()
    {
        var request = new AppRestRequest(ApiEndpoints.Berry, Method.Get, Creds);
        var response = await Client.ExecuteWithHandling<ListResponse<Berry>>(request);

        return new(response.Results);
    }

    /// <summary>
    /// Creates a new item
    /// </summary>
    /// <param name="input">Action parameter with the data for creation a new item</param>
    /// <returns>Newly created item</returns>
    [Action("Get berry", Description = "Get specific berry by ID")]
    public Task<Berry> GetBerry([ActionParameter] GetBerryRequest input)
    {
        var request = new AppRestRequest($"{ApiEndpoints.Berry}/{input.BerryName}", Method.Get, Creds);

        return Client.ExecuteWithHandling<Berry>(request);
    }

    /// <summary>
    /// Demonstration of working with files in BlackBird
    /// </summary>
    /// <param name="input">Action parameter with the data for downloading the file</param>
    /// <returns>File data</returns>
    [Action("Download file by URL", Description = "Download specific file by URL")]
    public Task<FileResponse> DownloadFileByUrl([ActionParameter] DownloadFileRequest input)
    {
        // Creating file instance that will be asynchronously downloaded by Blackbird
        var file = new FileReference(new HttpRequestMessage(HttpMethod.Get, input.FileUrl), input.FileName,
            MediaTypeNames.Application.Octet);
        
        return Task.FromResult<FileResponse>(new(file));
    }

    /// <summary>
    /// Creates action callback that can be received later in a BlackBird webhook
    /// </summary>
    /// <param name="input">Callback creation data</param>
    [Action("Create callback", Description = "Create action callback")]
    public Task CreateCallback([ActionParameter] CreateCallbackRequest input)
    {
        var request = new AppRestRequest(ApiEndpoints.Callbacks, Method.Post, Creds);
        request.AddJsonBody(input);

        return Client.ExecuteWithHandling(request);
    }
    
    /// <summary>
    /// Demonstration of dynamic input with parameters
    /// </summary>
    [Action("Dynamic input with parameters", Description = "Demonstration of dynamic input with parameters")]
    public DataSourceWithParametersRequest DynamicInputWithParameters([ActionParameter] DataSourceWithParametersRequest input)
    {
        return input;
    }

    #endregion
}
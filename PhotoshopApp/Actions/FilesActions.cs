using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using PhotoshopApp.Invocables;
using PhotoshopApp.Models.Request;
using PhotoshopApp.Models.Response;

namespace PhotoshopApp.Actions;

/// <summary>
/// Contains list of file actions
/// </summary>
[ActionList]
public class FilesActions : AppInvocable
{
    // Injecting instance of IFileManagementClient that helps to work with files inside of Blackbird
    private readonly IFileManagementClient _fileManagementClient;

    protected FilesActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(
        invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }
    
    /// <summary>
    /// Demonstration of downloading file with IFileManagementClient
    /// </summary>
    /// <param name="input">Action parameter with the data for downloading the file</param>
    /// <returns>File data</returns>
    [Action("Download file", Description = "Download specific file")]
    public async Task<FileResponse> DownloadFile([ActionParameter] DownloadFileRequest input)
    {
        var request = new RestRequest(input.FileUrl);
        var response = await Client.ExecuteAsync(request);

        // Throwing error if status code is not successful
        if (!response.IsSuccessStatusCode)
            throw new PluginApplicationException($"Could not download your file; Code: {response.StatusCode}");

        // Uploading downloaded file to Blackbird
        var file = await _fileManagementClient.UploadAsync(new MemoryStream(response.RawBytes!), response.ContentType!,
            input.FileName);
        return new(file);
    }

    /// <summary>
    /// Demonstration of downloading a large file with IFileManagementClient
    /// </summary>
    /// <param name="input">Action parameter with the data for downloading the file</param>
    /// <returns>File data</returns>
    [Action("Download large file", Description = "Download a large file efficiently")]
    public async Task<FileResponse> DownloadLargeFile([ActionParameter] DownloadFileRequest input)
    {
        // In this case the Blackbird core will download the file efficiently, without a strain on time or memory.
        var request = new HttpRequestMessage(HttpMethod.Get, input.FileUrl);
        var file = new FileReference(request, input.FileName, "text/html");
        return new(file);
    }

    /// <summary>
    /// Demonstration of uploading file with IFileManagementClient
    /// </summary>
    /// <param name="input">Action parameter with the data for downloading the file</param>
    /// <returns>File data</returns>
    [Action("Upload file", Description = "Upload specific file")]
    public async Task UploadFile([ActionParameter] UploadFileRequest input)
    {
        // Downloading input file from Blackbird into a stream
        var fileStream = await _fileManagementClient.DownloadAsync(input.File);
        
        var request = new RestRequest(input.UploadUrl, Method.Post)
            .AddFile("file", () => fileStream, input.File.Name);
        var response = await Client.ExecuteAsync(request);

        // Throwing error if status code is not successful
        if (!response.IsSuccessStatusCode)
            throw new PluginApplicationException($"Could not upload your file; Code: {response.StatusCode}");
    } 
}
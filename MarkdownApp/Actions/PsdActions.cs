using System.Text;
using System.Xml.Linq;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using MarkdownApp.Models;

namespace MarkdownApp.Actions;

[ActionList]
public class PsdActions : BaseInvocable
{
    private readonly IFileManagementClient _fileClient;

    public PsdActions(InvocationContext context, IFileManagementClient fileClient) 
        : base(context)
    {
        _fileClient = fileClient;
    }

    [Action("Markdown to XLIFF", Description = "Convert Markdown (MD/MDX) files to XLIFF")]
    public async Task<FileResponse> PsdToXliff(
        [ActionParameter][Display("Markdown File")] FileReference psdFile)
    {
        var downloadedStream = await _fileClient.DownloadAsync(psdFile);

        using var fileStream = new MemoryStream();
        await downloadedStream.CopyToAsync(fileStream);
        fileStream.Position = 0;

        var xliffReference = await _fileClient.UploadAsync(
            fileStream,
            "application/x-xliff+xml",
            "source.xliff");
        
        return new(xliffReference);
    }

    [Action("XLIFF to Markdown", Description = "Created translated Markdown (MD/MDX) files from XLIFF")]
    public async Task<FileResponse> UpdatePsdFromXliff(
        [ActionParameter][Display("Original Markdown (MD/MDX) file")] FileReference originalPsd,
        [ActionParameter][Display("Translated XLIFF")] FileReference xliffFile)
    {
        var downloadedXliff = await _fileClient.DownloadAsync(xliffFile);

        using var xliffStream = new MemoryStream();
        await downloadedXliff.CopyToAsync(xliffStream);
        xliffStream.Position = 0;
        
        var xliffDoc = await XDocument.LoadAsync(xliffStream,
            System.Xml.Linq.LoadOptions.None, CancellationToken.None);
        
        var translations = xliffDoc.Descendants("trans-unit")
            .Where(u => u.Attribute("id") != null)
            .ToDictionary(
                u => int.Parse(u.Attribute("id")!.Value),
                u => u.Element("target")?.Value ?? string.Empty);


        var downloadedMd = await _fileClient.DownloadAsync(originalPsd);
        using var mdStream = new MemoryStream();
        await downloadedMd.CopyToAsync(mdStream);
        mdStream.Position = 0;

        var translatedMd = await _fileClient.UploadAsync(
            mdStream,
            "text/markdown",
            "translated.md");
        
        return new(translatedMd);
    }
}
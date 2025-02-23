using System.Text;
using System.Xml.Linq;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using MarkdownApp.Models;
using Markdig;
using ReverseMarkdown;

namespace MarkdownApp.Actions;

[ActionList]
public class MdActions : BaseInvocable
{
    private readonly IFileManagementClient _fileClient;

    public MdActions(InvocationContext context, IFileManagementClient fileClient) 
        : base(context)
    {
        _fileClient = fileClient;
    }

    [Action("Markdown to HTML", Description = "Convert Markdown (MD/MDX) files to HTML")]
    public async Task<FileResponse> MdToHtml(
        [ActionParameter][Display("Markdown File")] FileReference MdFile)
    {
        var markdown = await readFileAsync(MdFile);
        
        var frontmatterEnd = getFrontmatterEndIndex(markdown);
        var html = Markdig.Markdown.ToHtml(markdown.Substring(frontmatterEnd));

        var resultFileName = Path.ChangeExtension(MdFile.Name, ".html");
        var resultFile = await uploadContentAsync(html, "text/html", resultFileName);
        return new(resultFile);
    }

    [Action("HTML to Markdown", Description = "Create translated Markdown (MD/MDX) files from HTML")]
    public async Task<FileResponse> UpdateMdFromHtml(
        [ActionParameter][Display("Original Markdown (MD/MDX) file")] FileReference originalMd,
        [ActionParameter][Display("Translated HTML")] FileReference htmlFile)
    {
        var originalMarkdown = await readFileAsync(originalMd);

        var translatedHtml = await readFileAsync(htmlFile);
        var converter = new Converter();
        var translatedMarkdown = converter.Convert(translatedHtml);

        var frontmatter = getFrontmatter(originalMarkdown);
        var finalMarkdown = string.IsNullOrEmpty(frontmatter) 
            ? translatedMarkdown 
            : $"{frontmatter}\n\n{translatedMarkdown}";

        var resultFile = await uploadContentAsync(finalMarkdown, "text/markdown", originalMd.Name);
        return new(resultFile);
    }

    private async Task<string> readFileAsync(FileReference file)
    {
        var fileStream = await _fileClient.DownloadAsync(file);
        using var reader = new StreamReader(fileStream);
        return await reader.ReadToEndAsync();
    }

    private Task<FileReference> uploadContentAsync(string content, string mimeType, string fileName)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        using var stream = new MemoryStream(bytes);
        return _fileClient.UploadAsync(stream, mimeType, fileName);
    }

    private int getFrontmatterEndIndex(string markdown)
    {
        var frontmatterEndIndex = 0;
        var frontmatterDelimiter = "---";

        if (!markdown.StartsWith(frontmatterDelimiter))
        {
            return frontmatterEndIndex;
        }

        var endIndex = markdown.IndexOf(frontmatterDelimiter, frontmatterDelimiter.Length);
        if (endIndex != -1)
        {
            frontmatterEndIndex = endIndex + frontmatterDelimiter.Length;
        }

        return frontmatterEndIndex;
    }

    private string getFrontmatter(string markdown)
    {
        var frontmatterEnd = getFrontmatterEndIndex(markdown);
        return markdown.Substring(0, frontmatterEnd);
    }
}
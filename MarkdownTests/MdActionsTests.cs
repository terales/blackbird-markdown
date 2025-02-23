using MarkdownApp.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using MarkdownTests.Base;

namespace MarkdownTests;

[TestClass]
public class MdActionsTests : TestBase
{
    [TestMethod]
    public async Task TestMdToHtml()
    {
        // Arrange
        var actions = new MdActions(InvocationContext, FileManager);
        var inputFile = new FileReference { Name = "cms-contentful.md" };

        // Act
        var result = await actions.MdToHtml(inputFile);

        // Assert the output file exists and has expected content
        Assert.IsNotNull(result, "Result should not be null");
        Assert.IsNotNull(result.File, "Result.File should not be null");
        Assert.IsTrue(result.File.Name.EndsWith(".html"), "Output file should have .html extension");

        var outputPath = Path.Combine(
            FileManager.FolderLocation,
            result.File.Name);

        Assert.IsTrue(File.Exists(outputPath), $"File should exist at {outputPath}");

        var fileInfo = new FileInfo(outputPath);
        Assert.IsTrue(fileInfo.Length > 0, $"File should not be empty (current size: {fileInfo.Length} bytes)");

        var content = await File.ReadAllTextAsync(outputPath);
        var expectedContent = await File.ReadAllTextAsync(Path.Combine(
            Environment.CurrentDirectory,
            "Base", "Input", "cms-contentful.html"
        ));
        Assert.AreEqual(expectedContent, content, "Generated HTML should match expected content");
    }

    [TestMethod]
    public async Task TestUpdateMdFromHtml()
    {
        // Arrange
        var actions = new MdActions(InvocationContext, FileManager);
        var originalMd = new FileReference { Name = "cms-contentful.md" };
        var translatedHtml = new FileReference { Name = "cms-contentful.html" };

        // Act
        var result = await actions.UpdateMdFromHtml(originalMd, translatedHtml);

        // Assert
        Assert.IsNotNull(result, "Result should not be null");
        Assert.IsNotNull(result.File, "Result.File should not be null");
        Assert.AreEqual(originalMd.Name, result.File.Name, "Output file should have the same name as input");

        var outputPath = Path.Combine(
            FileManager.FolderLocation,
            result.File.Name);

        Assert.IsTrue(File.Exists(outputPath), $"File should exist at {outputPath}");

        var fileInfo = new FileInfo(outputPath);
        Assert.IsTrue(fileInfo.Length > 0, $"File should not be empty (current size: {fileInfo.Length} bytes)");
    }
}
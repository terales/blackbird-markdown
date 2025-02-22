using PhotoshopApp.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using PhotoshopTests.Base;

namespace PhotoshopTests;

[TestClass]
public class PsdActionsTests : TestBase
{
    [TestMethod]
    public async Task TestPsdToXliff()
    {
        // Arrange
        var actions = new PsdActions(InvocationContext, FileManager);
        var inputFile = new FileReference { Name = "webinar-2025-example.psd" };

        // Act
        var result = await actions.PsdToXliff(inputFile);

        // Assert the output file exists and has expected content
        Assert.IsNotNull(result, "Result should not be null");
        Assert.IsNotNull(result.File, "Result.File should not be null");
        Assert.IsTrue(result.File.Name.EndsWith(".xliff"), "Output file should have .xliff extension");

        var outputPath = Path.Combine(
            FileManager.FolderLocation, 
            "Output", 
            result.File.Name);

        Assert.IsTrue(File.Exists(outputPath), $"File should exist at {outputPath}");

        var fileInfo = new FileInfo(outputPath);
        Assert.IsTrue(fileInfo.Length > 0, $"File should not be empty (current size: {fileInfo.Length} bytes)");

        var content = await File.ReadAllTextAsync(outputPath);
        var expectedContent = await File.ReadAllTextAsync(Path.Combine(
            Environment.CurrentDirectory,
            "Base", "Input", "webinar-2025-example.xliff"
        ));
        Assert.AreEqual(expectedContent, content, "Generated XLIFF should match expected content");
    }
}
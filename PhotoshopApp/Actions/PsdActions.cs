using Aspose.PSD;
using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FileFormats.Psd.Layers;
using System.Text;
using System.Xml.Linq;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using PhotoshopApp.Models;

namespace PhotoshopApp.Actions;

[ActionList]
public class PsdActions : BaseInvocable
{
    private readonly IFileManagementClient _fileClient;

    public PsdActions(InvocationContext context, IFileManagementClient fileClient) 
        : base(context)
    {
        _fileClient = fileClient;
    }

    [Action("PSD to XLIFF", Description = "Convert PSD text layers to XLIFF")]
    public async Task<FileResponse> PsdToXliff(
        [ActionParameter][Display("PSD File")] FileReference psdFile)
    {
        using var psdStream = new MemoryStream();
        var downloadedStream = await _fileClient.DownloadAsync(psdFile);
        await downloadedStream.CopyToAsync(psdStream);
        psdStream.Position = 0;

        var fileElement = new XElement("file", 
            new XAttribute("source-language", "it"),
            new XAttribute("target-language", "en"),
            new XElement("body"));

        var xliffDoc = new XDocument(
            new XElement("xliff", 
                new XAttribute("version", "1.2"),
                fileElement));

        using (var psdImage = (PsdImage)Image.Load(psdStream))
        {
            var textLayers = psdImage.Layers
                .Where(l => l is TextLayer)
                .Cast<TextLayer>()
                .Reverse()
                .ToList();

            foreach (var textLayer in textLayers)
            {
                var transUnit = new XElement("trans-unit",
                    new XAttribute("id", textLayer.Name),
                    new XElement("source", textLayer.TextData.Text.Trim())
                );
                
                fileElement.Element("body")?.Add(transUnit);
            }
        }

        var xmlStream = new MemoryStream();
        xliffDoc.Save(xmlStream);
        xmlStream.Position = 0;

        var xliffReference = await _fileClient.UploadAsync(
            xmlStream,
            "application/x-xliff+xml",
            $"{Path.GetFileNameWithoutExtension(psdFile.Name)}.xliff");
        
        return new(xliffReference);
    }

    [Action("XLIFF to PSD", Description = "Update PSD from translated XLIFF")]
    public async Task<FileResponse> UpdatePsdFromXliff(
        [ActionParameter][Display("Original PSD")] FileReference originalPsd,
        [ActionParameter][Display("Translated XLF")] FileReference xliffFile)
    {
        using var xliffStream = new MemoryStream();
        var downloadedXliff = await _fileClient.DownloadAsync(xliffFile);
        await downloadedXliff.CopyToAsync(xliffStream);
        xliffStream.Position = 0;
        
        var xliffDoc = await XDocument.LoadAsync(xliffStream,
            System.Xml.Linq.LoadOptions.None, CancellationToken.None);
        
        var translations = xliffDoc.Descendants("trans-unit")
            .Where(u => u.Attribute("id") != null)
            .ToDictionary(
                u => u.Attribute("id")!.Value,
                u => u.Element("target")?.Value ?? string.Empty);

        using var psdStream = new MemoryStream();
        var downloadedPsd = await _fileClient.DownloadAsync(originalPsd);
        await downloadedPsd.CopyToAsync(psdStream);
        psdStream.Position = 0;
        
        using (var psdImage = (PsdImage)Image.Load(psdStream))
        {
            foreach (var layer in psdImage.Layers)
            {
                if (!(layer is TextLayer textLayer)) continue;
                
                if (translations.TryGetValue(textLayer.Name, out var translatedText))
                {
                    var portion = textLayer.TextData.Items[0];
                    portion.Text = translatedText;
                    textLayer.TextData.UpdateLayerData();
                }
            }

            psdImage.Save(psdStream);
        }

        var updatedPsdRef = await _fileClient.UploadAsync(
            psdStream,
            "application/x-photoshop",
            $"{Path.GetFileNameWithoutExtension(originalPsd.Name)}_translated.psd");
        
        return new(updatedPsdRef);
    }
}
using Blackbird.Applications.Sdk.Common;

namespace PhotoshopApp.Models.Request;

public class DownloadFileRequest
{
    [Display("File URL")]
    public string FileUrl { get; set; }
    
    [Display("File name")]
    public string FileName { get; set; }
}
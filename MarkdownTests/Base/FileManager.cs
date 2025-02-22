using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace PhotoshopTests.Base
{
    public class FileManager : IFileManagementClient
    {
        public string FolderLocation { get; }

        public FileManager(string folderLocation)
        {
            FolderLocation = folderLocation;
        }

        public Task<Stream> DownloadAsync(FileReference reference)
        {
            var inputPath = Path.Combine(Environment.CurrentDirectory, "Base", "Input", reference.Name);
            var fullPath = Path.GetFullPath(inputPath);
            
            var bytes = File.ReadAllBytes(fullPath);
            var stream = new MemoryStream(bytes);
            return Task.FromResult((Stream)stream);
        }

        public Task<FileReference> UploadAsync(Stream stream, string contentType, string fileName)
        {
            var path = Path.Combine(FolderLocation, "Output", fileName);
            var directory = new FileInfo(path).Directory 
                ?? throw new DirectoryNotFoundException($"Could not access directory for path: {path}");
            directory.Create();
            
            using (var fileStream = File.Create(path))
            {
                stream.CopyTo(fileStream);
            }

            return Task.FromResult(new FileReference() { Name = fileName });
        }
    }
}
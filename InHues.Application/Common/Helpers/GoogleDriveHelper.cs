using InHues.Application.Common.Interfaces;
using InHues.Domain.DTO.Custom;
using InHues.Domain.Enums;
using InHues.Domain.Persistence;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InHues.Application.Common.Helpers
{
    public class GoogleDriveHelper
    {
        static DriveService _driveService;
        private  string PathToServiceAccountKeyFile = "g_service_config.json";
        readonly IMainContext _dbContext;
        readonly ICurrentUserService _currentUserService;

        private  Dictionary<FileCategory,string> Directories = new Dictionary<FileCategory,string>();

        public GoogleDriveHelper(IMainContext dbContext, ICurrentUserService currentUserService)
        {
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                 .CreateScoped(DriveService.ScopeConstants.Drive);

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            _dbContext = dbContext;
            _currentUserService = currentUserService;

            //var directories = _dbContext.TenantGDriveDirectories.Where(x => x.TenantId == _currentUserService.TenantId).ToList();
            //if (directories.Any())
            //{
            //    directories.ForEach(d => {
            //        Directories.Add((FileCategory)d.CategoryId, d.DirectoryId);
            //    });
            //}
            
        }

        public  async Task<GDriveResponse?> UploadFileAsync(string base64Image, string mimeType,FileCategory category)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = $"{Guid.NewGuid()}_{category}_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_tt")}",
                    Parents = new List<string>() { Directories[category] }
                };

                base64Image = base64Image.Contains(',') ? base64Image.Split(',')[1] : base64Image; //get just the image data
                byte[] imageBytes = Convert.FromBase64String(base64Image);
                using (var stream = new MemoryStream(imageBytes))
                {
                    var request = _driveService.Files.Create(
                        fileMetadata,
                        stream,
                        mimeType);
                    request.Fields = "id, webViewLink"; // Include 'webViewLink' in the fields to retrieve the URL
                    await request.UploadAsync();

                    return new GDriveResponse()
                    {
                        Url = request.ResponseBody.WebViewLink,
                        Id = request.ResponseBody.Id,
                    };
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public  async Task<bool> DeleteFileAsync(string fileId)
        {
            try
            {
                var request = _driveService.Files.Delete(fileId);
                await request.ExecuteAsync();
                return true; // Deletion successful
            }
            catch (Exception ex)
            {
                // Handle any error that occurs during the deletion process
                Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
                return false; // Deletion failed
            }
        }

        public  IList<Google.Apis.Drive.v3.Data.File> GetFilesInDirectory(string directoryId)
        {
            var request = _driveService.Files.List();
            request.Q = $"'{directoryId}' in parents";
            request.Fields = "files(id, name)";

            var files = new List<Google.Apis.Drive.v3.Data.File>();
            do
            {
                var response = request.Execute();
                files.AddRange(response.Files);
                request.PageToken = response.NextPageToken;
            }
            while (!string.IsNullOrEmpty(request.PageToken));

            return files;
        }

        public  Google.Apis.Drive.v3.Data.File GetFileById(string fileId)
        {
            var request = _driveService.Files.Get(fileId);
            request.Fields = "id, name, mimeType"; // Add additional fields as needed

            return request.Execute();
        }
        public string CreateNewFolder(string folderName,List<string> folderParents) {
            try
            {
                var rootMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = folderName,
                    MimeType = "application/vnd.google-apps.folder",
                    Parents = folderParents
                };
                var rootRequest = _driveService.Files.Create(rootMetadata);
                rootRequest.Fields = "id";
                var rootFile = rootRequest.Execute();
                return rootFile.Id;
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return string.Empty;
            }

        }
    }
}

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PEDRO.DriveUtils
{
    public class GoogleDriveUtil
    {
        //string[] scopes; 
        //String clientId = "223955441118-6rkg9ssnc2cg7kg8l5rpao3mto01kos1.apps.googleusercontent.com";
        //String clientSecret = "cY0UZcKSCueq7QUEzRMw9680";
        private UserCredential credential;
        private DriveService service;
        
        public GoogleDriveUtil(string clientId, string clientSecret)
        {
            this.credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            new string[] { DriveService.Scope.Drive, DriveService.Scope.DriveFile },
            Environment.UserName,
            CancellationToken.None,
            new FileDataStore("Daimto.GoogleDrive.Auth.Store")).Result;

            this.service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Pedro",
            });

        }

        public void upload(string path, string nome)
        {
            //path = "C:/Users/Will/Desktop/jubileu.jpeg";
            var fileMetadata = new File() { Title = nome };
            FilesResource.InsertMediaUpload request;
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                request = service.Files.Insert(fileMetadata, stream, "application/unknown");
                request.Fields = "id";
                request.Upload();
            }
        }

        public void DeleteFile(string fileId)
        {
            service.Files.Delete(fileId).Execute();
        }
    }
}
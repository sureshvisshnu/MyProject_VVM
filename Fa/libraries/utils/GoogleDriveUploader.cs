using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using VisioForge.MediaFramework.ONVIF;
using UserCredential = Google.Apis.Auth.OAuth2.UserCredential;

public static class GoogleDriveUploader
{
    public static string UploadPdfAndGetLink(string pdfPath)
    {
        string[] Scopes = { DriveService.Scope.DriveFile };
        string ApplicationName = "Lab Report Uploader";

        UserCredential credential;

        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            string credPath = "token.json";

            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)
            ).Result;
        }

        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = Path.GetFileName(pdfPath)
        };

        FilesResource.CreateMediaUpload request;

        using (var stream = new FileStream(pdfPath, FileMode.Open))
        {
            request = service.Files.Create(fileMetadata, stream, "application/pdf");

            request.Fields = "id, webViewLink";

            request.Upload();
        }

        var file = request.ResponseBody;

        var permission = new Google.Apis.Drive.v3.Data.Permission
        {
            Type = "anyone",
            Role = "reader"
        };

        service.Permissions.Create(permission, file.Id).Execute();

        return file.WebViewLink;
    }
}

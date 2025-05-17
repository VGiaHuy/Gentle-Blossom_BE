using GentleBlossom_BE.Exceptions;
using GentleBlossom_BE.Helpers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Text.Json;

namespace GentleBlossom_BE.Services.GoogleService
{
    public enum MediaType
    {
        Post,
        Message,
        Avatar,
        Other
    }

    public class GoogleDriveService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private string? _gentleBlossomFolderId;
        private string? _postsFolderId;
        private string? _messagesFolderId;
        private string? _avatarsFolderId;
        private string? _othersFolderId;

        public GoogleDriveService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            InitializeFolderIds();
        }

        public async Task<string> UploadFileAsync(IFormFile file, MediaType mediaType, bool isPublic = true)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null.");

            var accessToken = await GetAccessTokenAsync();

            var credential = GoogleCredential.FromAccessToken(accessToken);
            var driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _config["GoogleAPI:Drive:ApplicationName"] ?? "Gentle Blossom"
            });

            var targetFolderId = mediaType switch
            {
                MediaType.Post => _postsFolderId,
                MediaType.Message => _messagesFolderId,
                MediaType.Avatar => _avatarsFolderId,
                MediaType.Other => _othersFolderId,
                _ => throw new ArgumentException("Invalid media type.")
            };

            if (string.IsNullOrEmpty(targetFolderId))
                throw new Exception($"Folder ID for {mediaType} is not configured.");

            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}",
                Parents = new List<string> { targetFolderId }
            };

            using var stream = file.OpenReadStream();
            var mimeType = file.ContentType;

            var request = driveService.Files.Create(fileMetadata, stream, mimeType);
            request.Fields = "id, webViewLink";
            request.UploadType = "resumable";

            try
            {
                var upload = await request.UploadAsync();
                if (upload.Exception != null)
                {
                    Console.WriteLine($"Upload exception: {upload.Exception.Message}");
                    throw upload.Exception;
                }

                var uploadedFile = request.ResponseBody;
                if (uploadedFile == null)
                    throw new Exception("Failed to get uploaded file response.");

                if (isPublic)
                {
                    var permission = new Google.Apis.Drive.v3.Data.Permission
                    {
                        Role = "reader",
                        Type = "anyone"
                    };
                    try
                    {
                        await driveService.Permissions.Create(permission, uploadedFile.Id).ExecuteAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to set public permission.", ex);
                    }
                }

                return GoogleDriveHelper.ConvertToDirectLink(uploadedFile.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file to Google Drive: {ex.Message}");
                throw;
            }
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var values = new Dictionary<string, string>
            {
                { "client_id", _config["GoogleAPI:Drive:ClientID"] },
                { "client_secret", _config["GoogleAPI:Drive:ClientSecret"] },
                { "refresh_token", _config["GoogleAPI:Drive:RefreshToken"] },
                { "grant_type", _config["GoogleAPI:Drive:GrantType"] ?? "refresh_token" }
            };

            var client = _httpClientFactory.CreateClient();
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync($"{_config["GoogleAPI:Drive:Oauth2Url"]}/token", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"AccessToken failed: {response.StatusCode} - {errorContent}");
                throw new Exception("Failed to get access token.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonDocument.Parse(json);
            if (!tokenResponse.RootElement.TryGetProperty("access_token", out var tokenElement))
                throw new Exception("Access token not found in response.");
            return tokenElement.GetString();
        }

        private void InitializeFolderIds()
        {
            _gentleBlossomFolderId = _config["GoogleAPI:Drive:FolderIds:GentleBlossom"];
            _postsFolderId = _config["GoogleAPI:Drive:FolderIds:Posts"];
            _messagesFolderId = _config["GoogleAPI:Drive:FolderIds:Messages"];
            _avatarsFolderId = _config["GoogleAPI:Drive:FolderIds:Avatars"];
            _othersFolderId = _config["GoogleAPI:Drive:FolderIds:Others"];
        }
    }
}
using GentleBlossom_BE.Exceptions;
using System.Text.RegularExpressions;

namespace GentleBlossom_BE.Helpers
{
    public static class GoogleDriveHelper
    {
        public static string ConvertToDirectLink(string fileId)
        {
            if (string.IsNullOrEmpty(fileId))
                throw new BadRequestException("File ID không hợp lệ.");

            return $"https://drive.google.com/uc?id={fileId}";
        }
    }
}

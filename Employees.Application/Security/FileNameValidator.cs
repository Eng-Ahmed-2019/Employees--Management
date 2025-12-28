using System.Text.RegularExpressions;

namespace Employees.Application.Security
{
    public static class FileNameValidator
    {
        private static readonly string[] AllowedExtensions =
        {
            ".pdf", ".png", ".jpg", ".jpeg"
        };

        public static bool IsValid(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\")) return false;

            var ext = Path.GetExtension(fileName).ToLower();
            if (!AllowedExtensions.Contains(ext)) return false;

            return Regex.IsMatch(
                fileName,
                @"^[a-zA-Z0-9_\-\.]+$"
            );
        }
    }
}
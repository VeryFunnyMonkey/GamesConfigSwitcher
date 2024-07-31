namespace GCS.UI
{

    using System;
    using System.IO;

    public static class PathValidator
    {
        private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();
        private const int MaxPathLength = 260; // Windows maximum path length

        public static bool IsValidWindowsFilePath(string path, bool checkFileExistence = false)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            // Check if path length is valid
            if (path.Length > MaxPathLength)
            {
                return false;
            }

            // Check if the path is a valid UNC path
            if (path.StartsWith(@"\\"))
            {
                // Validate UNC path format
                if (path.IndexOfAny(InvalidPathChars) >= 0)
                {
                    return false;
                }

                // Check if the UNC path is a valid network path
                if (path.Length < 5 || path[2] != '\\')
                {
                    return false;
                }
            }
            else
            {
                // Check for drive letter and colon
                if (path.Length < 3 || !char.IsLetter(path[0]) || path[1] != ':' || !Path.IsPathRooted(path))
                {
                    return false;
                }

                // Check for invalid characters in path
                if (path.IndexOfAny(InvalidPathChars) >= 0)
                {
                    return false;
                }

                // Check for invalid characters in the file name
                string fileName = Path.GetFileName(path);
                if (fileName.IndexOfAny(InvalidFileNameChars) >= 0)
                {
                    return false;
                }

                // Check for reserved names (e.g., CON, PRN)
                if (IsReservedName(Path.GetFileNameWithoutExtension(path)))
                {
                    return false;
                }

                // Check if the directory exists
                string directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    return false;
                }
            }

            // Optionally, check if the file exists
            if (checkFileExistence)
            {
                return File.Exists(path);
            }

            return true;
        }

        private static bool IsReservedName(string name)
        {
            string[] reservedNames = { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
            return Array.Exists(reservedNames, reservedName => string.Equals(name, reservedName, StringComparison.OrdinalIgnoreCase));
        }
    }


}

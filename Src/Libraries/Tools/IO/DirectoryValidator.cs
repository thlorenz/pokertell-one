namespace Tools.IO
{
    using System;
    using System.IO;

    public static class DirectoryValidator
    {
        public static bool IsValidDirectory(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            try
            {
                // if path is invalid, the following line throws ArgumentException: The path is not of a legal form
                new DirectoryInfo(path);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        public static bool IsExistingDirectory(this string path)
        {
            return path.IsValidDirectory() && Directory.Exists(path);
        }
    }
}
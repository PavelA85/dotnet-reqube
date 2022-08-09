namespace ReQube.Utils
{
    public static class PathExts
    {
        public static string Fix(this string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return path.Replace("\\", "/");
        }
    }
}
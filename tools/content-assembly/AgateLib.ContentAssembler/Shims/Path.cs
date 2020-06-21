namespace AgateLib.ContentAssembler.Shims
{
    public interface IPath
    {
        string Combine(string path1, string path2);
        string Combine(string path1, string path2, string path3);
        string GetDirectoryName(string path);
        string GetExtension(string path);
        string GetFileName(string path);
        string GetFileNameWithoutExtension(string path);
        string GetFullPath(string path);
    }

    public class Path : IPath
    {
        public string PathRoot { get; set; }

        public string Combine(string path1, string path2)
            => System.IO.Path.Combine(path1, path2);

        public string Combine(string path1, string path2, string path3)
            => System.IO.Path.Combine(path1, path2, path3);

        public string GetDirectoryName(string path)
            => System.IO.Path.GetDirectoryName(path);

        public string GetExtension(string path) => System.IO.Path.GetExtension(path);

        public string GetFileName(string path)
            => System.IO.Path.GetFileName(path);

        public string GetFileNameWithoutExtension(string path)
            => System.IO.Path.GetFileNameWithoutExtension(path);

        public string GetFullPath(string path)
            => System.IO.Path.GetFullPath(Combine(PathRoot, path));
    }
}

using VermilionTower.ContentPipeline.Shims;

namespace VermilionTower.ContentPipeline
{
    public interface IFileSystem
    {
        string PathRoot { get; set; }

        IFile File { get; }
        IDirectory Directory { get; }
        IPath Path { get; }
    }

    public class SystemIOFileSystem : IFileSystem
    {
        public File File { get; } = new File();
        public Path Path { get; } = new Path();
        public Directory Directory { get; } = new Directory();

        public string PathRoot
        {
            get => File.PathRoot;
            set
            {
                File.PathRoot = value;
                Directory.PathRoot = value;
                Path.PathRoot = value;
            }
        }

        IFile IFileSystem.File => File;
        IPath IFileSystem.Path => Path;
        IDirectory IFileSystem.Directory => Directory;
    }
}

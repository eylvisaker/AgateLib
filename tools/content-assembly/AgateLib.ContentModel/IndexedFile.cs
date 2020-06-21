using System.Diagnostics;

namespace VermilionTower.Content
{
    [DebuggerDisplay("IndexFile {Type}: {Path}")]
    public class IndexedFile
    {
        public string Path { get; set; }
        public ContentType Type { get; set; }
    }

    public enum ContentType
    {
        Raw,
        Texture,
        SoundEffect,
        Music,
    }
}

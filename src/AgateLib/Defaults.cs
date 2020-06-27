namespace AgateLib
{
    public class Defaults
    {
        public static Defaults Instance { get; set; } = new Defaults();

        public string MonospaceFont { get; set; } = "AgateLib/AgateMono";
        public string Cursor { get; set; } = "default";

    }
}

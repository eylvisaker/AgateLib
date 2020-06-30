using AgateLib.UserInterface;

namespace AgateLib.Demo.Selector
{
    internal class FakeTextRepository : ITextRepository
    {
        public string Lookup(string key) => key;
    }
}

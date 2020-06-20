using AgateLib.UserInterface;

namespace AgateLib.Tests.Selector
{
    internal class FakeTextRepository : ITextRepository
    {
        public string Lookup(string key) => key;
    }
}

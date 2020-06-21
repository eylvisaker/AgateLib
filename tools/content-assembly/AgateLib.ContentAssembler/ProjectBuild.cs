using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace AgateLib.ContentAssembler
{
    public class ProjectBuild
    {
        public List<string> Include { get; set; } = new List<string>();

        public string Output { get; set; }

        [YamlMember(Alias = "create-mgcb")]
        public string CreateMgcb { get; set; }
        public string CreateCredits { get; set; }


        public List<CreateIndex> Index { get; set; }
    }

    public class CreateIndex
    {
        public string Output { get; set; }
        public string Filter { get; set; }
        public bool Recurse { get; set; }
        public OutputFormat Format { get; set; }
    }

    public enum OutputFormat
    {
        JSON,
        YAML,
    }
}
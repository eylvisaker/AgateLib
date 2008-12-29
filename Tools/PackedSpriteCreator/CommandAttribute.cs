using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackedSpriteCreator
{
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class CommandAttribute : Attribute
    {
        // This is a positional argument
        public CommandAttribute()
        {
        }

        public string ArgText { get; set; }
        public string HelpText { get; set; }
    }
}

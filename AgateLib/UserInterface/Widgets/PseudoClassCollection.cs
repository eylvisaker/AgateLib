using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface.Widgets
{
    public class PseudoClassCollection
    {
        HashSet<string> values = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Adds or removes a pseudoclass if the flag passed is true.
        /// </summary>
        /// <param name="pseudoclass">The pseudoclass that should be present if value is true</param>
        /// <param name="value">Pass true to include the pseudoclass, false to disable it.</param>
        public void SetIf(string pseudoclass, bool value)
        {
            if (value)
                values.Add(pseudoclass.ToLowerInvariant());
            else
                values.Remove(pseudoclass);
        }

        public void Remove(string pseudoclass)
        {
            values.Remove(pseudoclass);
        }

        public void Add(string pseudoclass)
        {
            values.Add(pseudoclass);
        }

        public bool Contains(string pseudoclass)
        {
            return values.Contains(pseudoclass);
        }
    }
}

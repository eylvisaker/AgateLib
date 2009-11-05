using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PackedSpriteCreator
{
    class ImportImageInfo
    {
        public string FullPath { get; set; }
        public Bitmap Image { get; set; }
        public Color ColorKey { get; set; }
        public bool UseColorKey { get; set; }

        public string Filename
        {
            get { return System.IO.Path.GetFileName(FullPath); }
        }
    }
}

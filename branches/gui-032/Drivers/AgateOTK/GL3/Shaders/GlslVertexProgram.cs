﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;

namespace AgateOTK.GL3.Shaders
{
	class GlslVertexProgram 
	{
		int index;

		public GlslVertexProgram(int index, string source)
		{
			this.index = index;
			this.Source = source;
		}

		public int ShaderHandle
		{
			get { return index; }
		}
		public string Source { get; private set; }
	}
}
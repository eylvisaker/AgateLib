﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.ApplicationModels
{
	public abstract class Scene
	{
		public bool UpdateBelow { get; set; }
		public bool DrawBelow { get; set; }

		public abstract void Update(double delta_t);
		public abstract void Draw();
	}
}

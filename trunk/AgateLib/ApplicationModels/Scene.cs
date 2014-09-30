using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.ApplicationModels
{
	public abstract class Scene
	{
		public bool SceneFinished { get; set; }

		public bool UpdateBelow { get; set; }
		public bool DrawBelow { get; set; }

		protected internal virtual void OnSceneStart()
		{ }
		protected internal virtual void OnSceneEnd()
		{ }

		public abstract void Update(double deltaT);
		public abstract void Draw();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Sprites;

namespace Tests.DisplayTests
{
	class BasicSprite : AgateApplication, IAgateTest
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Basic Sprite"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public void Main(string[] args)
		{
			Run(args);
		}

		#endregion

		Sprite p;

		protected override void Initialize()
		{
			p = new Sprite("Data/boxsprite.png", new Size(96, 96));
			p.AnimationType = SpriteAnimType.PingPong;
			p.TimePerFrame = 250;
			p.StartAnimation();
		}

		protected override void Update(double time_ms)
		{
			p.Update(time_ms);
		}
		protected override void Render()
		{
			Display.Clear(Color.Blue);

			p.Draw();
		}
	}
}

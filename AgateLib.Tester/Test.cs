using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AgateLib.Tests
{
	public interface ITest
	{
		string Name { get; }

		string Category { get; }

        Action Exit { get; set; }

        void Initialize(ITestResources resources);

		void Update(GameTime gameTime);

		void Draw(GameTime gameTime);
	}
}

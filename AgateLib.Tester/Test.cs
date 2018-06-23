using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ManualTests.AgateLib
{
	public interface ITest
	{
		string Name { get; }

		string Category { get; }

		void Initialize(ITestResources resources);

		void Update(GameTime gameTime);

		void Draw(GameTime gameTime);
	}
}

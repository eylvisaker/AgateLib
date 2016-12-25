using AgateLib.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests
{
	/// <summary>
	/// Test based on a scene application model
	/// </summary>
	public interface ISceneModelTest : IAgateTest
	{
		void ModifyModelParameters(SceneModelParameters parameters);
		Scene StartScene { get; }
	}
}

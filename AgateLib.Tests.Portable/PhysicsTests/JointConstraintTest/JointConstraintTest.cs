using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.PhysicsTests.JointConstraintTest
{
	public class JointConstraintTest : AgatePhysicsTest
	{
		public override string Name => "Joint Constraint";

		protected override void OnSceneStart()
		{
			AddExamples(new ParticleOnCircleOffCenterExample(),
				new SmallChainNoGravityExample(),
				new ChainOnCircleExample(),
				new BoxChainExample(),
				new ParticleOnCircleExample());

			base.OnSceneStart();
		}
	}
}

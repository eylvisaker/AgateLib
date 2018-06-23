using AgateLib.Tests.PhysicsTests.JointConstraintTest;

namespace AgateLib.Tests.PhysicsTests.CollisionConstraintTest
{
	public class ContactConstraintTest : AgatePhysicsTest
	{
		public override string Name => "Contact Constraint";

		protected override void OnSceneStart()
		{
			AddExamples(
				new ColliderExample(),
				new ContactCollisionExample(),
				new HittingTheGroundExample(),
				new BoxStackingExample());

			base.OnSceneStart();
		}
	}
}
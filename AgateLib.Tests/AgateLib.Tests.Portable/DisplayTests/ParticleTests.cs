
using System;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Particles;
using AgateLib.Sprites;
using AgateLib.Testing;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.DisplayTests.ParticleTest
{
	public class PixelParticleTest : Scene, ISceneModelTest
	{
		public string Name { get { return "Particles"; } }
		public string Category { get { return "Display"; } }

		Random ran = new Random();

		// PixelParticle
		PixelEmitter pe;
		GravityManipulator gm;
		GravityManipulator gm2;
		
		//SurfaceParticle
		SurfaceEmitter sm;
		
		//SpriteParticle
		SpriteEmitter se;
		
		FadeOutManipulator fom;
		FadeOutManipulator fom2;
		
		GravityPointManipulator gpm;
		
		protected override void OnSceneStart()
		{
			//PixelParticle
			pe = new PixelEmitter(new Vector2(400f, 550f) ,Color.Blue, 2000);
			pe.EmitLife = 15f;
			pe.EmitFrequency = 0.01f;
			pe.PixelSize = new Size(3, 3);
			
			//SurfaceParticle
			sm = new SurfaceEmitter(new Vector2(150f, 550f), 4.2f, 50, 0);
			Surface surf = new Surface(@"smoke2.png");
			sm.AddSurface(surf);
			sm.EmitFrequency = 0.1f;
			sm.EmitAlpha = 1d;
			sm.EmitAcceleration = new Vector2(0, -20);
			sm.EmitVelocity = new Vector2(0, -10);
			
			//SpriteParticle
			Surface surf2 = new Surface(@"smoke.png");
			Sprite sprite = new Sprite(100, 100);
			sprite.AddFrame(surf);
			sprite.AddFrame(surf2);
			sprite.TimePerFrame = 3d;
			sprite.AnimationType = SpriteAnimType.Looping;
			se = new SpriteEmitter(new Vector2(600f, 550f), 4.2f, 100, 0);
			se.AddSprite(sprite);
			se.EmitFrequency = 0.05f;
			se.EmitAlpha = 1d;
			se.EmitAcceleration = new Vector2(0, -20);
			se.EmitVelocity = new Vector2(0, -10);
			
			//Manipulators
			gm = new GravityManipulator(new Vector2(0f, -20f));
			gm.SubscribeToEmitter(sm);
			gm.SubscribeToEmitter(se);
			
			gm2 = new GravityManipulator(Vector2.Empty);
			//gm2.SubscribeToEmitter(pe);
			gm2.SubscribeToEmitter(sm);
			gm2.SubscribeToEmitter(se);
			
			fom = new FadeOutManipulator(2.5f, 0.6f);
			fom.SubscribeToEmitter(pe);
			
			fom2 = new FadeOutManipulator(4f, 0.3f);
			fom2.SubscribeToEmitter(sm);
			fom2.SubscribeToEmitter(se);
			
			gpm = new GravityPointManipulator(new Vector2(400f, 350f), -1f);
			gpm.SubscribeToEmitter(pe);
		}		

		public override void Update(double time_ms)
		{
			gm2.Gravity = new Vector2((float)ran.Next(-300, 300), 0f);
			
			fom.AlphaAmount = (float)ran.NextDouble() * 1.3f;
			fom.LifeBarrier = (float)ran.NextDouble() * 5f;
			
			pe.Update(time_ms);
			pe.EmitVelocity = new Vector2((float)ran.Next(-10, 10), 0f);
			
			sm.Update(time_ms);
			
			se.Update(time_ms);
			se.GetSpriteByKey(0).TimePerFrame = ran.NextDouble() * 3 + 1.5d;
		}
		
		public override void Draw()
		{
			Display.Clear(Color.Black);
			Font font = AgateLib.Assets.Fonts.AgateSans;
			font.Size = 14;

			font.DrawText("FPS: " + Display.FramesPerSecond);
			
			pe.Draw();
			font.DrawText((int)pe.Position.X, (int)pe.Position.Y, "Particles: " + pe.Particles.Count + "/" + pe.Particles.Capacity);
			
			sm.Draw();
			font.DrawText((int)sm.Position.X, (int)sm.Position.Y, "Particles: " + sm.Particles.Count + "/" + sm.Particles.Capacity);
			
			se.Draw();
			font.DrawText((int)se.Position.X, (int)se.Position.Y, "Particles: " + se.Particles.Count + "/" + se.Particles.Capacity);
		}

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}
		public Scene StartScene
		{
			get { return this; }
		}

	}
}

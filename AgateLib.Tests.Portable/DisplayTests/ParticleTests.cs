using System;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Particles.Emitters;
using AgateLib.DisplayLib.Particles.Manipulators;
using AgateLib.DisplayLib.Sprites;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Tests.DisplayTests
{
	public class PixelParticleTest : IAgateTest
	{
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

		public string Name { get { return "Particles"; } }

		public string Category { get { return "Display"; } }

		public void Run(string[] args)
		{
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				Initialize();

				while (AgateApp.IsAlive)
				{
					Update(AgateApp.GameClock.Elapsed.TotalMilliseconds);
					Draw();
				}
			}
		}

		private void Initialize()
		{
			//PixelParticle
			pe = new PixelEmitter(new Vector2f(400f, 550f), Color.Blue, 2000);
			pe.EmitLife = 15f;
			pe.EmitFrequency = 0.01f;
			pe.PixelSize = new Size(3, 3);

			//SurfaceParticle
			sm = new SurfaceEmitter(new Vector2f(150f, 550f), 4.2f, 50, 0);
			Surface surf = new Surface(@"Images/smoke2.png");
			sm.AddSurface(surf);
			sm.EmitFrequency = 0.1f;
			sm.EmitAlpha = 1d;
			sm.EmitAcceleration = new Vector2f(0, -20);
			sm.EmitVelocity = new Vector2f(0, -10);

			//SpriteParticle
			Surface surf2 = new Surface(@"Images/smoke.png");
			Sprite sprite = new Sprite(100, 100);
			sprite.AddFrame(surf);
			sprite.AddFrame(surf2);
			sprite.TimePerFrame = 3d;
			sprite.AnimationType = SpriteAnimType.Looping;
			se = new SpriteEmitter(new Vector2f(600f, 550f), 4.2f, 100, 0);
			se.AddSprite(sprite);
			se.EmitFrequency = 0.05f;
			se.EmitAlpha = 1d;
			se.EmitAcceleration = new Vector2f(0, -20);
			se.EmitVelocity = new Vector2f(0, -10);

			//Manipulators
			gm = new GravityManipulator(-20 * Vector2f.UnitY);
			gm.SubscribeToEmitter(sm);
			gm.SubscribeToEmitter(se);

			gm2 = new GravityManipulator(Vector2f.Zero);
			//gm2.SubscribeToEmitter(pe);
			gm2.SubscribeToEmitter(sm);
			gm2.SubscribeToEmitter(se);

			fom = new FadeOutManipulator(2.5f, 0.6f);
			fom.SubscribeToEmitter(pe);

			fom2 = new FadeOutManipulator(4f, 0.3f);
			fom2.SubscribeToEmitter(sm);
			fom2.SubscribeToEmitter(se);

			gpm = new GravityPointManipulator(new Vector2f(400f, 350f), -1f);
			gpm.SubscribeToEmitter(pe);
		}

		public void Update(double deltaT)
		{
			gm2.Gravity = new Vector2f((float)ran.Next(-300, 300), 0f);

			fom.AlphaAmount = (float)ran.NextDouble() * 1.3f;
			fom.LifeBarrier = (float)ran.NextDouble() * 5f;

			pe.Update(deltaT);
			pe.EmitVelocity = new Vector2f((float)ran.Next(-10, 10), 0f);

			sm.Update(deltaT);

			se.Update(deltaT);
			se.GetSpriteByKey(0).TimePerFrame = ran.NextDouble() * 3 + 1.5d;
		}

		public void Draw()
		{
			Display.BeginFrame();
			Display.Clear(Color.Black);
			IFont font = Font.AgateSans;
			font.Size = 14;

			font.DrawText("FPS: " + Display.FramesPerSecond);

			pe.Draw();
			font.DrawText((int)pe.Position.X, (int)pe.Position.Y, "Particles: " + pe.Particles.Count + "/" + pe.Particles.Capacity);

			sm.Draw();
			font.DrawText((int)sm.Position.X, (int)sm.Position.Y, "Particles: " + sm.Particles.Count + "/" + sm.Particles.Capacity);

			se.Draw();
			font.DrawText((int)se.Position.X, (int)se.Position.Y, "Particles: " + se.Particles.Count + "/" + se.Particles.Capacity);

			Display.EndFrame();
			AgateApp.KeepAlive();
		}
	}
}

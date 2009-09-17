
using System;

using Tests;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Particles;
using AgateLib.Sprites;

namespace Tests.ParticleTest
{
	public class PixelParticleTest : AgateApplication, IAgateTest
	{
		#region IAgateTest Members

		public string Name { get { return "Particles"; } }

		public string Category { get { return "Display"; } }

		public void Main(string[] args)
		{
			Run(args);
		}

		#endregion
		
		protected override AppInitParameters GetAppInitParameters ()
		{
			AppInitParameters para = new AppInitParameters();
			para.InitializeAudio = false;
			para.InitializeJoysticks = false;
			para.ShowSplashScreen = false;
			
			return para;
		}
		
		Random ran = new Random();

		// PixelParticle
		PixelEmitter pe;
		GravityManipulator gm;
		GravityManipulator gm2;
		
		//SurfaceParticle
		SurfaceEmitter sm;
		
		//SpriteParticle
		SpriteEmitter se;
		
		FontSurface fontSurface;
		
		FadeOutManipulator fom;
		FadeOutManipulator fom2;
		
		protected override void Initialize()
		{
			//PixelParticle
			pe = new PixelEmitter(new Vector2(400f, 550f) ,Color.Blue, 5000);
			pe.EmitLife = 4f;
			pe.EmitFrequency = 0.001f;
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
			gm.SubscribeToEmitter(pe);
			gm.SubscribeToEmitter(sm);
			gm.SubscribeToEmitter(se);
			
			gm2 = new GravityManipulator(Vector2.Empty);
			gm2.SubscribeToEmitter(pe);
			gm2.SubscribeToEmitter(sm);
			gm2.SubscribeToEmitter(se);
			
			fom = new FadeOutManipulator(2.5f, 0.6f);
			fom.SubscribeToEmitter(pe);
			
			fom2 = new FadeOutManipulator(4f, 0.3f);
			fom2.SubscribeToEmitter(sm);
			fom2.SubscribeToEmitter(se);
			
			fontSurface = new FontSurface("Arial", 10f, FontStyle.Bold);
		}		

		protected override void Update(double time_ms)
		{
			gm2.Gravity = new Vector2((float)ran.Next(-300, 300), 0f);
			
			fom.AlphaAmount = (float)ran.NextDouble() * 1.3f;
			fom.LifeBarrier = (float)ran.NextDouble() * 5f;
			
			pe.Update(time_ms);
			pe.EmitVelocity = new Vector2(0f, (float)ran.Next(-100, 0));
			
			sm.Update(time_ms);
			
			se.Update(time_ms);
			se.GetSpriteByKey(0).TimePerFrame = ran.NextDouble() * 3 + 1.5d;
		}
		
		protected override void Render()
		{
			Display.Clear(Color.Black);
			
			fontSurface.DrawText("FPS: " + Display.FramesPerSecond);
			
			pe.Draw();
			fontSurface.DrawText(pe.Position.X, pe.Position.Y, "Particles: " + pe.Particles.Count + "/" + pe.Particles.Capacity);
			
			sm.Draw();
			fontSurface.DrawText(sm.Position.X, sm.Position.Y, "Particles: " + sm.Particles.Count + "/" + sm.Particles.Capacity);
			
			se.Draw();
			fontSurface.DrawText(se.Position.X, se.Position.Y, "Particles: " + se.Particles.Count + "/" + se.Particles.Capacity);
		}
	}
}

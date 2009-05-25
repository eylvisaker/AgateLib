
using System;

using Tests;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Particles;

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
		
		FontSurface fontSurface;
		
		protected override void Initialize()
		{
			//PixelParticle
			pe = new PixelEmitter(new Vector2(400f, 550f) ,Color.Blue, 5000);
			pe.EmitLife = 4f;
			pe.EmitFrequenzy = 0.002f;
			pe.PixelSize = new Size(3, 3);
			
			//SurfaceParticle
			sm = new SurfaceEmitter(new Vector2(150f, 550f), 4.2f, 50, 0);
			Surface surf = new Surface(@"smoke2.png");
			sm.AddSurface(surf);
			sm.EmitFrequenzy = 0.1f;
			sm.EmitAlpha = 1d;
			sm.EmitAcceleration = new Vector2(0, -20);
			sm.EmitVelocity = new Vector2(0, -10);
			
			//Manipulators
			gm = new GravityManipulator(new Vector2(0f, -50f));
			gm.SubscribeToEmitter(pe);
			gm.SubscribeToEmitter(sm);
			
			gm2 = new GravityManipulator(Vector2.Empty);
			gm2.SubscribeToEmitter(pe);
			gm2.SubscribeToEmitter(sm);
			
			FadeOutManipulator fom = new FadeOutManipulator(3f, 0.8f);
			fom.SubscribeToEmitter(pe);
			
			FadeOutManipulator fom2 = new FadeOutManipulator(4f, 0.4f);
			fom2.SubscribeToEmitter(sm);
			
			fontSurface = new FontSurface("Arial", 10f, FontStyle.Bold);
		}		

		protected override void Update(double time_ms)
		{
			gm2.Gravity = new Vector2((float)ran.Next(-300, 300), 0f);
			pe.Update(time_ms);
			sm.Update(time_ms);
		}
		
		protected override void Render()
		{
			Display.Clear(Color.Black);
			
			fontSurface.DrawText("FPS: " + Display.FramesPerSecond);
			
			pe.Draw();
			fontSurface.DrawText(pe.Position.X, pe.Position.Y, "Particles: " + pe.Particles.Count + "/" + pe.Particles.Capacity);
			
			sm.Draw();
			fontSurface.DrawText(sm.Position.X, sm.Position.Y, "Particles: " + sm.Particles.Count + "/" + sm.Particles.Capacity);
		}
	}
}

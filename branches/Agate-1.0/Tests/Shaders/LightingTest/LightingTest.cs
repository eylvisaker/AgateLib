using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.InputLib;
using AgateLib.Geometry;

namespace Tests.LightingTest
{
	class LightingTest : IAgateTest
	{
		#region IAgateTest Members

		public string Name { get { return "Lighting"; } }
		public string Category { get { return "Shaders"; } }

		#endregion

		public void Main(string[] args)
		{
			using (AgateSetup setup = new AgateSetup(args))
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				//LightingTestForm frm = new LightingTestForm();
				//frm.Show();

				//DisplayWindow wnd = new DisplayWindow(CreateWindowParams.FromControl(frm.agateRenderTarget1));
				DisplayWindow wnd = DisplayWindow.CreateWindowed("TItle", 640, 480);

				Surface image = new Surface("jellybean.png");
				Surface ball = new Surface("ball.png");
				Point ballPt;
				double time = 0;

				image.SetScale(2.0, 2.0);
				ball.DisplayAlignment = OriginAlignment.Center;

				//Effect fx = ShaderCompiler.CompileEffect(ShaderLanguage.Hlsl,
				//    System.IO.File.ReadAllText("Data/shaders/hlsl/Lighting.fx"));

				var lights = AgateBuiltInShaders.Lighting2D;

				Light lt1 = new Light();
				lt1.Position = new Vector3(0, 0, -1);
				lt1.DiffuseColor = Color.White;

				Light lt2 = new Light();
				lt2.Position = new Vector3(0, 0, -1);
				lt2.DiffuseColor = Color.Yellow;

				lights.AddLight(lt1);
				lights.AddLight(lt2);

				lt1.AttenuationConstant = 0.01f;
				lt1.AttenuationQuadratic = 5e-7f;

				lt2.AttenuationConstant = 0.01f;
				lt2.AttenuationLinear = 1e-4f;

				Display.RenderState.WaitForVerticalBlank = false;

				Mouse.MouseMove += e =>
					{
						lt2.Position = new Vector3(e.MousePosition.X, e.MousePosition.Y, -1);
					};

				while (wnd.IsClosed == false)
				{
					//if (frm.chkMoveLight.Checked)
						time += Display.DeltaTime / 1000.0;

					ballPt = new Point((int)(120 + 110 * Math.Cos(time)),
									   (int)(120 + 110 * Math.Sin(time)));

					lt1.Position = new Vector3(ballPt.X, ballPt.Y, -1);


					//image.RotationAngleDegrees = (double)frm.nudAngle.Value;

					Display.BeginFrame();
					Display.Clear(Color.DarkRed);

					lights.Activate();
					
					//lights.DoLighting();
					//fx.SetVariable("worldViewProj", wnd.OrthoProjection);

					//if (frm.enableShader.Checked)
					//{
					//    Display.Effect = fx;
					//}
					//else
					//    Display.Effect = null;

					//if (frm.chkSurfaceGradient.Checked)
					//{
					//	Gradient g = new Gradient(Color.Red, Color.Blue, Color.Cyan, Color.Green);

					//	image.ColorGradient = g;
					//}
					//else
						image.Color = Color.White;

					//image.TesselateFactor = (int)frm.nudTess.Value;

					image.Draw(50, 50);

					image.Draw(50 + image.DisplayWidth, 50);
					image.Draw(50, 50 + image.DisplayHeight);

					ball.Draw(ballPt);

					Display.EndFrame();
					Core.KeepAlive();

					//frm.lblFPS.Text = "FPS: " + Display.FramesPerSecond.ToString("0.00");
				}
			}
		}

	}
}
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
				setup.AskUser = true;
				setup.PreferredDisplay = AgateLib.Drivers.DisplayTypeID.Direct3D9_SDX;
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				if (Display.Caps.SupportsShaders == false)
				{
					MessageBox.Show("You must have a driver that supports shaders.", "Lighting Test");
					return;
				}
				LightingTestForm frm = new LightingTestForm();
				frm.Show();

				DisplayWindow wnd = new DisplayWindow(CreateWindowParams.FromControl(frm.agateRenderTarget1));

				Surface image = new Surface("jellybean.png");
				Surface ball = new Surface("ball.png");
				Point ballPt;
				double time = 0;

				image.SetScale(2.0, 2.0);
				ball.DisplayAlignment = OriginAlignment.Center;

				Effect fx = ShaderCompiler.CompileEffect(ShaderLanguage.Hlsl,
					System.IO.File.ReadAllText("Data/shaders/hlsl/Lighting.fx"));

				//LightManager lights = new LightManager();
				//lights.Enabled = true;
				//lights.AddPointLight(new Vector3(0, 0, -1), Color.White);
				//lights.AddPointLight(new Vector3(0, 0, -1), Color.Yellow);

				fx.SetVariable("lightColor", Color.White);

				Display.RenderState.WaitForVerticalBlank = false;

				//lights[0].Ambient = Color.White;
				//lights[1].AttenuationConstant = 0.01f;
				//lights[1].AttenuationQuadratic = 5e-7f;

				fx.SetVariable("attenuation", 0.01f, 0, 5e-4f);
				fx.SetTexture(EffectTexture.Texture0, "texture0");

				Mouse.MouseMove += delegate(InputEventArgs e)
					{
						fx.SetVariable("lightPos", (float) e.MousePosition.X, e.MousePosition.Y);
					};

				while (frm.Visible == true)
				{
					if (frm.chkMoveLight.Checked)
						time += Display.DeltaTime / 1000.0;


					ballPt = new Point((int)(120 + 110 * Math.Cos(time)),
									   (int)(120 + 110 * Math.Sin(time)));

					//lights[0].Position = new Vector3(ballPt.X, ballPt.Y, -1);
					//lights[0].Ambient = Color.FromArgb(frm.btnAmbient.BackColor.ToArgb());
					//lights[0].Diffuse = Color.FromArgb(frm.btnDiffuse.BackColor.ToArgb());

					image.RotationAngleDegrees = (double)frm.nudAngle.Value;

					Display.BeginFrame();
					Display.Clear(Color.DarkRed);

					//lights.Enabled = frm.enableLightingCheck.Checked;
					//lights.DoLighting();
					fx.SetVariable("worldViewProj", Display.GetOrthoProjection());

					Display.Effect = fx;

					if (frm.chkSurfaceGradient.Checked)
					{
						Gradient g = new Gradient(Color.Red, Color.Blue, Color.Cyan, Color.Green);

						image.ColorGradient = g;
					}
					else
						image.Color = Color.White;

					//image.TesselateFactor = (int)frm.nudTess.Value;

					image.Draw(50, 50);

					image.Draw(50 + image.DisplayWidth, 50);
					image.Draw(50, 50 + image.DisplayHeight);

					ball.Draw(ballPt);

					Display.EndFrame();
					Core.KeepAlive();

					frm.lblFPS.Text = "FPS: " + Display.FramesPerSecond.ToString("0.00");
				}
			}
		}

	}
}
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ERY.AgateLib;
using ERY.AgateLib.Geometry;

namespace LightingTest
{
    static class LightingTest
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.Cancel)
                    return;

                LightingTestForm frm = new LightingTestForm();
                frm.Show();

                DisplayWindow wnd = new DisplayWindow(frm.agateRenderTarget1);

                Surface image = new Surface("test.png");
                Surface ball = new Surface("ball.png");
                Point ballPt;
                double time = 0;

                image.SetScale(2.0, 2.0);
                ball.DisplayAlignment = OriginAlignment.Center;

                LightManager lights = new LightManager();
                lights.Enabled = true;
                lights.AddPointLight(new Vector3(0, 0, -1), Color.White);
                lights.AddPointLight(new Vector3(0, 0, -1), Color.Yellow);

                Display.VSync = false;

                //lights[0].Ambient = Color.White;
                //lights[0].AttenuationLinear = 0.01f;
                //lights[0].AttenuationQuadratic = 0.0001f;

                Mouse.MouseMove += delegate(InputEventArgs e)
                    {
                        lights[1].Position =
                            new Vector3(e.MousePosition.X, e.MousePosition.Y, -1);
                    };

                while (frm.Visible == true)
                {
                    if (frm.chkMoveLight.Checked)
                        time += Display.DeltaTime / 1000.0;

                    
                    ballPt = new Point((int)(120 + 110 * Math.Cos(time)),
                                       (int)(120 + 110 * Math.Sin(time)));

                    lights[0].Position = new Vector3(ballPt.X, ballPt.Y, -1);
                    lights[0].Ambient = Color.FromArgb(frm.btnAmbient.BackColor.ToArgb());
                    lights[0].Diffuse = Color.FromArgb(frm.btnDiffuse.BackColor.ToArgb());

                    image.RotationAngleDegrees = (double)frm.nudAngle.Value;

                    Display.BeginFrame();
                    Display.Clear(Color.DarkRed);

                    lights.Enabled = frm.enableLightingCheck.Checked;
                    lights.DoLighting();

                    image.TesselateFactor = (int)frm.nudTess.Value;

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
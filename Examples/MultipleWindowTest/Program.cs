// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using ERY.AgateLib;
using ERY.AgateLib.Geometry;

namespace MultipleWindowTest
{
    static class Program
    {
        static Surface surf;
        static Random rand = new Random();


        [STAThread]
        static void Main()
        {

            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.Cancel) return;

                MultipleRenderTargetExample myForm = new MultipleRenderTargetExample();
                myForm.Show();

                // create three display windows
                DisplayWindow wnd_1 = new DisplayWindow(myForm.pictureBox1);
                DisplayWindow wnd_2 = new DisplayWindow(myForm.pictureBox2);
                DisplayWindow wnd_3 = new DisplayWindow(myForm.pictureBox3);
                //DisplayWindow wnd_4 = new DisplayWindow(myForm.pictureBox4);

                // create a surface for drawing
                surf = new Surface(200, 150);

                // this is the code that will be called when the button is pressed
                myForm.button1.Click += new EventHandler(button1_Click);
                myForm.button2.Click += new EventHandler(button2_Click);
                while (myForm.Visible)
                {
                    // Render targets must be set before the call to BeginFrame,
                    // and may not be changed between BeginFrame and EndFrame.
                    Display.RenderTarget = wnd_1;

                    Display.BeginFrame();
                    Display.Clear(Color.Red);
                    Display.FillRect(new Rectangle(20, 20, 40, 30), Color.Blue);
                    Display.EndFrame();

                    // now do the second window.
                    Display.RenderTarget = wnd_2;

                    Display.BeginFrame();
                    Display.Clear(Color.Green);
                    Display.FillRect(new Rectangle(20, 20, 40, 30), Color.Yellow); 
                    Display.EndFrame();

                    // draw the third window from the surface
                    Display.RenderTarget = wnd_3;

                    Display.BeginFrame();
                    Display.Clear(Color.Black);
                    surf.Draw(0, 0);
                    Display.EndFrame();

                    //Display.RenderTarget = wnd_4;
                    //Display.BeginFrame();
                    //Display.EndFrame();

                    Core.KeepAlive();
                    //System.Threading.Thread.Sleep(250);

                }

            }


        }

        static void button2_Click(object sender, EventArgs e)
        {
            Display.RenderTarget = surf;

            Display.BeginFrame();
            Display.Clear(0, 0, 0, 0);
            Display.EndFrame();
        }

        static void button1_Click(object sender, EventArgs e)
        {
            Display.RenderTarget = surf;

            Display.BeginFrame();

            Rectangle rect = new Rectangle(
                rand.Next(-10, 190),
                rand.Next(-10, 140),
                rand.Next(20, 100),
                rand.Next(20, 100));
            Color clr = Color.FromArgb(255 /*rand.Next(200, 256)*/, rand.Next(0, 256),
                    rand.Next(0, 256), rand.Next(0, 256));

            Display.FillRect(rect, clr);

            Display.EndFrame();

            System.Diagnostics.Debug.Print("Wrote rectangle to {0} with color {1}.", rect, clr);
            System.Diagnostics.Debug.Flush();
        }

    }
}
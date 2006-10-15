// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;

using ERY.AgateLib;
using ERY.AgateLib.Drivers;
using ERY.AgateLib.Geometry;

namespace PerformanceTester
{
    static class Program
    {
        struct Rects
        {
            public Rectangle rect;
            public Color color;
        }
        static Random rnd = new Random();
        static FontSurface font;

        const int totalFrames = 300;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ERY.AgateLib.MDX.MDX1_Display.Register();
            //ERY.AgateLib.PlatformSpecific.Win32Platform.Register();
            Core.Initialize();

            ICollection<DriverInfo<DisplayTypeID>> drivers = Registrar.DisplayDriverInfo;

            frmPerformanceTester frm = new frmPerformanceTester();
            frm.Show();

            foreach (DriverInfo<DisplayTypeID> info in drivers)
            {
                Trace.WriteLine(string.Format("Starting driver {0}...", info.Name));
                Trace.Indent();
                double fps;

                Display.Initialize(info.TypeID);
                DisplayWindow wind = new DisplayWindow("Performance Test", 300, 300);
                font = new FontSurface("Arial", 11);

                Trace.WriteLine("Doing Filled Rect test");
                fps = FilledRectTest() * 1000;
                Trace.WriteLine(string.Format("The driver {0} got {1} fps.", info.Name, fps));

                Trace.WriteLine("Doing Draw Rect test");
                fps = DrawRectTest() * 1000;
                Trace.WriteLine(string.Format("The driver {0} got {1} fps.", info.Name, fps));

                Trace.WriteLine("Doing Draw Surface test, no color");
                fps = DrawSurfaceTest(false) * 1000;
                Trace.WriteLine(string.Format("The driver {0} got {1} fps.", info.Name, fps));

                Trace.WriteLine("Doing Draw Surface test");
                fps = DrawSurfaceTest(true) * 1000;
                Trace.WriteLine(string.Format("The driver {0} got {1} fps.", info.Name, fps));

                Trace.WriteLine("Doing Stretch test, no color");
                fps = StretchTest(false) * 1000;
                Trace.WriteLine(string.Format("The driver {0} got {1} fps.", info.Name, fps));

                Trace.WriteLine("Doing Stretch test");
                fps = StretchTest(true) * 1000;
                Trace.WriteLine(string.Format("The driver {0} got {1} fps.", info.Name, fps));

                //Trace.WriteLine("Doing Stretch test with queued rects");
                //fps = StretchTestQueue(true) * 1000;
                //Trace.WriteLine(string.Format("The driver {0} got {1} fps.", info.Name, fps));

                Trace.Unindent();

                if (Display.CurrentWindow.Closed)
                {
                    Display.Dispose();
                    return;
                }

                Display.Dispose();

            }

            System.Windows.Forms.Application.Run(frm);

        }

        private static double StretchTestQueue(bool applyColor)
        {
            Timing.StopWatch timer = new Timing.StopWatch();
            Surface surf = new Surface("test.jpg");
            int frames = 0;

            surf.Color = Color.White;
            double count = 1;

            for (frames = 0; frames < totalFrames; frames++)
            {
                if (Display.CurrentWindow.Closed)
                    return frames / (double)timer.TotalMilliseconds;

                count = 1 + Math.Cos(frames / 60.0);

                Display.BeginFrame();

                
                for (int i = 0; i < 15; i++)
                {
                    surf.SetScale(1.0, 1.0);
                    surf.Color = Color.White;
                    surf.Draw(10, 10);

                    if (applyColor)
                        surf.Color = Color.Orange;

                    surf.SetScale(1.5, 1.5);
                    surf.Draw(120, 10);

                    if (applyColor)
                        surf.Color = Color.LightGreen;

                    surf.SetScale(count, 1.25);
                    surf.Draw(10, 100);

                    if (applyColor)
                        surf.Color = Color.LightCoral;

                    surf.SetScale(0.5 + count / 2, 0.5 + count / 2);
                    surf.Draw((int)(150 + 40 * Math.Cos(frames / 70.0)),
                              (int)(120 + 40 * Math.Sin(frames / 70.0)));

                    font.DrawText(string.Format("{0} frames per second.", Math.Round(Display.FramesPerSecond, 2)));
                }

                Display.EndFrame(false);
                Core.KeepAlive();

                if (timer.TotalSeconds > 30)
                    break;
            }

            return frames / (double)timer.TotalMilliseconds;
        }
        
        private static double StretchTest(bool applyColor)
        {
            Timing.StopWatch timer = new Timing.StopWatch();
            Surface surf = new Surface("test.jpg");
            int frames = 0;

            surf.Color = Color.White;
            double count = 1;

            for (frames = 0; frames < totalFrames; frames++)
            {
                if (Display.CurrentWindow.Closed)
                    return frames / (double)timer.TotalMilliseconds;

                count = 1 + Math.Cos(frames / 60.0);

                Display.BeginFrame();
                Display.Clear();
                for (int i = 0; i < 15; i++)
                {
                   
                    surf.SetScale(1.0, 1.0);
                    surf.Color = Color.White;
                    surf.Draw(10, 10);

                    if (applyColor)
                        surf.Color = Color.Orange;

                    surf.SetScale(1.5, 1.5);
                    surf.Draw(120, 10);

                    if (applyColor)
                        surf.Color = Color.LightGreen;

                    surf.SetScale(count, 1.25);
                    surf.Draw(10, 100);

                    if (applyColor)
                        surf.Color = Color.LightCoral;

                    surf.SetScale(0.5 + count / 2, 0.5 + count / 2);
                    surf.Draw((int)(150 + 40 * Math.Cos(frames / 70.0)),
                              (int)(120 + 40 * Math.Sin(frames / 70.0)));


                } 
                font.DrawText(string.Format("{0} frames per second.", Math.Round(Display.FramesPerSecond, 2)));

                Display.EndFrame(false);
                Core.KeepAlive();

                if (timer.TotalSeconds > 30)
                    break;
            }

            return frames / (double)timer.TotalMilliseconds;
        }
        
        private static double DrawSurfaceTest(bool applyColor)
        {
            Timing.StopWatch timer = new Timing.StopWatch();
            Surface surf = new Surface("test.jpg");
            List<Rects> rects = new List<Rects>();
            int frames = 0;

            surf.Color = Color.White;

            for (int i = 1; i < 10; i++)
            {
                Rects r = CreateRandomRects();

                rects.Add(r);
            }

            for (frames = 0; frames < totalFrames; frames++)
            {
                if (Display.CurrentWindow.Closed)
                    return frames / (double)timer.TotalMilliseconds; 

                rects.Add(CreateRandomRects());

                Display.BeginFrame();
                Display.Clear();

                if (applyColor)
                {
                    for (int i = 0; i < rects.Count; i++)
                    {
                        surf.Color = rects[i].color;
                        surf.Draw(rects[i].rect.Location);
                    }
                }
                else

                    for (int i = 0; i < rects.Count; i++)
                    {
                        surf.Draw(rects[i].rect.Location);
                    }

                font.DrawText(string.Format("{0} frames per second.", Math.Round(Display.FramesPerSecond, 2)));

                Display.EndFrame();
                Core.KeepAlive();

                if (timer.TotalSeconds > 30)
                    break;
            }

            return frames / (double)timer.TotalMilliseconds;
        }

        private static double DrawRectTest()
        {
            Timing.StopWatch timer = new Timing.StopWatch();
            List<Rects> rects = new List<Rects>();
            int frames = 0;

            for (int i = 1; i < 10; i++)
            {
                Rects r = CreateRandomRects();

                rects.Add(r);
            }

            for (frames = 0; frames < totalFrames; frames++)
            {
                if (Display.CurrentWindow.Closed)
                    return frames / (double)timer.TotalMilliseconds;

                rects.Add(CreateRandomRects());

                Display.BeginFrame();
                Display.Clear();

                for (int i = 0; i < rects.Count; i++)
                {
                    Display.DrawRect(rects[i].rect, rects[i].color);
                }

                font.DrawText(string.Format("{0} frames per second.", Math.Round(Display.FramesPerSecond, 2)));

                Display.EndFrame();
                Core.KeepAlive();


                if (timer.TotalSeconds > 30)
                    break;
            }

            return frames / (double)timer.TotalMilliseconds;
        }

        private static double FilledRectTest()
        {
            Timing.StopWatch timer = new Timing.StopWatch();
            List<Rects> rects = new List<Rects>();
            int frames = 0;
            
            for (int i = 1; i < 10; i++)
            {
                Rects r = CreateRandomRects();

                rects.Add(r);
            }

            for (frames = 0; frames < totalFrames; frames++)
            {
                if (Display.CurrentWindow.Closed)
                    return frames / (double)timer.TotalMilliseconds;

                rects.Add(CreateRandomRects());

                Display.BeginFrame();
                Display.Clear();

                for (int i = 0; i < rects.Count; i++)
                {
                    Display.FillRect(rects[i].rect, rects[i].color);
                }

                font.DrawText(string.Format("{0} frames per second.", Math.Round(Display.FramesPerSecond, 2)));

                Display.EndFrame();
                Core.KeepAlive();


                if (timer.TotalSeconds > 30)
                    break;
            }

            return frames / (double)timer.TotalMilliseconds;
        }

        private static Rects CreateRandomRects()
        {
            Rects r = new Rects();

            r.rect = new Rectangle(
                rnd.Next(10, 250), rnd.Next(10, 250),
                rnd.Next(10, 40), rnd.Next(10, 40));
            r.color = Color.FromArgb(
                rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256), rnd.Next(128, 256));

            return r;
        }
    }
}
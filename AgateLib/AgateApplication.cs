using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.DisplayLib;

namespace AgateLib
{
    public class AgateApplication
    {
        DisplayWindow mWindow;
        string mTitle;
        AppInitParameters mInitParams;
        FontSurface font;

        public int Run()
        {
            return Run(null);
        }
        public int Run(string[] args)
        {
            using (AgateSetup setup = new AgateSetup(args))
            {
                setup.Initialize(
                    InitParams.InitializeDisplay,
                    InitParams.InitializeAudio,
                    InitParams.InitializeJoysticks);

                if (setup.WasCanceled)
                    return 1;

                CreateDisplayWindow();

                font = new FontSurface("Arial", 14.0f, FontStyle.Italic);


                if (InitParams.ShowSplashScreen)
                {
                    DoSplash();
                }

                if (MainWindow.IsClosed)
                    return 0;
            }

            return 0;
        }

        private void DoSplash()
        {
            Timing.StopWatch watch = new Timing.StopWatch();

            while (watch.TotalMilliseconds < 5000)
            {
                Display.BeginFrame();

                DrawSplashScreen(Display.DeltaTime);

                Display.EndFrame();
                Core.KeepAlive();

                System.Threading.Thread.Sleep(0);

                if (MainWindow.IsClosed)
                    return;
            }
        }

        double totalSplashTime = 0;

        protected virtual void DrawSplashScreen(double time_ms)
        {
            Display.Clear(Color.White);

            string text = "Powered by AgateLib";
            Size size = font.StringDisplaySize(text);

            int left = (int)(totalSplashTime * size.Width - size.Width);
            Rectangle gradientRect = new Rectangle(left, MainWindow.Height - size.Height,
                size.Width, size.Height);

            if (left < 0)
            {
                Display.PushClipRect(gradientRect);
            }

            font.Color = Color.Black;
            font.DisplayAlignment = OriginAlignment.BottomLeft;
            font.DrawText(0, MainWindow.Height, text);

            Gradient g = new Gradient(
                Color.FromArgb(0, Color.White),
                Color.White,
                Color.FromArgb(0, Color.White),
                Color.White);

            Display.FillRect(gradientRect, g);
            if (left < 0)
            {
                Display.PopClipRect();
            }

            totalSplashTime += time_ms / 1000.0;
        }

        private void CreateDisplayWindow()
        {
            if (InitParams.FullScreen)
            {
                mWindow = DisplayWindow.CreateFullScreen(ApplicationTitle,
                    InitParams.WindowSize.Width, InitParams.WindowSize.Height);
            }
            else
            {
                mWindow = DisplayWindow.CreateWindowed(
                    ApplicationTitle, InitParams.WindowSize.Width, InitParams.WindowSize.Height);
            }
        }

        protected virtual string ApplicationTitle
        {
            get { return "AgateLib Application"; }
        }
        protected virtual AppInitParameters GetAppInitParameters()
        {
            return new AppInitParameters();
        }

        private AppInitParameters InitParams
        {
            get
            {
                mInitParams = mInitParams ?? GetAppInitParameters();
                return mInitParams;
            }
        }

        public DisplayWindow MainWindow
        {
            get { return mWindow; }
        }
    }
}

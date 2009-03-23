//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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

                font = new FontSurface("Arial", 12.0f);

                if (InitParams.ShowSplashScreen)
                {
                    DoSplash();
                }

                Initialize();

                while (MainWindow.IsClosed == false)
                {
                    Display.BeginFrame();

                    Render(Display.DeltaTime);

                    Display.EndFrame();
                    Core.KeepAlive();
                }
            }

            return 0;
        }

        protected virtual void Initialize()
        {
        }

        private void DoSplash()
        {
            Timing.StopWatch watch = new Timing.StopWatch();

            while (watch.TotalMilliseconds < 500)
            {
                Display.BeginFrame();

                RenderSplashScreen(Display.DeltaTime);

                Display.EndFrame();
                Core.KeepAlive();

                System.Threading.Thread.Sleep(0);

                if (MainWindow.IsClosed)
                    return;
            }
        }

        double totalSplashTime = 0;
        bool splashFadeDone = false;

        protected virtual void Render(double time_ms)
        {
            RenderSplashScreen(time_ms);

            if (splashFadeDone)
            {
                Surface powered = InternalResources.Data.PoweredBy;
                Size size = powered.SurfaceSize;

                int bottom = MainWindow.Height - size.Height;
                int h = font.FontHeight;

                font.DisplayAlignment = OriginAlignment.BottomLeft;
                font.Color = Color.Black;

                font.DrawText(0, bottom - 2 * h, "Welcome to AgateLib.");
                font.DrawText(0, bottom - h, "Your application framework is ready.");
                font.DrawText(0, bottom, "Override the Render method in order to do your own drawing.");
            }
        }
        protected virtual void RenderSplashScreen(double time_ms)
        {
            Display.Clear(Color.White);

            Surface powered = InternalResources.Data.PoweredBy;
            Size size = powered.SurfaceSize;

            int left = (int)(totalSplashTime * size.Width - size.Width);
            Rectangle gradientRect = new Rectangle(left, MainWindow.Height - size.Height,
                size.Width, size.Height);

            if (left < 0)
            {
                Display.PushClipRect(gradientRect);
            }
            else if (left > size.Width)
                splashFadeDone = true;

            powered.DisplayAlignment = OriginAlignment.BottomLeft;
            powered.Draw(0, MainWindow.Height);

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
            CreateWindowParams windp;

            if (InitParams.FullScreen)
            {
                windp = CreateWindowParams.FullScreen(ApplicationTitle,
                    InitParams.WindowSize.Width, InitParams.WindowSize.Height, 32);

                windp.IconFile = InitParams.IconFile;
            }
            else
            {
                windp = CreateWindowParams.Windowed(ApplicationTitle, 
                    InitParams.WindowSize.Width, InitParams.WindowSize.Height,
                    InitParams.AllowResize, InitParams.IconFile);
            }

            mWindow = new DisplayWindow(windp);
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

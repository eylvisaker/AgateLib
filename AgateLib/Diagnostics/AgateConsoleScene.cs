using AgateLib;
using AgateLib.Diagnostics;
using AgateLib.Scenes;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Diagnostics
{
    [Singleton]
    public class AgateConsoleScene : Scene
    {
        private readonly AgateConsole console;
        private readonly IFontProvider fontProvider;

        public AgateConsoleScene(AgateConsole console, IFontProvider fontProvider)
        {
            this.console = console;
            this.fontProvider = fontProvider;

            UpdateBelow = false;
            DrawBelow = true;

            console.ConsoleClosed += () => IsFinished = true;
        }

        protected override void OnSceneStart()
        {
            base.OnSceneStart();

            IsFinished = false;

            console.Open();
        }

        protected override void OnUpdate(GameTime time)
        {
            base.OnUpdate(time);

            console.Update(time);
        }

        protected override void DrawScene(GameTime time)
        {
            base.DrawScene(time);

            console.Draw(time);
        }
    }
}

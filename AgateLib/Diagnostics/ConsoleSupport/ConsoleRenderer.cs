using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using AgateLib.Quality;

namespace AgateLib.Diagnostics.ConsoleSupport
{
	internal class ConsoleRenderer : IConsoleRenderer
	{
		private readonly IAgateConsole console;

		private int height;
		private int entryHeight;
		private long timeOffset;

		private IConsoleTheme theme = ConsoleThemes.Default;

		private FrameBuffer renderTarget;

		public ConsoleRenderer(IAgateConsole instance)
		{
			this.console = instance;

			console.VisibleChanged += (sender, e) => { height = Display.Coordinates.Height * 5 / 12; };
			console.KeyProcessed += (sender, e) => { timeOffset = CurrentTime; };
		}

		private IReadOnlyList<ConsoleMessage> Messages => console.Messages;

		private long CurrentTime => AgateApp.State.App.MasterTime.ElapsedMilliseconds;

		public IConsoleTheme Theme
		{
			get { return theme; }
			set
			{
				Require.ArgumentNotNull(value, nameof(Theme));
				theme = value;

				foreach (var message in console.Messages)
					message.ClearCache();
			}
		}

		public double Alpha { get; set; } = 0.875;
	
		public IFont Font => AgateApp.State.Console.Font;

		public void Draw()
		{
			if (renderTarget == null)
				return;

			if (console.IsVisible == false)
			{
				DrawRecentMessages();
				return;
			}

			renderTarget.RenderTarget.Alpha = Alpha;
			renderTarget.RenderTarget.Draw(Point.Empty);
		}

		public void Update()
		{
			if (renderTarget == null)
				renderTarget = new FrameBuffer(Display.RenderTarget.Width, height);

			Display.PushRenderTarget(renderTarget);
			Display.BeginFrame();
			Display.Clear();

			Redraw();

			Display.EndFrame();
			Display.PopRenderTarget();
		}

		private void Redraw()
		{
			Display.Shader = AgateBuiltInShaders.Basic2DShader;
			AgateBuiltInShaders.Basic2DShader.CoordinateSystem = new Rectangle(0, 0, Display.CurrentWindow.Width,
				Display.CurrentWindow.Height);
			
			DrawConsoleWindow();
		}

		private void DrawRecentMessages()
		{
			long time = CurrentTime;
			int y = 0;
			Font.DisplayAlignment = OriginAlignment.TopLeft;
			Font.Color = Theme.RecentMessageColor;

			for (int i = 0; i < Messages.Count; i++)
			{
				if (time - Messages[i].Time > 5000)
					continue;
				if (Messages[i].MessageType != ConsoleMessageType.Text)
					continue;

				Font.DrawText(new Point(0, y), Messages[i].Text);
				y += Font.FontHeight;
			}
		}

		private void DrawConsoleWindow()
		{
			Display.FillRect(new Rectangle(0, 0, Display.RenderTarget.Width, height), Theme.BackgroundColor);

			DrawUserEntry();

			var y = height - entryHeight;
			y += Font.FontHeight * console.ViewShift;

			for (int i = Messages.Count - 1; i >= 0; i--)
			{
				var message = Messages[i];
				var messageTheme = Theme.MessageTheme(message);

				if (message.Layout == null)
				{
					var text = message.Text;

					if (message.MessageType == ConsoleMessageType.UserInput)
					{
						text = Theme.EntryPrefix + message.Text;
					}

					Font.Color = messageTheme.ForeColor;
					message.Layout = Font.LayoutText(text, Display.RenderTarget.Width);
				}

				y -= message.Layout.Height;

				if (messageTheme.BackColor.A > 0)
				{
					Display.FillRect(
						new Rectangle(0, y,
							Display.RenderTarget.Width, message.Layout.Height),
						messageTheme.BackColor);
				}

				message.Layout.Draw(new Point(0, y));

				if (y < 0)
					break;
			}
		}

		private void DrawUserEntry()
		{
			int y = height;
			Font.DisplayAlignment = OriginAlignment.BottomLeft;

			string currentLineText = Theme.EntryPrefix;

			currentLineText += EscapeText(console.InputText);

			entryHeight = Font.FontHeight;

			if (Theme.EntryBackgroundColor.A > 0)
			{
				Display.FillRect(0, height - entryHeight, Display.RenderTarget.Width, entryHeight,
					Theme.EntryBackgroundColor);
			}

			Font.Color = Theme.EntryColor;
			Font.DrawText(0, y, currentLineText);

			// draw insertion point
			if ((CurrentTime - timeOffset) % 1000 < 500)
			{
				int x = Font.MeasureString(currentLineText.Substring(0, Theme.EntryPrefix.Length + console.InsertionPoint)).Width;

				Display.DrawLine(
					new Point(x, y - Font.FontHeight),
					new Point(x, y),
					Theme.EntryColor);
			}

			Font.DisplayAlignment = OriginAlignment.TopLeft;
		}

		private string EscapeText(string p)
		{
			if (p == null)
				return p;

			return p.Replace("{", "{{}");
		}
	}

	internal interface IConsoleRenderer
	{
		IConsoleTheme Theme { get; set; }

		void Draw();

		void Update();
	}
}

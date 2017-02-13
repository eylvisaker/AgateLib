using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;

namespace AgateLib.Diagnostics.ConsoleSupport
{
	internal class ConsoleRenderer : IConsoleRenderer
	{
		private readonly IAgateConsole console;

		private Size size;
		private int entryHeight;
		private long timeOffset;
		private double viewShiftPixels;

		private IConsoleTheme theme;

		private FrameBuffer renderTarget;

		public ConsoleRenderer(IAgateConsole instance)
		{
			this.console = instance;

			console.KeyProcessed += (sender, e) => { timeOffset = CurrentTime; };
		}

		private IReadOnlyList<ConsoleMessage> Messages => console.Messages;

		private long CurrentTime => (long)AgateApp.State.App.ApplicationClock.CurrentTime.TotalMilliseconds;

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

		public double Alpha { get; set; } = 0.95;

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

			Display.SetClipRect(new Rectangle(Point.Empty, Display.RenderTarget.Size));

			Display.Shader = AgateBuiltInShaders.Basic2DShader;
			AgateBuiltInShaders.Basic2DShader.CoordinateSystem = new Rectangle(0, 0, Display.RenderTarget.Width,
				Display.RenderTarget.Height);

			renderTarget.RenderTarget.Alpha = Alpha;
			renderTarget.RenderTarget.Draw(Point.Empty);
		}

		public void Update()
		{
			if (console.IsVisible == false)
				return;

			if (theme == null)
				theme = ConsoleThemes.Default;

			UpdateViewShift(AgateApp.ApplicationClock.Elapsed.TotalSeconds);

			ResizeRenderTarget();

			Display.PushRenderTarget(renderTarget);
			Display.BeginFrame();
			Display.Clear(Theme.BackgroundColor);

			Redraw();

			Display.EndFrame();
			Display.PopRenderTarget();

		}

		private void UpdateViewShift(double time)
		{
			const int maxDelta = 100;
			const int viewShiftSpeed = 75;

			int targetViewShift = Font.FontHeight * console.ViewShift;
			double delta = targetViewShift - viewShiftPixels;

			if (Math.Abs(delta) > 0.001)
			{
				delta = Math.Sign(delta) * Math.Min(Math.Abs(delta), maxDelta);

				double amount = delta * viewShiftSpeed * time;

				//if (Math.Abs(amount) > maxAmount)
				//	amount = Math.Sign(amount) * maxAmount;

				viewShiftPixels += amount;

				if ((viewShiftPixels - targetViewShift) * delta > 0)
				{
					viewShiftPixels = targetViewShift;
				}
			}
		}

		private void ResizeRenderTarget()
		{
			var newSize = new Size(Display.Coordinates.Width, Display.Coordinates.Height * 5 / 12);

			if (renderTarget == null || newSize != size)
			{
				renderTarget?.Dispose();
				renderTarget = new FrameBuffer(newSize);
				size = newSize;
			}
		}

		private void Redraw()
		{
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
			DrawUserEntry();

			DrawHistory();
		}

		private void DrawHistory()
		{
			var y = size.Height - entryHeight;

			Display.PushClipRect(new Rectangle(0, 0, size.Width, y));

			y += (int)viewShiftPixels;

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
			int y = size.Height;
			Font.DisplayAlignment = OriginAlignment.BottomLeft;

			string currentLineText = Theme.EntryPrefix;

			currentLineText += EscapeText(console.InputText);

			entryHeight = Font.FontHeight;

			if (Theme.EntryBackgroundColor.A > 0)
			{
				Display.FillRect(0, size.Height - entryHeight, Display.RenderTarget.Width, entryHeight,
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

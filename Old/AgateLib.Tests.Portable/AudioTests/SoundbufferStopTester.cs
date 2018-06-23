using AgateLib.AudioLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;

namespace AgateLib.Tests.Audio
{
	class SoundbufferStopTester : IAgateTest
	{
		SoundBuffer snda, sndb;
		SoundBuffer last;

		public string Name
		{
			get { return "SoundBuffer Stop"; }
		}

		public string Category
		{
			get { return "Audio"; }
		}

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				snda = new SoundBuffer("snda.wav");
				sndb = new SoundBuffer("sndb.wav");

				IFont font = Font.AgateSans;

				Input.Unhandled.KeyDown += Keyboard_KeyDown;
				Input.Unhandled.MouseDown += Mouse_MouseDown;

				while (AgateApp.IsAlive)
				{
					Display.BeginFrame();
					Display.Clear();

					font.Size = 14;
					font.Color = Color.White;
					font.DrawText("Press a for first sound, b for second sound.");

					if (snda.IsPlaying)
						font.DrawText(0, 30, "first sound is playing");
					if (sndb.IsPlaying)
						font.DrawText(0, 60, "second sound is playing");

					Display.EndFrame();
					AgateApp.KeepAlive();
				}
			}
		}

		void Mouse_MouseDown(object sender, AgateInputEventArgs e)
		{
			if (last == snda)
				last = sndb;
			else
				last = snda;

			last.Play();
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			if (e.KeyCode == KeyCode.A)
				snda.Play();

			if (e.KeyCode == KeyCode.B)
				sndb.Play();
		}
		
	}
}

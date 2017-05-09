using AgateLib.AudioLib;
using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.AudioTests
{
	public class MusicTest : Scene, IAgateTest
	{
		public string Name => "Music Playback";

		public string Category => "Audio";

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				new SceneStack().Start(this);
			}
		}

		protected override void OnSceneStart()
		{
			var music = new Music("Audio/testmusic.ogg");

			music.Play();
		}
	}
}

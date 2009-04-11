using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Geometry.Builders;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace Glsl
{
	public class Glsl
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			new Glsl().Run(args);
		}
		bool rotating = true;

		private void Run(string[] args)
		{
			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateLib.Utility.AgateFileProvider.AssemblyProvider.AddPath("../Drivers");
			AgateLib.Utility.AgateFileProvider.ImageProvider.AddPath("../../../Tests/TestImages");

			using (AgateSetup setup = new AgateSetup(args))
			{
				setup.AskUser = true;
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				DisplayWindow wind = new DisplayWindow(CreateWindowParams.Windowed("GLSL", 800, 600, null, true));
				Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
				Vector3 eye = new Vector3(0, 40, 40);
				Vector3 target = new Vector3(0, 0, 0);
				Vector3 up = new Vector3(0, 0, 1);

				Surface texture = new Surface("bg-bricks.png");
				Surface height = new Surface("bg-bricks-heightmap.png");

				LightManager m = new LightManager();
				m.AddPointLight(new Vector3(75, 25, 75), Color.White);
				m.Enabled = true;
				m[0].AttenuationConstant = 0.0001f;
				m[0].AttenuationLinear = 0.01f;
				m[0].AttenuationQuadratic = 0f;
				m[0].Range = 200;

				double time = 0;
				double frequency = 2 * Math.PI / 5;
				const float size = 50;

				//HeightMapTerrain b = new HeightMapTerrain(height.ReadPixels());
				//b.Width = size;
				//b.Height = size;
				//b.MaxPeak = 1;
				CubeBuilder b = new CubeBuilder();
				b.Length = size;

				VertexBuffer buffer = b.CreateVertexBuffer();
				buffer.Texture = texture;

				var shader = ShaderCompiler.CompileShader(ShaderLanguage.Glsl,
					"void main() { gl_Position = ftransform(); } ",
					"uniform vec4 my_color; void main() { gl_FragColor = my_color; }");


				while (wind.IsClosed == false)
				{
					if (rotating)
					{
						time += Display.DeltaTime / 1000.0;
					}

					Display.BeginFrame();
					Display.Clear(Color.Blue);

					Display.Shader = shader;

					if (rotating)
					{
						shader.SetUniform("my_color", Color.Red);
					}
					else
					{
						shader.SetUniform("my_color", Color.Green);
					}
					Display.MatrixProjection = Matrix4.Projection(45f, 800 / 600.0f, 1.0f, 1000f);
					Display.MatrixView = Matrix4.LookAt(eye, target, up);
					Display.MatrixWorld = Matrix4.Identity;

					Display.MatrixWorld = Matrix4.Translation(-size / 2, -size / 2, 0);
					m.DoLighting();

					Display.MatrixWorld = Matrix4.RotateZ((float)(frequency * time)) * Matrix4.Translation(-size / 2, -size / 2, 0);

					buffer.Draw();

					Display.EndFrame();
					Core.KeepAlive();
				}
			}
		}

		void Mouse_MouseDown(InputEventArgs e)
		{
			rotating = !rotating;
		}

	}
}

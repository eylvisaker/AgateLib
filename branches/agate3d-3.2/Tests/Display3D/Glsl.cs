using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Geometry.Builders;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using AgateLib.InputLib;

namespace Tests.Display3D.Glsl
{
	public class Glsl: IAgateTest 
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Glsl"; }
		}

		public string Category
		{
			get { return "Display 3D"; }
		}

		void IAgateTest.Main(string[] args)
		{
			Run(args);
		}

		#endregion

		bool rotating = false;
		const float velocity = 0.04f;
		const float mousevelocity = 0.004f;
		float theta = (float)Math.PI / 2;
		float phi = (float)-Math.PI / 2;
		Vector3 eye = new Vector3(0, 70, 20);
		Vector3 up = new Vector3(0, 0, 1);
		DisplayWindow wind;

		Vector3 lookDir
		{
			get { return Vector3.FromPolar(1, theta, phi); }
		}
		Vector2 centerPoint
		{
			get { return new Vector2(wind.Width / 2, wind.Height / 2); }
		}
		void resetmouse()
		{
			Mouse.Position = new Point(wind.Width / 2, wind.Height / 2);
		}
		private void Run(string[] args)
		{
			using (AgateSetup setup = new AgateSetup(args))
			{
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				wind = DisplayWindow.CreateWindowed("GLSL", 800, 600);
				Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);

				FontSurface font = new FontSurface("Arial", 14.0f);

				Surface texture = new Surface("bg-bricks.png");
				Surface height = new Surface("bg-bricks-heightmap.png");
				//Surface height = new Surface("jellybean.png");

				LightManager m = new LightManager();
				m.AddPointLight(new Vector3(0, 0, 0), Color.White);
				m.Enabled = true;
				m.Ambient = Color.FromArgb(0, 255, 0);
				m[0].AttenuationConstant = 0.0001f;
				m[0].AttenuationLinear = 0.004f;
				m[0].AttenuationQuadratic = 0.0001f;
				m[0].Range = 200;

				double time = 0;
				double frequency = 2 * Math.PI / 5;
				const float size = 25;

				//HeightMapTerrain b = new HeightMapTerrain(height.ReadPixels());
				//b.Width = size;
				//b.Height = size;
				//b.MaxPeak = 1;
				CubeBuilder b = new CubeBuilder();
				b.VertexType = VertexLayout.PositionNormalTangentBitangentTexture;
				b.Length = size;

				VertexBuffer buffer = b.CreateVertexBuffer();
				buffer.Textures[0] = texture;
				buffer.Textures[1] = new Surface(
					PixelBuffer.NormalMapFromHeightMap(height.ReadPixels(), 2.0f));
				buffer.Textures[1].SaveTo("normal.png");

				//var shader = ShaderCompiler.CompileShader(ShaderLanguage.Glsl,
				//    Shaders.PerPixelLighting_vertex, Shaders.PerPixelLighting_fragment);
				var shader = ShaderCompiler.CompileShader(ShaderLanguage.Glsl,
					File.ReadAllText("Data/shaders/BumpMap_vertex.txt"), 
					File.ReadAllText("Data/shaders/BumpMap_fragment.txt"));
				//var shader = ShaderCompiler.CompileShader(ShaderLanguage.Glsl,
				//    Shaders.BumpMap_vertex, Shaders.PerPixelLighting_fragment);

				resetmouse();
				Mouse.Hide();

				while (wind.IsClosed == false)
				{
					if (Core.IsActive)
					{
						Vector3 move = lookDir * velocity * Display.DeltaTime;
						Vector3 strafe = Vector3.CrossProduct(move, up).Normalize() * velocity * Display.DeltaTime;
						Vector3 fly = new Vector3(0, 0, velocity * Display.DeltaTime);

						if (Keyboard.Keys[KeyCode.W]) eye += move;
						else if (Keyboard.Keys[KeyCode.S]) eye -= move;

						if (Keyboard.Keys[KeyCode.A]) eye -= strafe;
						else if (Keyboard.Keys[KeyCode.D]) eye += strafe;

						if (Keyboard.Keys[KeyCode.Q]) eye += fly;
						else if (Keyboard.Keys[KeyCode.E]) eye -= fly;

						if (Keyboard.Keys[KeyCode.Escape])
							return;

						Vector2 mouseDiff = new Vector2(Mouse.X, Mouse.Y) - centerPoint;

						theta += mouseDiff.Y * mousevelocity;
						phi -= mouseDiff.X * mousevelocity;
						resetmouse();

						if (phi < -Math.PI) phi += (float)(Math.PI * 2);
						if (phi > Math.PI) phi -= (float)(Math.PI * 2);
						if (theta < 0) theta = 0;
						if (theta > Math.PI) theta = (float)Math.PI;
					}

					if (rotating)
					{
						time += Display.DeltaTime / 1000.0;
					}

					Display.BeginFrame();
					Display.Clear(Color.Gray);

					Display.Shader = shader;

					Display.MatrixProjection = Matrix4.Projection(45f, wind.Width / (float)wind.Height, 1.0f, 1000f);
					Display.MatrixWorld = Matrix4.Identity;
					Display.MatrixView = Matrix4.Identity;

					Display.MatrixView = Matrix4.LookAt(eye, eye + lookDir, up);

					m[0].Position = eye;
					m.DoLighting();

					Display.MatrixWorld =
						Matrix4.Translation(-size / 2, -size / 2, 0) * Matrix4.RotateZ((float)(frequency * time));

					buffer.Draw();

					Debug.Print("x, y, z = {0}", eye.ToString());
					Debug.Print("angle = {0}", phi * 180 / Math.PI);

					Display.EndFrame();
					Core.KeepAlive();
				}
			}
		}

		void Mouse_MouseDown(InputEventArgs e)
		{
			if (e.MouseButtons == Mouse.MouseButtons.Secondary)
			{
				rotating = !rotating;
			}
		}

	}
}

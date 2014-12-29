using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Platform.WinForms.Resources;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Testing.Shaders
{
	class Lighting3DTest : IAgateTest 
	{

		public string Name
		{
			get { return "Lighting 3D"; }
		}

		public string Category
		{
			get { return "Shaders"; }
		}

		Vector3 eye = new Vector3(-3, 0, 4);
		Vector3 up = new Vector3(0, 0, 1);
		Vector3 lightPos = new Vector3(0, 1.5, 2.5);
		double lookAngle = 0;
		double angle = 0;
		double lightAngle = 0;
		bool lightEnable = true;
		bool paused = false;
		bool advance = false;
		bool done = false;

		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				DisplayWindow wind = DisplayWindow.CreateWindowed("Lighting 3D", 640, 480);
				Font font = DefaultAssets.Fonts.AgateSans;
				Surface texture = new Surface("bg-bricks.png");

				AgateLib.Geometry.Builders.CubeBuilder cb = new AgateLib.Geometry.Builders.CubeBuilder();
				cb.Location = new Vector3(0, 0, 0);
				cb.Length = 1;
				cb.CreateVertexBuffer();

				Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

				int frameCount = 0;

				AgateLib.Geometry.Builders.CubeBuilder lightMesh = new AgateLib.Geometry.Builders.CubeBuilder();
				lightMesh.Length = 0.02f;
				lightMesh.CreateVertexBuffer();

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.Blue);

					var shader = AgateBuiltInShaders.Lighting3D;
					shader.AmbientLight = Color.Black;

					if (lightEnable)
					{
						lightPos.X = 2f * (float)Math.Sin(lightAngle);
						lightPos.Y = 2f * (float)Math.Cos(lightAngle);

						shader.EnableLighting = true;
						shader.Lights[0] = new Light();
						shader.Lights[0].Position = lightPos;
						shader.Lights[0].DiffuseColor = Color.White;
						shader.Lights[0].AttenuationConstant = 1f;
						shader.Lights[0].AttenuationLinear = 0.1f;
						shader.Lights[0].AttenuationQuadratic = 0.03f;
					}
					else
						shader.EnableLighting = false;

					Vector3 dir = LookDir;
					Vector3 target = eye + dir * 3;
					target.Z = 0;

					shader.Projection = Matrix4x4.Projection(45, 640 / 480.0f, 0.1f, 200);
					shader.View = Matrix4x4.ViewLookAt(eye, target, up);
					shader.World = Matrix4x4.RotateZ(angle);
					shader.Activate();

					cb.VertexBuffer.Textures[0] = texture;
					cb.VertexBuffer.DrawIndexed(cb.IndexBuffer);

					shader.EnableLighting = false;
					shader.World = Matrix4x4.Translation(lightPos);
					shader.Activate();

					lightMesh.VertexBuffer.Textures[0] = texture;
					lightMesh.VertexBuffer.DrawIndexed(lightMesh.IndexBuffer);

					AgateBuiltInShaders.Basic2DShader.Activate();

					font.DrawText(0, 0, "Location: {0}", eye);
					font.DrawText(0, font.FontHeight, "Angle: {0}", lookAngle);
					font.DrawText(0, font.FontHeight * 2, "Press L to toggle lighting effects.");

					Display.EndFrame();
					Core.KeepAlive();

					angle += 1 * Display.DeltaTime / 1000.0f;
					lightAngle += 2.1 * Display.DeltaTime / 1000.0f;

					if (angle > 6 * Math.PI) angle -= 6 * Math.PI;
					if (lightAngle > 6 * Math.PI) lightAngle -= 6 * Math.PI;

					frameCount++;

					if (paused)
					{
						while (paused && advance == false && done == false && wind.IsClosed == false)
							Core.KeepAlive();

						advance = false;
					}

					if (done)
						break;
				}
			});
		}

		private Vector3 LookDir
		{
			get
			{
				Vector3 dir = new Vector3(Math.Cos(lookAngle), Math.Sin(lookAngle), 0);

				return dir;
			}
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			const float speed = 3f;

			switch (e.KeyCode)
			{
				case KeyCode.Up: eye += (float)(speed * Display.DeltaTime / 1000.0f) * LookDir; break;
				case KeyCode.Down: eye -= (float)(speed * Display.DeltaTime / 1000.0f) * LookDir; break;
				case KeyCode.Left: lookAngle += (float)(speed * Display.DeltaTime / 1000.0f); break;
				case KeyCode.Right: lookAngle -= (float)(speed * Display.DeltaTime / 1000.0f); break;
				case KeyCode.L: lightEnable = !lightEnable; break;
				case KeyCode.P: paused = !paused; break;
				case KeyCode.A: advance = true; break;
				case KeyCode.Escape: done = true; break;
			}
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.InputLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry.Builders;

namespace AgateLib.Tests.Shaders
{
	class Lighting3DTest : IAgateTest
	{
		Vector3f eye = new Vector3f(-3, 0, 4);
		Vector3f up = new Vector3f(0, 0, 1);
		Vector3f lightPos = new Vector3f(0, 1.5, 2.5);
		double lookAngle = 0;
		double angle = 0;
		double lightAngle = 0;
		bool lightEnable = true;
		bool paused = false;
		bool advance = false;
		bool done = false;

		private Vector3f LookDir => new Vector3f(Math.Cos(lookAngle), Math.Sin(lookAngle), 0);

		public string Name => "Lighting 3D";

		public string Category => "Shaders";

		public void Run(string[] args)
		{
			IFont font = Font.AgateSans;
			Surface texture = new Surface("bg-bricks.png");

			CubeBuilder cb = new CubeBuilder();
			cb.Location = Vector3.Zero;
			cb.Length = 1;
			cb.CreateVertexBuffer();

			Input.Unhandled.KeyDown += Keyboard_KeyDown;

			int frameCount = 0;

			CubeBuilder lightMesh = new CubeBuilder();
			lightMesh.Length = 0.02f;
			lightMesh.CreateVertexBuffer();

			while (AgateApp.IsAlive)
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

				Vector3f dir = LookDir;
				Vector3f target = eye + dir * 3;
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
				AgateApp.KeepAlive();

				angle += 1 * AgateApp.GameClock.Elapsed.TotalSeconds;
				lightAngle += 2.1 * AgateApp.GameClock.Elapsed.TotalSeconds;

				if (angle > 6 * Math.PI) angle -= 6 * Math.PI;
				if (lightAngle > 6 * Math.PI) lightAngle -= 6 * Math.PI;

				frameCount++;

				if (paused)
				{
					while (paused && advance == false && done == false && AgateApp.IsAlive)
						AgateApp.KeepAlive();

					advance = false;
				}

				if (done)
					break;
			}
		}

		void Keyboard_KeyDown(object sender, AgateInputEventArgs e)
		{
			const float speed = 3f;

			switch (e.KeyCode)
			{
				case KeyCode.Up: eye += (float)(speed * AgateApp.GameClock.Elapsed.TotalSeconds) * LookDir; break;
				case KeyCode.Down: eye -= (float)(speed * AgateApp.GameClock.Elapsed.TotalSeconds) * LookDir; break;
				case KeyCode.Left: lookAngle += (float)(speed * AgateApp.GameClock.Elapsed.TotalSeconds); break;
				case KeyCode.Right: lookAngle -= (float)(speed * AgateApp.GameClock.Elapsed.TotalSeconds); break;
				case KeyCode.L: lightEnable = !lightEnable; break;
				case KeyCode.P: paused = !paused; break;
				case KeyCode.A: advance = true; break;
				case KeyCode.Escape: done = true; break;
			}
		}
	}
}

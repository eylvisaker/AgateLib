﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.InputLib;
using AgateLib.Geometry;

namespace Tests.Shaders
{
	class Lighting3DTest : IAgateTest 
	{
		#region IAgateTest Members

		public string Name
		{
			get { return "Lighting 3D"; }
		}

		public string Category
		{
			get { return "Shaders"; }
		}

		Vector3 eye = new Vector3(-3, 0, 2);
		Vector3 up = new Vector3(0, 0, 1);
		double lookAngle = 0;
		double angle = 0;
		bool lightEnable;

		public void Main(string[] args)
		{
			using (var setup = new AgateSetup())
			{
				setup.AskUser = true;
				setup.Initialize(true, false, false);
				if (setup.WasCanceled)
					return;

				Surface texture = new Surface("bg-bricks.png");
				DisplayWindow wind = DisplayWindow.CreateWindowed("Lighting 3D", 640, 480);
				FontSurface font = new FontSurface("Times", 14);

				AgateLib.Geometry.Builders.CubeBuilder cb = new AgateLib.Geometry.Builders.CubeBuilder();
				cb.Location = new Vector3();
				cb.Length = 1;
				cb.CreateVertexBuffer();

				Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

				int frameCount = 0;

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.Blue);

					font.DrawText(0, 0, "Location: {0}", eye);
					font.DrawText(0, font.FontHeight, "Angle: {0}", lookAngle);

					var shader = AgateBuiltInShaders.Lighting3D;
					shader.AmbientLight = Color.Black;

					if (lightEnable)
					{
						shader.Lights[0] = new Light();
						shader.Lights[0].Position = new Vector3(0, 3, eye.Z);
						shader.Lights[0].DiffuseColor = Color.White;
						shader.Lights[0].AttenuationConstant = 1f;
						shader.Lights[0].AttenuationLinear = 0.1f;
						shader.Lights[0].AttenuationQuadratic = 0.03f;
					}
					else
						shader.Lights[0] = null;

					Vector3 dir = LookDir;
					Vector3 target = eye + dir * 3;
					target.Z = 0;

					shader.Projection = Matrix4x4.Projection(45, 640 / 480.0f, 1, 200);
					shader.View = Matrix4x4.ViewLookAt(eye, target, up);
					shader.World = Matrix4x4.RotateZ(angle);
					shader.Activate();

					cb.VertexBuffer.Textures[0] = texture;
					cb.VertexBuffer.DrawIndexed(cb.IndexBuffer);

					AgateBuiltInShaders.Basic2DShader.Activate();

					Display.EndFrame();
					Core.KeepAlive();

					angle += 1 * Display.DeltaTime / 1000.0f;
					if (angle > 6 * Math.PI) angle -= 6 * Math.PI;

					frameCount++;

				}
			}
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
			}
		}

		#endregion
	}
}

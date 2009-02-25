﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.Geometry.Builders;
using AgateLib.DisplayLib;
using AgateLib.InputLib;

namespace Test3D
{
    class Test3d
    {
        static  int matrixIndex = 1;
        static int move, turn;

        static void Main(string[] args)
        {
            System.IO.FileStream fs = new System.IO.FileStream(@"c:\cube.obj",System.IO.FileMode.Open, System.IO.FileAccess.Read);

            AgateLib.Meshes.Loaders.Obj.OBJFileRepresentation objRep = new AgateLib.Meshes.Loaders.Obj.OBJFileRepresentation(fs);

            return;

            // These two lines are used by AgateLib tests to locate
            // driver plugins and images.
            AgateLib.Utility.AgateFileProvider.AssemblyProvider.AddPath("../Drivers");
            AgateLib.Utility.AgateFileProvider.ImageProvider.AddPath("../../../Tests/TestImages");

            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.WasCanceled)
                    return;

                DisplayWindow wind = new DisplayWindow(CreateWindowParams.Windowed(
                    "Test3D", 800, 600, null, true));

                Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
                Keyboard.KeyUp += new InputEventHandler(Keyboard_KeyUp);
                Surface surf = new Surface("bg-bricks.png");

                Cube cube = new Cube();
                cube.GenerateTextureCoords = true;
                cube.Length = 4;

                VertexBuffer b = cube.CreateVertexBuffer();
                b.Texture = surf;

                int lastMyMatrix = matrixIndex-1;
                Matrix4 myproj = new Matrix4();
                Matrix4 myview = new Matrix4();

                LightManager m = new LightManager();
                m.Enabled = true;
                m.AddPointLight(new Vector3(3, 3, -0.5), Color.Gray);

                m[0].AttenuationConstant = 0.002f;
                m[0].AttenuationLinear = 0.00f;
                m[0].AttenuationQuadratic = 0.002f;

                Vector3 position = new Vector3(-5, 0, 2);
                float facingAngle = 0;
                float speed = 0.03f;
                float turnSpeed = 0.004f;

                while (wind.IsClosed == false)
                {
                    Vector3 lookDirection = CalculateLookDirection(facingAngle);

                    if (move != 0)
                    {
                        position += move * speed * lookDirection * (float)Display.DeltaTime;
                    }
                    if (turn != 0)
                    {
                        facingAngle -= turn * turnSpeed * (float) Display.DeltaTime;
                    }

                    Vector3 lookTarget = position + lookDirection;

                    Display.BeginFrame();
                    Display.Clear(Color.Gray);

                    Rectangle ortho = new Rectangle(-9, -9, 20, 20);

                    switch (matrixIndex)
                    {
                        case 0:
                            myproj = Matrix4.Ortho((RectangleF)ortho, -1, 1);
                            myview = Matrix4.Identity;

                            Display.MatrixProjection = myproj;
                            Display.MatrixView = myview;
                            Display.MatrixWorld = Matrix4.Identity;

                            break;

                        case 1:
                            myproj = Matrix4.Projection(45, wind.Width / (float)wind.Height, 1f, 100);
                            myview = Matrix4.LookAt(position, lookTarget, 
                                new Vector3(0, 0, 1));

                            Display.MatrixProjection = myproj;
                            Display.MatrixView = myview;
                            Display.MatrixWorld = Matrix4.Identity;

                            break;
                    }

                    if (lastMyMatrix != matrixIndex)
                    {
                        Debug.Write("=-=-=-=-=-=-=-=-=-=-=-=--=-=-=-=-=-=-=-=-=-=");
                        Debug.Print("Matrix Index: {0}", matrixIndex);

                        //DisplayMatrices(GetPName.ProjectionMatrix, myproj, false);
                        //DisplayMatrices(GetPName.ModelviewMatrix, myview, false);
                    }

                    lastMyMatrix = matrixIndex;

                    Display.MatrixWorld = Matrix4.Translation(0.5, 0.5, 0.5);

                    m[0].Position = position;
                    m.DoLighting();

                    // draw a weird checkerboard
                    for (int x = 0; x < 8; x += 2)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            int sx = (y % 2 == 0) ? 1 : 0;
                            Color clr = (y / 4 == 0) ? Color.Red : Color.Blue;

                            Display.FillRect(new Rectangle(x + sx, y, 1, 1), clr);
                        }
                    }
                    Display.DrawRect(new Rectangle(0, 0, 8, 8), Color.Black);


                    b.DrawIndexed();

                    Display.EndFrame();
                    Core.KeepAlive();

                   
                }
            }
        }

        private static Vector3 CalculateLookDirection(float facingAngle)
        {
            return new Vector3(Math.Cos(facingAngle), Math.Sin(facingAngle), 0);
        }

        private static Vector3 CalculateLookTarget(Vector3 position, float facingAngle)
        {
            Vector3 lookTarget = position;
            lookTarget += new Vector3(Math.Cos(facingAngle), Math.Sin(facingAngle), 0);
            return lookTarget;
        }

        static void Keyboard_KeyUp(InputEventArgs e)
        {
            if (e.KeyCode == KeyCode.Up || e.KeyCode == KeyCode.Down)
                move = 0;
            if (e.KeyCode == KeyCode.Left || e.KeyCode == KeyCode.Right)
                turn = 0;
        }

        static void Keyboard_KeyDown(AgateLib.InputLib.InputEventArgs e)
        {
            if (e.KeyCode == AgateLib.InputLib.KeyCode.Space)
                matrixIndex++;

            if (matrixIndex > 1)
                matrixIndex = 0;

            if (e.KeyCode == KeyCode.Up)
                move = 1;
            if (e.KeyCode == KeyCode.Down)
                move = -1;
            if (e.KeyCode == KeyCode.Left)
                turn = -1;
            if (e.KeyCode == KeyCode.Right)
                turn = 1;
        }
    }
}

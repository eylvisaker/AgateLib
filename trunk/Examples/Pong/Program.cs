using System;
using System.Collections.Generic;
using ERY.AgateLib;
using ERY.AgateLib.Geometry;

namespace Pong
{
    class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        FontSurface font;
        int[] score = new int[2];
        Vector2 ball, ballvelocity;
        Vector2[] paddle = new Vector2[2];
        
        const int paddleHeight = 80;
        const int paddleWidth = 16;
        const int borderSize = paddleWidth;
        const int ballSize = paddleWidth;
        const int displayWidth = 640;
        const int playAreaWidth = 500;
        const int displayHeight = 480;
        const float paddleSpeed = 150.0f;

        Color paddleColor = Color.LightGray;
        Color ballColor = Color.LightGray;

        void Run(string[] args)
        {
            using (AgateSetup setup = new AgateSetup(args))
            {
                setup.Initialize(true, false, false);
                if (setup.Cancel)
                    return;

                DisplayWindow wind = new DisplayWindow(
                    CreateWindowParams.Windowed("Pong Example", displayWidth, displayHeight, null, false));

                font = new FontSurface("Sans Serif", 14);

                Mouse.Hide();

                paddle[0] = new Vector2(50, displayHeight / 2);
                paddle[1] = new Vector2(playAreaWidth - 50 - paddleWidth, displayHeight / 2);
                ball = new Vector2(playAreaWidth / 2, displayHeight / 2);
                ballvelocity = new Vector2(-50, 50);

                while (wind.IsClosed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.DarkGray);

                    DrawBorder();
                    DrawPaddles();
                    DrawBall();
                    DrawScore();

                    Display.EndFrame();
                    Core.KeepAlive();

                    if (Keyboard.Keys[KeyCode.Escape])
                        wind.Dispose();

                    float time_s = (float)Display.DeltaTime / 1000.0f;

                    UpdatePaddles(time_s);
                    UpdateBall(time_s);

                }
            }
        }

        private void DrawScore()
        {
            int x = playAreaWidth + borderSize * 2;
            int y = borderSize * 2;

            font.DrawText(x, y, "Score");

            for (int i = 0; i < 2; i++)
            {
                y += font.StringDisplayHeight("M") * 2;
                font.DrawText(x, y, score[i].ToString());
            }
        }

        private void UpdateBall(float time_s)
        {
            bool newBall = false;

            ball += ballvelocity * time_s;

            if (ball.Y + ballSize > displayHeight - borderSize) ballvelocity.Y *= -1;
            if (ball.Y < borderSize) ballvelocity.Y *= -1;

            if (ball.X < borderSize)
            {
                newBall = true;
                score[1]++;
            }
            else if (ball.X + ballSize > playAreaWidth)
            {
                newBall = true;
                score[0]++;
            }

            if (newBall)
            {
                ball = new Vector2(playAreaWidth / 2, displayHeight / 2);
                ballvelocity = new Vector2(-90, 90);
            }
                
            // check for paddles
            bool increaseSpeed = false;

            if (ball.X < paddle[0].X + paddleWidth)
            {
                if (ball.Y + ballSize - 1 >= paddle[0].Y &&
                    ball.Y <= paddle[0].Y + paddleHeight)
                {
                    ballvelocity.X *= -1;
                    increaseSpeed = true;
                }
            }
            if (ball.X + ballSize >= paddle[1].X)
            {
                if (ball.Y + ballSize - 1 >= paddle[1].Y &&
                    ball.Y <= paddle[1].Y + paddleHeight)
                {
                    ballvelocity.X *= -1;
                    increaseSpeed = true;
                }
            }

            if (increaseSpeed)
            {
                ballvelocity.X += Math.Sign(ballvelocity.X) * 10.0f;
                ballvelocity.Y += Math.Sign(ballvelocity.Y) * 10.0f;
            }
        }

        private void UpdatePaddles(float time_s)
        {
            float paddleMove = paddleSpeed * time_s;

            if (Keyboard.Keys[KeyCode.Down]) paddle[0].Y += paddleMove;
            if (Keyboard.Keys[KeyCode.Up]) paddle[0].Y -= paddleMove;

            // do AI
            if (ballvelocity.X > 0)
            {
                if (ball.Y + ballSize * 2 > paddle[1].Y + paddleHeight) paddle[1].Y += paddleMove;
                if (ball.Y - ballSize < paddle[1].Y) paddle[1].Y -= paddleMove;
            }

            for (int i = 0; i < 2; i++)
            {
                paddle[i].Y = Math.Max(paddle[i].Y, borderSize);
                paddle[i].Y = Math.Min(paddle[i].Y, displayHeight - borderSize - paddleHeight);
            }
        }

        private void DrawBall()
        {
            Display.FillRect(new Rectangle((int)ball.X, (int)ball.Y, ballSize, ballSize), ballColor);
        }

        private void DrawPaddles()
        {
            for (int i = 0; i < 2; i++)
            {
                Display.FillRect(
                    new Rectangle((int)paddle[i].X, (int)paddle[i].Y, paddleWidth, paddleHeight), paddleColor);
            }
        }

        private void DrawBorder()
        {
            Color borderColor = paddleColor;

            Display.FillRect(new Rectangle(0, 0, displayWidth, borderSize), borderColor);
            Display.FillRect(new Rectangle(0, 0, borderSize, displayHeight), borderColor);
            Display.FillRect(new Rectangle(0, displayHeight - borderSize, displayWidth, borderSize), borderColor);
            Display.FillRect(new Rectangle(displayWidth - borderSize, 0, borderSize, displayHeight), borderColor);
            Display.FillRect(new Rectangle(playAreaWidth - borderSize, 0, borderSize, displayHeight), borderColor);
            
        }

        void Mouse_MouseMove(InputEventArgs e)
        {
            int deltaY = e.MousePosition.Y - displayHeight / 2;

            paddle[0].Y += deltaY;

            Mouse.Position = new Point(displayWidth / 2, displayHeight / 2);
        }
    }
}
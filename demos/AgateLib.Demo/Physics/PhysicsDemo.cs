using AgateLib.Display;
using AgateLib.Input;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Solvers;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Demo.Physics
{
    public abstract class PhysicsDemo : IDemo
    {
        IKinematicsExample example;
        private KinematicsHistory history;
        private KeyboardEvents keyboard;
        private GamePadEvents gamepad;
        private Canvas canvas;
        private IFontProvider fonts;
        private KinematicsSystem system = new KinematicsSystem();
        private KinematicsIntegrator kinematics;
        private KinematicsInfoDisplay infoDisplay;
        private IConstraintSolver constraintSolver;
        private InputState inputState = new InputState();

        private bool running = true;

        protected PhysicsDemo(IKinematicsExample example)
        {
            this.example = example;
            this.history = new KinematicsHistory();

            this.keyboard = new KeyboardEvents();
            this.gamepad = new GamePadEvents(PlayerIndex.One);

            keyboard.KeyDown += Keyboard_KeyDown;
            gamepad.ButtonReleased += Gamepad_ButtonReleased;
        }

        public string Name => example.Name;

        public string Category => "Physics";

        public Rectangle ScreenArea { get; set; }

        private IContentProvider content;

        public event Action OnExit;

        public void Draw(GameTime gameTime)
        {
            canvas.Begin();

            example.Draw(canvas);

            infoDisplay?.Draw(canvas);

            canvas.End();
        }

        public void Initialize(IDemoResources resources)
        {
            canvas = new Canvas(resources.GraphicsDevice);

            fonts = resources.Fonts;
            ScreenArea = resources.ScreenArea;
            content = resources.Content;

            ScreenArea = new Rectangle(0, 0, 1024, 768);

            InitializeExample();
        }

        private void InitializeExample()
        {
            system = example.Initialize(ScreenArea.Size, content);

            constraintSolver = new ImpulseConstraintSolverV2(system);
            kinematics = new KinematicsIntegrator(system, constraintSolver);

            this.infoDisplay = new KinematicsInfoDisplay(example, constraintSolver, system, history, fonts);

            history.Clear();
            history.AdvanceAndStoreHistory(system.Particles);
        }

        private void Gamepad_ButtonReleased(object sender, GamepadButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Buttons.Y:
                    InitializeExample();
                    break;

                case Buttons.A:
                    running = !running;
                    break;

                case Buttons.B:
                    running = false;
                    Advance();
                    break;

                case Buttons.X:
                    infoDisplay.DebugInfoPage++;
                    break;

                case Buttons.LeftShoulder:
                    example.RemoveParticle();
                    InitializeExample();
                    break;

                case Buttons.RightShoulder:
                    example.AddParticle();
                    InitializeExample();
                    break;

                case Buttons.LeftTrigger:
                    history.Index--;
                    LoadHistory();
                    break;

                case Buttons.RightTrigger:
                    history.Index++;
                    LoadHistory();
                    break;

                case Buttons.Start:
                    OnExit?.Invoke();
                    break;
            }
        }

        private void Keyboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.Z)
                InitializeExample();
            if (e.Key == Keys.Space && !running)
                Advance();
            if (e.Key == Keys.Enter || e.Key == Keys.Enter)
                running = !running;

            if (e.Key == Keys.Escape)
                OnExit?.Invoke();

            if (e.Key == Keys.OemPlus)
            {
                example.AddParticle();
                InitializeExample();
            }
            if (e.Key == Keys.OemMinus)
            {
                example.RemoveParticle();
                InitializeExample();
            }
            if (e.Key == Keys.Down)
            {
                history.Index--;

                LoadHistory();
            }
            if (e.Key == Keys.Up)
            {
                history.Index++;

                LoadHistory();
            }

            if (e.Key >= Keys.D1 && e.Key <= Keys.D2)
            {
                infoDisplay.DebugInfoPage = (int)(e.Key - Keys.D1);
            }
        }

        public void Update(GameTime gameTime)
        {
            inputState.Update(gameTime);

            keyboard.Update(gameTime);
            gamepad.Update(inputState);

            if (system.Particles.Any(p => double.IsNaN(p.Position.X)))
                running = false;

            if (running)
                Advance((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void Advance(float dt = 0.005f)
        {
            example.ComputeExternalForces();

            kinematics.Integrate(dt);

            history.AdvanceAndStoreHistory(system.Particles);
        }

        private void LoadHistory()
        {
            IReadOnlyList<PhysicalParticle> historyItem = history.State;

            for (int i = 0; i < system.Particles.Count; i++)
            {
                historyItem[i].CopyTo(system.Particles[i]);
            }

            constraintSolver.ComputeConstraintForces(0.005f);
        }
    }
}

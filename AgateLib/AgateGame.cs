using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.DisplayLib;

namespace AgateLib
{
	/// <summary>
	/// Application framework for a game.  Using AgateGame can simplify the application 
	/// source code and start up.
	/// </summary>
	/// <example>
	/// <code>
	/// using AgateLib;
	/// 
	/// namespace MyApplication
	/// {
	///		class MyApp : AgateGame
	///		{
	///			public static void Main(string[] args)
	///			{
	///				new MyApp().Run(args);
	///			}
	///			
	/// 		protected override void Initialize()
	/// 		{
	/// 			// do any needed initialization, loading of surfaces, etc. here.
	///			}
	///			protected override void Update(double time_ms)
	///			{
	///				// update game logic here.
	///			}
	///			protected override void Render()
	///			{
	///				// render game here. 
	///			}
	///		}
	/// }
	/// </code>
	/// </example>
	public class AgateGame
	{
		DisplayWindow mWindow;
		AppInitParameters mInitParams;
		FontSurface font;
		Gui.GuiRoot mGui;

		double totalSplashTime = 0;
		bool splashFadeDone = false;

		#region --- Run Method ---

		/// <summary>
		/// Runs the application.
		/// </summary>
		/// <returns></returns>
		public int Run()
		{
			return Run(null);
		}
		/// <summary>
		/// Runs the application.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public int Run(string[] args)
		{
			using (AgateSetup setup = new AgateSetup(args))
			{
				setup.PreferredDisplay = InitParams.PreferredDisplay;
				setup.PreferredAudio = InitParams.PreferredAudio;
				setup.PreferredInput = InitParams.PreferredInput;

				setup.Initialize(
					InitParams.InitializeDisplay,
					InitParams.InitializeAudio,
					InitParams.InitializeJoysticks);

				if (setup.WasCanceled)
					return 1;

				CreateDisplayWindow();

				font = FontSurface.Gentium12;

				if (InitParams.ShowSplashScreen)
				{
					DoSplash();
				}

				Initialize();

				while (MainWindow.IsClosed == false)
				{
					Update(Display.DeltaTime);

					if (GuiRoot != null)
						GuiRoot.DoUpdate();

					Display.RenderTarget = mWindow;
					Display.BeginFrame();

					Render();

					if (GuiRoot != null)
						GuiRoot.Draw();

					Display.EndFrame();
					Core.KeepAlive();
				}
			}

			return 0;
		}

		private AppInitParameters InitParams
		{
			get
			{
				if (mInitParams == null)
				{
					mInitParams = GetAppInitParameters();
					AdjustAppInitParameters(ref mInitParams);
				}
				return mInitParams;
			}
		}

		#endregion
		#region --- Initialization ---

		/// <summary>
		/// Override Initialize() to do any needed initialization for your application,
		/// such as creating <see cref="AgateLib.DisplayLib.Surface">Surface</see> objects.
		/// </summary>
		protected virtual void Initialize()
		{
		}

		private void CreateDisplayWindow()
		{
			CreateWindowParams windp;

			if (FullScreen)
			{
				windp = CreateWindowParams.FullScreen(ApplicationTitle,
					WindowSize.Width, WindowSize.Height, 32);

				windp.IconFile = InitParams.IconFile;
			}
			else
			{
				windp = CreateWindowParams.Windowed(ApplicationTitle,
					WindowSize.Width, WindowSize.Height,
					InitParams.AllowResize, InitParams.IconFile);
			}

			mWindow = new DisplayWindow(windp);
		}

		#endregion
		#region --- Rendering and Updating ---

		/// <summary>
		/// Override this method to update your game logic.
		/// </summary>
		/// <param name="time_ms">The amount of time that has passed since the last
		/// update, in milliseconds.</param>
		protected virtual void Update(double time_ms)
		{

		}

		/// <summary>
		/// Override this method to render your game.
		///  Do not call Display.BeginFrame or Display.EndFrame.
		/// </summary>
		/// <remarks>Do not call Display.BeginFrame or Display.EndFrame or change
		/// render targets in this method.</remarks>
		protected virtual void Render()
		{
			RenderSplashScreen();

			if (splashFadeDone)
			{
				Surface powered = InternalResources.Data.PoweredBy;
				Size size = powered.SurfaceSize;

				int bottom = MainWindow.Height - size.Height;
				int h = font.FontHeight;

				font.DisplayAlignment = OriginAlignment.BottomLeft;
				font.Color = Color.Black;

				font.DrawText(0, bottom - 2 * h, "Welcome to AgateLib.");
				font.DrawText(0, bottom - h, "Your application framework is ready.");
				font.DrawText(0, bottom, "Override the Render method in order to do your own drawing.");
			}
		}

		#endregion
		#region --- Splash Screen ---

		private void DoSplash()
		{
			while (UpdateSplashScreen(Display.DeltaTime) == true)
			{
				Display.BeginFrame();

				RenderSplashScreen();

				Display.EndFrame();
				Core.KeepAlive();

				System.Threading.Thread.Sleep(0);

				if (MainWindow.IsClosed)
					return;
			}
		}

		/// <summary>
		/// Override this method to update the logic for a custom splash screen,
		/// if there are any animations.
		/// Return false to cancel the splash screen and move on.
		/// </summary>
		/// <param name="time_ms"></param>
		protected virtual bool UpdateSplashScreen(double time_ms)
		{
			totalSplashTime += time_ms / 1000.0;

			if (totalSplashTime > 3.0)
				return false;
			else
				return true;
		}
		/// <summary>
		/// Override this method to provide a custom splash screen.  This method
		/// is called before the Initialize method is called.
		/// </summary>
		/// <param name="time_ms"></param>
		protected virtual void RenderSplashScreen()
		{
			Display.Clear(Color.White);

			Surface powered = InternalResources.Data.PoweredBy;
			Size size = powered.SurfaceSize;

			int left = (int)(totalSplashTime * size.Width - size.Width) + 1;
			Rectangle gradientRect = new Rectangle(left, MainWindow.Height - size.Height,
				size.Width, size.Height);

			if (left < 0)
				Display.PushClipRect(gradientRect);
			else if (left > size.Width)
				splashFadeDone = true;

			powered.DisplayAlignment = OriginAlignment.BottomLeft;
			powered.Draw(0, MainWindow.Height);

			Gradient g = new Gradient(
				Color.FromArgb(0, Color.White),
				Color.White,
				Color.FromArgb(0, Color.White),
				Color.White);

			Display.FillRect(gradientRect, g);
			if (left < 0)
			{
				Display.PopClipRect();
			}
		}

		#endregion

		#region --- Virtual Properties ---

		/// <summary>
		/// Override this to set the title of the window which is created.
		/// </summary>
		protected virtual string ApplicationTitle
		{
			get { return "AgateLib Application"; }
		}
		/// <summary>
		/// Gets the initialization parameters.
		/// </summary>
		/// <returns></returns>
		[Obsolete("Override AdjustAppInitParameters")]
		protected virtual AppInitParameters GetAppInitParameters()
		{
			return new AppInitParameters();
		}
		/// <summary>
		/// Adjusts the initialization parameters.
		/// </summary>
		/// <param name="initParams"></param>
		protected virtual void AdjustAppInitParameters(ref AppInitParameters initParams)
		{

		}

		protected virtual Size WindowSize { get { return new Size(800, 600); } }
		protected virtual bool FullScreen { get { return false; } }

		#endregion
		#region --- Public Properties ---

		/// <summary>
		/// Gets the main display window.
		/// </summary>
		public DisplayWindow MainWindow
		{
			get { return mWindow; }
		}

		public Gui.GuiRoot GuiRoot
		{
			get { return mGui; }
			set
			{
				if (value == null && mGui == null)
					return;

				if (mGui != null)
					mGui.EnableInteraction = false;

				mGui = value;

				if (mGui != null)
					mGui.EnableInteraction = true;
			}
		}

		#endregion

		/// <summary>
		/// Closes the main window.
		/// </summary>
		public void Quit()
		{
			MainWindow.Dispose();
		}
	}
}

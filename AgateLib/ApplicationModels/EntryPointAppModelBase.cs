using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels
{
	/// <summary>
	/// Provides a base class for application models that use a function
	/// as an entry point.
	/// </summary>
	[Obsolete("Use new AgateSetup object instead.", true)]
	public abstract class EntryPointAppModelBase : AgateAppModel
	{

		public EntryPointAppModelBase(ModelParameters parameters)
			: base(parameters)
		{ }

		/// <summary>
		/// Runs the application model with the specified entry point for your application.
		/// </summary>
		/// <param name="entry">A delegate which will be called to run your application.</param>
		/// <returns>Returns 0.</returns>
		public int Run(Action entry)
		{
			return RunImpl(entry);
		}
		/// <summary>
		/// Runs the application model with the specified entry point for your application.
		/// </summary>
		/// <param name="entry">A delegate which will be called to run your application.</param>
		/// <returns>Returns the return value from the <c>entry</c> parameter.</returns>
		public int Run(Func<int> entry)
		{
			return RunImpl(entry);
		}
		/// <summary>
		/// Runs the application model with the specified entry point and command line arguments for your application.
		/// </summary>
		/// <param name="args">The command arguments to process.</param>
		/// <param name="entry">A delegate which will be called to run your application.</param>
		/// <returns>Returns 0.</returns>
		public int Run(string[] args, Action entry)
		{
			Parameters.Arguments = args;

			return RunImpl(entry);
		}
		/// <summary>
		/// Runs the application model with the specified entry point and command line arguments for your application.
		/// </summary>
		/// <param name="args">The command arguments to process.</param>
		/// <param name="entry">A delegate which will be called to run your application.</param>
		/// <returns>Returns the return value from the <c>entry</c> parameter.</returns>
		public int Run(string[] args, Func<int> entry)
		{
			Parameters.Arguments = args;

			return RunImpl(entry);
		}

		private int RunImpl(Action entry)
		{
			return RunImpl(ActionToFunc(entry));
		}
		/// <summary>
		/// Runs the application model by calling RunModel. If you override this, make
		/// sure to catch the ExitGameException and return 0 in the exception handler.
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		protected virtual int RunImpl(Func<int> entry)
		{
			try
			{
				return RunModel(entry);
			}
			catch (ExitGameException)
			{
				return 0;
			}
		}

		/// <summary>
		/// Runs the application model.
		/// </summary>
		/// <param name="entryPoint"></param>
		/// <returns></returns>
		protected int RunModel(Func<int> entryPoint)
		{
			try
			{
				Initialize();
				AutoCreateDisplayWindow();
				PrerunInitialization();

				int result = BeginModel(entryPoint);

				return result;
			}
			finally
			{
				//DisposeAutoCreatedWindow();

				//Dispose();
			}
		}

		/// <summary>
		/// Override this to implement the application model. This function
		/// should call the entry point and return its return value.
		/// It should not catch ExitGameException.
		/// </summary>
		/// <param name="entryPoint">The application entry point to call.</param>
		/// <returns></returns>
		protected abstract int BeginModel(Func<int> entryPoint);

		static Func<int> ActionToFunc(Action entry)
		{
			return () => { entry(); return 0; };
		}

	}
}

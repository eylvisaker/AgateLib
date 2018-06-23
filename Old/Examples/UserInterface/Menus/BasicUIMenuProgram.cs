using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;

namespace Examples.UserInterface.Menus
{
	static class BasicUIMenuProgram
	{
		/// <summary>
		/// This class is used to hold the user
		/// interface. We declare properties for
		/// the controls we wish to interact with.
		/// </summary>
		class MyFacet : IUserInterfaceFacet
		{
			public Menu MainMenu { get; set; }
			public MenuItem StartItem { get; set; }
			public MenuItem QuitItem { get; set; }
			public Label StartLabel { get; set; }

			/// <summary>
			/// The FacetName property must match
			/// the name of the facet in the resources
			/// file.
			/// </summary>
			public string FacetName => "MainMenu";

			/// <summary>
			/// This holds the root object for the 
			/// user interface. It is set by the
			/// resource manager.
			/// </summary>
			public FacetScene InterfaceRoot { get; set; }
		}

		[STAThread]
		static void Main(string[] args)
		{
			using (new AgateWinForms(args)
					.AssetPath("Assets")
					.Initialize())
			using (new DisplayWindowBuilder(args)
					.BackbufferSize(500, 400)
					.Build())
			{
				// Load the resource file and initialize the resource manager.
				var resourceLoader = new ResourceDataLoader();
				var resourceManager = new AgateResourceManager(resourceLoader.Load("resources.yaml"));

				// Create the object which will hold the UI.
				var facet = new MyFacet();

				resourceManager.InitializeContainer(facet);

				// If the user hits escape or the B button on a game controller,
				// exit the program.
				facet.MainMenu.MenuCancel += (sender, e) => { AgateApp.IsAlive = false; };

				// Attach a handler to each menu item if the user
				// hits enter, the A button on a game controller, or 
				// clicks it with the mouse.
				facet.StartItem.PressAccept += (sender, e) =>
				{
					facet.StartLabel.Text = "Thanks for starting the game!";
				};

				facet.QuitItem.PressAccept += (sender, e) =>
				{
					AgateApp.IsAlive = false;
				};

				// Install the user interface so that it captures input.
				Input.Handlers.Add(facet.InterfaceRoot);

				// Run the game loop
				while (AgateApp.IsAlive)
				{
					// Update the UI outside a BeginFrame..EndFrame section.
					facet.InterfaceRoot.OnUpdate(AgateApp.ApplicationClock.Elapsed, true);

					Display.BeginFrame();
					Display.Clear(Color.Gray);

					// Draw the UI as the last thing before Display.EndFrame.
					facet.InterfaceRoot.Draw();

					Display.EndFrame();
					AgateApp.KeepAlive();
				}
			}
		}
	}
}

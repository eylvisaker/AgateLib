﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Tests.UserInterfaceTests
{
	public class MenuScroll : Scene, IAgateTest
	{
		class Facet : IUserInterfaceFacet
		{
			[BindTo("MedievalSharp")]
			public IFont Font { get; set; }

			public Window Window { get; set; }

			public Menu Menu { get; set; }

			public Label DebugLabel { get; set; }

			public Gui InterfaceRoot { get; set; }

			public string FacetName => "MenuScroll";
		}

		AgateResourceManager resources;
		Facet facet = new Facet();

		public string Name => "Menu Scroll";

		public string Category => "User Interface";

		public override void Update(TimeSpan elapsed)
		{
			facet.InterfaceRoot.OnUpdate(elapsed, true);
		}

		public override void Draw()
		{
			Display.Clear(Color.Purple);
			facet.InterfaceRoot.Draw();
		}

		public void CreateGui()
		{
			resources = new AgateResourceManager("UserInterface/MenuScroll.yaml");
			facet = new Facet();

			resources.InitializeContainer(facet);

			Input.Handlers.Add(facet.InterfaceRoot);
		}
		protected override void OnSceneStart()
		{
			CreateGui();
		}


		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				SceneStack.Start(this);
			}
		}
	}
}

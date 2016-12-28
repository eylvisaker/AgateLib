using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Tests.UserInterfaceTests
{
	public class NonWrappingLayoutTest : Scene, IAgateTest
	{
		class Facet : IUserInterfaceFacet
		{
			public IFont Font { get; set; }

			public Window window_1 { get; set; }
			public Window window_2 { get; set; }
			public Menu menu_1 { get; set; }

			public Label label_1 { get; set; }

			public Gui InterfaceRoot { get; set; }

			public string FacetName { get { return "NonWrappingLayout"; } }
		}

		AgateResourceManager resources;
		Facet facet = new Facet();

		public string Name
		{
			get { return "Non Wrapping Layout"; }
		}

		public string Category
		{
			get { return "User Interface"; }
		}

		public AgateConfig Configuration { get; set; }

		public override void Update(double deltaT)
		{
			facet.InterfaceRoot.OnUpdate(deltaT, true);
		}

		public override void Draw()
		{
			Display.Clear(Color.Purple);
			facet.InterfaceRoot.Draw();
		}

		public void CreateGui()
		{
			resources = new AgateResourceManager("NonWrappingLayout.yaml");
			facet = new Facet();

			resources.UserInterface.InitializeFacet(facet);

			facet.menu_1.SelectedItemChanged += (sender, args) =>
			{
				var pt = facet.label_1.ClientToScreen(Point.Empty);

				facet.label_1.Text = $"Client Rect: {pt.X},{pt.Y},{facet.label_1.Width},{facet.label_1.Height}";
			};

			Input.Handlers.Add(facet.InterfaceRoot);
		}

		public void ModifySetup(IAgateSetup setup)
		{
		}

		public void Run()
		{
			SceneStack.Start(this);
		}

		protected override void OnSceneStart()
		{
			CreateGui();
		}
	}
}

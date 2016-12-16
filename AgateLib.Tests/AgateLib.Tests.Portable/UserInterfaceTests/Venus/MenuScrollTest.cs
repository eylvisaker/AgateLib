using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.Testing.UserInterfaceTests
{
	public class MenuScroll : Scene, ISceneModelTest
	{
		class Facet : IUserInterfaceFacet
		{
			[BindTo("MedievalSharp")]
			public IFont Font { get; set; }

			public Window Window { get; set; }

			public Menu Menu { get; set; }

			public Label DebugLabel { get; set; }

			public Gui InterfaceRoot { get; set; }

			public string FacetName { get { return "MenuScroll"; } }
		}

		AgateResourceManager resources;
		Facet facet = new Facet();

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}

		public Scene StartScene
		{
			get { return this; }
		}

		public string Name
		{
			get { return "Menu Scroll"; }
		}

		public string Category
		{
			get { return "User Interface"; }
		}

		protected override void OnSceneStart()
		{
			CreateGui();
		}

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
			resources = new AgateResourceManager("MenuScroll.yaml");
			facet = new Facet();

			resources.InitializeContainer(facet);

			Input.InputHandlers.Add(facet.InterfaceRoot);

			facet.Menu.SelectedItemChanged += (sender, args) =>
			{
				facet.DebugLabel.Text = $"Menu.ScrollRow: {facet.Menu.ScrollRow}\nMenu.ScrollOffset: {facet.Menu.ScrollOffset}";
			};

		}
	}
}

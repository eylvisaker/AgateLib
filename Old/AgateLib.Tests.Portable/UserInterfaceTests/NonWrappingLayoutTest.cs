using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform;
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

			public FacetScene InterfaceRoot { get; set; }

			public string FacetName => "NonWrappingLayout";
		}

		AgateResourceManager resources;
		Facet facet = new Facet();

		public string Name => "Non Wrapping Layout";

		public string Category => "User Interface";

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
			facet.InterfaceRoot.OnUpdate(gameClockElapsed, true);
		}

		protected override void OnRedraw()
		{
			Display.Clear(Color.Purple);
			facet.InterfaceRoot.Draw();
		}

		public void CreateGui()
		{
			resources = new AgateResourceManager("UserInterface/NonWrappingLayout.yaml");
			facet = new Facet();

			resources.UserInterface.InitializeFacet(facet);

			facet.menu_1.SelectedItemChanged += (sender, args) =>
			{
				var pt = facet.label_1.ClientToScreen(Point.Zero);

				facet.label_1.Text = $"Client Rect: {pt.X},{pt.Y},{facet.label_1.Width},{facet.label_1.Height}";
			};

			Input.Handlers.Add(facet.InterfaceRoot);
		}

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				new SceneStack().Start(this);
			}
		}

		protected override void OnSceneStart()
		{
			CreateGui();
		}
	}
}

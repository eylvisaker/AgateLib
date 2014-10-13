using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsPhone.ApplicationModels;
using AgateLib.Platform.WindowsStore.ApplicationModels;
using AgateLib.Platform.WindowsStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AgateLib.IO;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AgateLib.Testing.WindowsPhone
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class TestContainer : Page
	{
		public TestContainer()
		{
			this.InitializeComponent();
		}

		public TestInfo Info { get; set;}
		AgateAppModel model;

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.
		/// This parameter is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Info = (TestInfo)e.Parameter;

			IAgateTest t = (IAgateTest)Activator.CreateInstance(Info.Class);

			if (t is ISceneModelTest)
				RunTest((ISceneModelTest)t);
			else if (t is ISerialModelTest)
				RunTest((ISerialModelTest)t);
		}
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			AgateAppModel.Instance.Exit();
		}

		private void RunTest(ISerialModelTest t)
		{
			var parameters = new WindowsStoreSerialModelParameters(new SwapChainPanelAdapter(RenderTarget));

			SetAssetPath(t, parameters.AssetLocations);

			t.ModifyModelParameters(parameters);

			var smodel = new SerialModel(parameters);
			model = smodel;
			smodel.Run(new Action(t.EntryPoint));
		}
		private void RunTest(ISceneModelTest t)
		{
			var parameters = new WindowsStoreSceneModelParameters(new SwapChainPanelAdapter(RenderTarget));

			SetAssetPath(t, parameters.AssetLocations);

			t.ModifyModelParameters(parameters);

			var smodel = new SceneModel(parameters);
			model = smodel;
			smodel.Run(t.StartScene);
		}

		private static void SetAssetPath(IAgateTest t, AssetLocations assets)
		{
			var assemblyName = t.GetType().GetTypeInfo().Assembly.GetName().Name;
			assets.Path = assemblyName + "/Assets";
		}

	}
}

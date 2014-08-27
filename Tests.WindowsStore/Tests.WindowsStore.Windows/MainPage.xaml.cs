using AgateLib.Platform.Windows.ApplicationModels;
using AgateLib.Platform.WindowsStoreCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Tests.WindowsStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
	{
		SceneModel model;

        public MainPage()
        {
            this.InitializeComponent();
        }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		public void BeginModel()
		{
			SceneModelParameters parameters = new SceneModelParameters(new SwapChainBackgroundPanelAdapter(this.RenderTarget));

			model = new SceneModel(parameters);

			model.Run(new TestScene());
		}
	}
}

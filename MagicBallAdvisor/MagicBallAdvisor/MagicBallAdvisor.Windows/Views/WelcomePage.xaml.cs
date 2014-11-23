using MagicBallAdvisor.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MagicBallAdvisor.Views
{
    public sealed partial class WelcomePage : UserControl
    {
        public WelcomePage()
        {
            this.InitializeComponent();

            var viewModel = new WelcomePageViewModel();
            this.DataContext = viewModel;
        }

        private void myImage_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var img = sender as Image;
            var delta = e.Delta;
            var scale = delta.Scale;
            img.Width *= scale;
            img.Height *= scale;
        }
    }
}

using MagicBallAdvisor.ViewModels;
using MagicBallAdvisor.Views;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MagicBallAdvisor.Pages
{
    public sealed partial class WelcomePage : Page
    {
        
        public WelcomePage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void LayoutWelcome_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Point velocities = e.Velocities.Linear;
            double swipeHorizontal = velocities.X;
            double swipeVertical = velocities.Y;
            if (swipeHorizontal < 0)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
            }
        }
    }
}

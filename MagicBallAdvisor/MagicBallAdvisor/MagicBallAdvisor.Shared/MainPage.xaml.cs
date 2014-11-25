using MagicBallAdvisor.Pages;
using MagicBallAdvisor.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MagicBallAdvisor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void LayoutAction_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Point velocities = e.Velocities.Linear;
            double swipeHorizontal = velocities.X;
            double swipeVertical = velocities.Y;
            if (swipeHorizontal > 0)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(WelcomePage));
                }
            }
        }
    }
}

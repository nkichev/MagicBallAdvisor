using MagicBallAdvisor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MagicBallAdvisor.Views
{
    public sealed partial class MainPage : UserControl
    {
        private const double accelerationNeededForShake = 0.3;
        private RotateTransform transform = new RotateTransform() { CenterX = 0.5, CenterY = 0.5, Angle = 0 };
        private int numberOfBallSpins = 0;
        private List<string> answers;
        public Accelerometer accelerometer { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            var viewModel = new MainPageViewModel();
            this.DataContext = viewModel;

            this.accelerometer = Accelerometer.GetDefault();
            accelerometer.ReportInterval = 4000;
            accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
        }

        private async void ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Random rnd = new Random();

                if (args.Reading.AccelerationX > accelerationNeededForShake)
                {
                    (this.DataContext as MainPageViewModel).TextResult = "";
                    var timer = new DispatcherTimer();
                    timer.Tick += (snd, timerArgs) =>
                    {
                        if (numberOfBallSpins >= 50)
                        {
                            numberOfBallSpins = 0;
                            answers = (this.DataContext as MainPageViewModel).CategoryChicks.Answers;
                            (this.DataContext as MainPageViewModel).TextResult = answers[rnd.Next(answers.Count)];
                            timer.Stop();
                        }
                        numberOfBallSpins++;
                        transform.Angle += 45;
                        this.imageBall.RenderTransform = transform;
                        
                    };
                    timer.Interval = TimeSpan.FromMilliseconds(30);
                    timer.Start();
                }
            });
        }
    }
}

using GalaSoft.MvvmLight;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using MagicBallAdvisor.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using System.Net.NetworkInformation;

namespace MagicBallAdvisor.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private const string tipForSharingTitle = "MagicBallAdvisor Tip";

        private ICommand commandShare;
        private ICommand commandGetTip;
        private ICommand commandDoubleTapped;

        private MediaElement meSong;
        private string textResult;
        private CategoryViewModel categoryChicks;
        private ObservableCollection<TipViewModel> tips;

        public ICommand DoubleTapped
        {
            get
            {
                if (this.commandDoubleTapped == null)
                {
                    this.commandDoubleTapped = new RelayCommand(this.PerformDoubleTapped);
                }
                return this.commandDoubleTapped;
            }
        }
        public ICommand Share
        {
            get
            {
                if (this.commandShare == null)
                {
                    this.commandShare = new RelayCommand(this.PerformShare);
                }
                return this.commandShare;
            }
        }
        public ICommand GetTip
        {
            get
            {
                if (this.commandGetTip == null)
                {
                    this.commandGetTip = new RelayCommand(this.PerformGetTip);
                }
                return this.commandGetTip;
            }
        }
        public string TextResult 
        {
            get
            {
                return this.textResult;
            }
            set
            {
                this.textResult = value;
                this.RaisePropertyChanged(() => this.TextResult);
            }
        }
        public CategoryViewModel CategoryChicks
        {
            get
            {
                return this.categoryChicks;
            }
            set
            {
                this.categoryChicks = value;
                this.RaisePropertyChanged(() => this.CategoryChicks);
            }
        }
        public IEnumerable<TipViewModel> Tips
        {
            get
            {
                if (this.tips == null)
                {
                    this.tips = new ObservableCollection<TipViewModel>();
                }
                return this.tips;
            }
            set
            {
                if (this.tips == null)
                {
                    this.tips = new ObservableCollection<TipViewModel>();
                }
                this.tips.Clear();
                foreach (var item in value)
                {
                    this.tips.Add(item);
                }
            }
        }
        public MainPageViewModel()
        {
            if (this.CheckNetworkAvailability())
            {
                DataTransferManager dtManager = DataTransferManager.GetForCurrentView();
                dtManager.DataRequested += dtManager_DataRequested;

                this.LoadCategoryChicks();
                this.LoadTips();
            }
            else
            {
                this.ShowMessageBox("You are not connected to the internet", "Internet connection");
            }
            
        }
        public async Task LoadCategoryChicks()
        {
            var categories = await ParseObject.GetQuery("Category")
                .WhereEqualTo("title", "Chicks")
                .FindAsync();

            var categoryChicksAnswers = ((IEnumerable)categories.FirstOrDefault()["answers"]).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToList();
            this.CategoryChicks = new CategoryViewModel();
            this.CategoryChicks.Answers = categoryChicksAnswers;

            int b = 5;
        }
        public async Task LoadTips()
        {
            var tips = await new ParseQuery<Tip>()
                .FindAsync();

            this.Tips = tips.AsQueryable()
                .Select(TipViewModel.FromModel);

            int b = 4;
        }
        private void PerformGetTip()
        {
            Random rnd = new Random();
            tips = this.Tips as ObservableCollection<TipViewModel>;
            if (!(this.tips.Count > 0))
            {
                this.ShowMessageBox("You cannot get a tip yet", "Too early for a tip");
            }
            else
            {
                this.TextResult = tips[rnd.Next(tips.Count)].Text;
            }
        }
        private void PerformDoubleTapped()
        {
            Random rnd = new Random();
            var answers = this.CategoryChicks.Answers;
            if (answers != null)
            {
                this.TextResult = answers[rnd.Next(answers.Count)];
            }
        }
        private void PerformShare()
        {
            DataTransferManager.ShowShareUI();
        }
        private async void dtManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var tipForSharing = this.TextResult;
            e.Request.Data.Properties.Title = tipForSharingTitle;
            e.Request.Data.SetText(tipForSharing);
        }
        private void ShowMessageBox(string msg, string title)
        {
            MessageDialog msgDialog = new MessageDialog(msg, title);

            UICommand okBtn = new UICommand("Ok");
            msgDialog.Commands.Add(okBtn);

            msgDialog.ShowAsync();
        }

        private bool CheckNetworkAvailability()
        {
            var isConnected = NetworkInterface.GetIsNetworkAvailable();

            return isConnected;
        }
    }
}

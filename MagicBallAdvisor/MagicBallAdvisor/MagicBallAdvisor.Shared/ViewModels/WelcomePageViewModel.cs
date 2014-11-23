using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MagicBallAdvisor.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MagicBallAdvisor.ViewModels
{
    public class WelcomePageViewModel : ViewModelBase
    {
        private const string dbName = "Users.db";
        private static readonly IEnumerable<string> SupportedImageFileTypes = new List<string> { ".jpeg", ".jpg", ".png" };
        private ICommand commandFileOpen;
        private ICommand commandSaveUserName;
        private WriteableBitmap originalBitmap;
        private string userName;

        public string UserName
        { 
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
                this.RaisePropertyChanged(() => this.UserName);
            }
        }

        public ICommand FileOpen
        {
            get
            {
                if (this.commandFileOpen == null)
                {
                    this.commandFileOpen = new RelayCommand(this.PerformFileOpen);
                }
                return this.commandFileOpen;
            }
        }

        public ICommand SaveUserName
        {
            get
            {
                if (this.commandSaveUserName == null)
                {
                    this.commandSaveUserName = new RelayCommand<object>((name) => this.PerformSaveUserName(name));
                }
                return this.commandSaveUserName;
            }
        }

        public WriteableBitmap OriginalBitmap
        {
            get
            {
                return this.originalBitmap;
            }
            set
            {
                this.originalBitmap = value;
                this.RaisePropertyChanged(() => this.OriginalBitmap);
            }
        }

        public WelcomePageViewModel()
        {
            var app = Application.Current as App;
            if (app != null)
            {
                app.FilesOpenPicked += OnFilesOpenPicked;
            }

            this.GetLastPhoto();
            this.InstantiateDB();
        }

        private async void PerformFileOpen()
        {
            // Pick photo or take new one
            var fop = new FileOpenPicker();
            foreach (var fileType in SupportedImageFileTypes)
            {
                fop.FileTypeFilter.Add(fileType);
            }
#if WINDOWS_PHONE_APP
            fop.PickSingleFileAndContinue();
#elif WINDOWS_APP
            StorageFile file = await fop.PickSingleFileAsync();
            GetPicture(file);
#endif
        }

        private void PerformSaveUserName(object name)
        {
            if ((name.ToString().Length < 1))
            {
                ShowMessageBox("The name must be longer than 0", "Invalid name");
            }
            else
            {
                this.UpdateUserNameAsync(name.ToString());
            }
        }

        private void ShowMessageBox(string msg, string title)
        {
            MessageDialog msgDialog = new MessageDialog(msg, title);

            UICommand okBtn = new UICommand("Ok");
            msgDialog.Commands.Add(okBtn);

            msgDialog.ShowAsync();
        }

        #region Pick Photo

        private async void OnFilesOpenPicked(IReadOnlyList<StorageFile> files)
        {
            if (files.Last() != null)
            {
                // Load picked file
                if (files.Count > 0)
                {
                    // Check if image and pick first file to show
                    var imageFile = files.FirstOrDefault(f => SupportedImageFileTypes.Contains(f.FileType.ToLower()));
                    if (imageFile != null)
                    {
                        // Use WriteableBitmapEx to easily load from a stream
                        using (var stream = await imageFile.OpenReadAsync())
                        {
                            this.OriginalBitmap = await new WriteableBitmap(1, 1).FromStream(stream);
                        }
                    }
                }
            }
        }

        private async void GetPicture(StorageFile file)
        {
            var imageFile = file;
            if (imageFile != null)
            {
                // Use WriteableBitmapEx to easily load from a stream
                using (var stream = await imageFile.OpenReadAsync())
                {
                    this.OriginalBitmap = await new WriteableBitmap(1, 1).FromStream(stream);
                }
                StorageApplicationPermissions.FutureAccessList.Add(file);
            }
        }

        private async void GetLastPhoto()
        {
            var filesCamera = await KnownFolders.CameraRoll.GetFilesAsync();
            if (filesCamera != null)
            {
                OnFilesOpenPicked(new ReadOnlyCollection<StorageFile>(new List<StorageFile> { filesCamera.FirstOrDefault() }));
                return;
            }
        }

        #endregion

        private async void InstantiateDB()
        {
            // Create Db if not exist
            bool dbExists = await CheckDbAsync(dbName);
            if (!dbExists)
            {
                await CreateDatabaseAsync();
                await AddUserAsync();
            }

            // Get User
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            var query = conn.Table<User>();
            var user = await query.FirstOrDefaultAsync();

            // Show users
            this.UserName = user.Name;
        }

        #region SQLite utils
        private async Task<bool> CheckDbAsync(string dbName)
        {
            bool dbExist = true;

            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(dbName);
            }
            catch (Exception)
            {
                dbExist = false;
            }

            return dbExist;
        }

        private async Task CreateDatabaseAsync()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            await conn.CreateTableAsync<User>();
        }

        private async Task AddUserAsync()
        {
            // Create a Articles list
            var user = new User()
            {
                Name = ""
            };

            //this.UserName = "Niki";
            // Add rows to the Article Table
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            await conn.InsertAsync(user);
        }

        private async Task UpdateUserNameAsync(string newName)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);

            // Retrieve User
            var user = await conn.Table<User>().FirstOrDefaultAsync();
            if (user != null)
            {
                // Modify User
                user.Name = newName;
                this.UserName = newName;

                // Update record
                await conn.UpdateAsync(user);

                string title = "Welcome, " + newName;
                string msg = "Start ballin' and catch some chicks :)";
                this.ShowMessageBox(msg, title);
            }
        }

        #endregion SQLite utils
    }
}

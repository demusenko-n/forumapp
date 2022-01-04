using MIST_infrastructure;
using MIST_infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;
using MIST_app.Services;
using MIST_app.Views;
using System.Threading.Tasks;

namespace MIST_app.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public Command ChangeNameCommand { get; }
        public Command ChangeLoginCommand { get; }
        public Command ChangePasswordCommand { get; }
        public Command OnAppearCommand { get; }

        string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        string login = string.Empty;
        public string Login
        {
            get { return login; }
            set { SetProperty(ref login, value); }
        }
        string password = string.Empty;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public ProfileViewModel()
        {
            Title = "Edit your profile";
            ChangeNameCommand = new Command(OnChangeName);
            ChangeLoginCommand = new Command(OnChangeLogin);
            ChangePasswordCommand = new Command(OnChangePassword);
            OnAppearCommand = new Command(OnAppear);
        }

        private async void OnAppear(object _)
        {
            await Task.Run(() =>
            {
                Login = ForumDataStore.Instance.CurrentUser.Login;
                Name = ForumDataStore.Instance.CurrentUser.Name;
            });
        }

        private async void OnChangeName(object _)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await TryUpdate(new User()
            {
                Id = ForumDataStore.Instance.CurrentUser.Id,
                Name = Name,
                Login = ForumDataStore.Instance.CurrentUser.Login,
                Password = ForumDataStore.Instance.CurrentUser.Password
            });
            IsBusy = false;
        }

        private async void OnChangeLogin(object _)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await TryUpdate(new User()
            {
                Id = ForumDataStore.Instance.CurrentUser.Id,
                Name = ForumDataStore.Instance.CurrentUser.Name,
                Login = Login,
                Password = ForumDataStore.Instance.CurrentUser.Password
            });
            IsBusy = false;
        }

        private async void OnChangePassword(object _)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await TryUpdate(new User()
            {
                Id = ForumDataStore.Instance.CurrentUser.Id,
                Name = ForumDataStore.Instance.CurrentUser.Name,
                Login = ForumDataStore.Instance.CurrentUser.Login,
                Password = Password
            });
            IsBusy = false;
        }

        private async Task TryUpdate(User user)
        {
            var resultsCheck = new List<ValidationResult>();
            if (!Validator.TryValidateObject(user, new ValidationContext(user), resultsCheck, true))
            {
                await Application.Current.MainPage.DisplayAlert("Notification", $"Error: {string.Join("\n", resultsCheck.Select(r => r.ErrorMessage))}", "ОK");
                return;
            }


            try
            {
                TransferObj<User> res = await HttpService.Instance.Post<User>("/api/users/update/", new StringContent(
                JsonService.Instance.Serialize(user),
                Encoding.UTF8, "application/json"));

                if (res.IsSucceed)
                {
                    ForumDataStore.Instance.CurrentUser = res.Result;
                    await Application.Current.MainPage.DisplayAlert("Success", "Successfully changed user data", "ОK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"{res.FailMessage}", "ОK");
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to establish a connection to the server", "ОK");
            }

        }


        private async void OnGoToLoginClicked(object _)
        {

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}

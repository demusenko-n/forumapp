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

namespace MIST_app.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public Command GoToLoginCommand { get; }
        public Command AttemptRegisterCommand { get; }
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
        string passwordRepeat = string.Empty;
        public string PasswordRepeat
        {
            get { return passwordRepeat; }
            set { SetProperty(ref passwordRepeat, value); }
        }

        public RegisterViewModel()
        {
            GoToLoginCommand = new Command(OnGoToLoginClicked);
            AttemptRegisterCommand = new Command(OnAttemptRegisterClicked);
        }

        private async void OnAttemptRegisterClicked(object _)
        {
            if (IsBusy)
                return;

            if (Password != PasswordRepeat)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", "Passwords don't match", "ОK");
                return;
            }
            User user = new User() { Name = Name, Login = Login, Password = Password };

            var resultsCheck = new List<ValidationResult>();
            if (!Validator.TryValidateObject(user, new ValidationContext(user), resultsCheck, true))
            {
                await Application.Current.MainPage.DisplayAlert("Notification", $"Error: {string.Join("\n", resultsCheck.Select(r => r.ErrorMessage))}", "ОK");
                return;
            }

            IsBusy = true;
            try
            {
                TransferObj<User> res = await HttpService.Instance.Post<User>("/api/users/reg/", new StringContent(
                JsonService.Instance.Serialize(user),
                Encoding.UTF8, "application/json"));

                if (res.IsSucceed)
                {
                    ForumDataStore.Instance.CurrentUser = res.Result;
                    await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
                    Login = Password = PasswordRepeat = Name = "";
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
            IsBusy = false;

            
        }

        private async void OnGoToLoginClicked(object _)
        {

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}

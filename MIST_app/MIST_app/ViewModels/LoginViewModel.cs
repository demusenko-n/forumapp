using MIST_infrastructure;
using MIST_infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using MIST_app.Services;
using MIST_app.Views;

namespace MIST_app.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command AttemptLoginCommand { get; }
        public Command GoToRegisterCommand { get; }

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
        public LoginViewModel()
        {
            AttemptLoginCommand = new Command(OnAttemptLoginClicked);
            GoToRegisterCommand = new Command(OnGoToRegisterClicked);
        }

        private async void OnGoToRegisterClicked(object _)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegistrationPage)}");
        }

        private async void OnAttemptLoginClicked(object _)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                TransferObj<User> res = await HttpService.Instance.Post<User>($"/api/users/auth?login={Login}&password={Password}", null);
                if (res.IsSucceed)
                {
                    ForumDataStore.Instance.CurrentUser = res.Result;
                    await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");

                    Login = Password = "";
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
    }
}

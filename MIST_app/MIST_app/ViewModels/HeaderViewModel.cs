using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using MIST_app.Services;

namespace MIST_app.ViewModels
{
    public class HeaderViewModel : BaseViewModel
    {
        public string Name
        {
            get => ForumDataStore.Instance.CurrentUser?.Name??"";
        }
        public string Login
        {
            get => ForumDataStore.Instance.CurrentUser?.Login??"";
        }
        public void UpdateView()
        {
            OnPropertyChanged("Name");
            OnPropertyChanged("Login");
        }
    }
}

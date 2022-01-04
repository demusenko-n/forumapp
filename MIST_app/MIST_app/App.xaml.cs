using MIST_app.Services;
using MIST_app.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MIST_app
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

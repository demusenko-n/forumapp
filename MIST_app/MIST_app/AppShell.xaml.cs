using MIST_app.Services;
using MIST_app.ViewModels;
using MIST_app.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MIST_app
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            PropertyChanged += (o, e) =>
            {
                if (e.PropertyName.Equals("FlyoutIsPresented"))
                {
                    if (Views.FlyoutHeader.ViewModel != null)
                        Views.FlyoutHeader.ViewModel.UpdateView();
                }
            };
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync("//LoginPage");
            ForumDataStore.Instance.CurrentUser = null;
        }
    }
}

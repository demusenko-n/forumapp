using MIST_app.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MIST_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        PostViewModel _viewModel;
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PostViewModel();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
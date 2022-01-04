using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MIST_app.ViewModels;

namespace MIST_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutHeader : ContentView
    {
        public static HeaderViewModel ViewModel { get; private set; }
        public FlyoutHeader()
        {
            InitializeComponent();
            ViewModel = new HeaderViewModel();
            BindingContext = ViewModel;
        }
    }
}
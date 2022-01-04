using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Test
{
    public partial class MainPage : ContentPage
    { 
        public ICommand Open => new Command<string>(async (url) => await Launcher.OpenAsync(url));
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}

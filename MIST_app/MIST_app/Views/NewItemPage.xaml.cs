using MIST_infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MIST_app.ViewModels;

namespace MIST_app.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Post Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}
using MIST_infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using MIST_app.Services;

namespace MIST_app.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        public ForumDataStore DataStore => ForumDataStore.Instance;
        private string body;
        private string subject;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(body);
               // && !String.IsNullOrWhiteSpace(description);
        }

        public string Body
        {
            get => body;
            set => SetProperty(ref body, value);
        }

        public string Subject
        {
            get => subject;
            set => SetProperty(ref subject, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Post newItem = new Post()
            {
                Body = Body,
                AuthorId = DataStore.CurrentUser.Id,
                Subject = Subject
            };

            await DataStore.AddPostAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}

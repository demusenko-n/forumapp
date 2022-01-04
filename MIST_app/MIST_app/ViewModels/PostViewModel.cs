using MIST_infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using MIST_app.Services;

namespace MIST_app.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class PostViewModel : BaseViewModel
    {
        Post post;
        public ForumDataStore DataStore => ForumDataStore.Instance;
        public ObservableCollection<Comment> Comments { get; }
        public Command LoadCommentsCommand { get; }
        public Command OnSaveCommand { get; }

        private string newCommentText;
        private int itemId;
        private string body;
        private string subject;
        private string author;
        public PostViewModel()
        {
            Title = "View Post";
            Comments = new ObservableCollection<Comment>();
            LoadCommentsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnSaveCommand = new Command(async () => await OnAddComment());
        }

        public string NewCommentText { get => newCommentText; set => SetProperty(ref newCommentText, value); }

        public Post Post
        {
            get => post;
            set => SetProperty(ref post, value);
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

        public string Author
        {
            get => author;
            set => SetProperty(ref author, value);
        }

        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async Task OnAddComment()
        {
            Comment newComment = new Comment()
            {
                CommentAuthorId = DataStore.CurrentUser.Id,
                Date = DateTime.UtcNow,
                PostId = itemId,
                Text = NewCommentText
            };

            if (await DataStore.AddCommentAsync(newComment))
            {
                NewCommentText = "";
            }
            IsBusy = true;
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await DataStore.GetPostAsync(itemId);
                Body = item.Body;
                Subject = item.Subject;
                Author = item.Author.Name;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Post = await DataStore.GetPostAsync(ItemId);
                Body = Post.Body;
                Subject = Post.Subject;
                Author = Post.Author.Name;
                Comments.Clear();
                var items = Post.Comments;
                foreach (var item in items.OrderBy(c=>c.Date))
                {
                    Comments.Add(item);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

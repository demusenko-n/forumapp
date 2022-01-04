using MIST_infrastructure;
using MIST_infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MIST_app.Services
{
    public class ForumDataStore
    {
        public static ForumDataStore Instance { get; } = new ForumDataStore();

        public User CurrentUser { get; set; }
        public List<Post> Posts { get; set; }


        public async Task<bool> AddPostAsync(Post post)
        {
            await UpdateStore();

            var resultsCheck = new List<ValidationResult>();
            if (!Validator.TryValidateObject(post, new ValidationContext(post), resultsCheck, true))
            {
                await Application.Current.MainPage.DisplayAlert("Notification", $"Error: {string.Join("\n", resultsCheck.Select(r => r.ErrorMessage))}", "ОK");
                return false;
            }


            try
            {
                TransferObj<Post> res = await HttpService.Instance.Post<Post>("/api/posts/add/", new StringContent(
                JsonService.Instance.Serialize(post),
                Encoding.UTF8, "application/json"));

                if (res.IsSucceed)
                {
                    post = res.Result;
                    Posts.Add(post);
                    return true;
                }

                await Application.Current.MainPage.DisplayAlert("Error", $"{res.FailMessage}", "ОK");
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to establish a connection to the server", "ОK");
            }
            return false;

        }
        public async Task<bool> AddCommentAsync(Comment comment)
        {

            await UpdateStore();

            var resultsCheck = new List<ValidationResult>();
            if (!Validator.TryValidateObject(comment, new ValidationContext(comment), resultsCheck, true))
            {
                await Application.Current.MainPage.DisplayAlert("Notification", $"Error: {string.Join("\n", resultsCheck.Select(r => r.ErrorMessage))}", "ОK");
                return false;
            }


            try
            {
                TransferObj<Comment> res = await HttpService.Instance.Post<Comment>("/api/comments/add/", new StringContent(
                JsonService.Instance.Serialize(comment),
                Encoding.UTF8, "application/json"));

                if (res.IsSucceed)
                {
                    comment = res.Result;
                    comment.CommentAuthor = CurrentUser;
                    Posts.Find(p=>p.Id == comment.PostId).Comments.Add(comment);
                    return true;
                }

                await Application.Current.MainPage.DisplayAlert("Error", $"{res.FailMessage}", "ОK");
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to establish a connection to the server", "ОK");
            }
            return false;
        }
        public async Task<Post> GetPostAsync(int id)
        {
            await UpdateStore();
            return await Task.FromResult(Posts.Find(post => post.Id == id));
        }
        public async Task<List<Post>> GetPostsAsync()
        {
            await UpdateStore();
            return Posts;
        }
        public async Task UpdateStore()
        {
            if (CurrentUser == null)
                return;

            try
            {
                Posts = await HttpService.Instance.Get<List<Post>>($"/api/posts/");
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to establish a connection to the server", "ОK");
            }
        }
    }
}

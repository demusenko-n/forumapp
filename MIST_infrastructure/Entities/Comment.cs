using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIST_infrastructure.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(1000, MinimumLength = 1)]
        public string Text { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int PostId { get; set; }

        public int? CommentAuthorId { get; set; }

        public Comment(string text, int postId)
        {
            Text = text;
            PostId = postId;
        }
        public Comment()
        {

        }
        //navigation
        public Post Post { get; set; }
        public User CommentAuthor { get; set; }
    }
}

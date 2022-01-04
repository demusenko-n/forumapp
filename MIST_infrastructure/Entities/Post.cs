using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIST_infrastructure.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Subject { get; set; }

        [Required, MaxLength(1000)]
        public string Body { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int? AuthorId { get; set; }
        public User Author { get; set; }

        public Post()
        {

        }
        //navigation
        public List<Comment> Comments { get; set; }
        public Post(string subject, string body)
        {
            Subject = subject;
            Body = body;
            Date = DateTime.UtcNow;
        }
    }
}

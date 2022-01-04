using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIST_infrastructure.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(30, MinimumLength = 5)]
        public string Name { get; set; }

        [Required, StringLength(30, MinimumLength = 5)]
        public string Login { get; set; }

        [Required, StringLength(30, MinimumLength = 6)]
        public string Password { get; set; }
        public User()
        {

        }

        public List<Comment> Comments { get; set; }
        public List<Post> Posts { get; set; }

        public override string ToString()
        {
            return $"[{Id} {Name} {Login}]";
        }

    }
}

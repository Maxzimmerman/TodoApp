using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Models
{
    public class UserProject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        [DefaultValue(false)]
        public bool IsLiked { get; set; }

        public UserProject() { }

        public UserProject(string title, string applicationUserId, ApplicationUser user, bool isDeleted, bool isLiked)
        {
            Title = title;
            ApplicationUserId = applicationUserId;
            User = user;
            IsDeleted = isDeleted;
            IsLiked = isLiked;
        }
    }
}

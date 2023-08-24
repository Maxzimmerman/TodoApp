using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Models
{
    public class TodoEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [DefaultValue(false)]
        public bool IChecked { get; set; }
        [DefaultValue(false)]
        public bool IDeleted { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("Priority")]
        public int PriorityId { get; set; }
        public Priority Priority { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
    }
}

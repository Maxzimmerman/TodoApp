using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Models
{
    public class Priority
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(25)]
        public string Color { get; set; }

        public Priority() { }

        public Priority(string name, string color)
        {
            Name = name;
            Color = color;
        }
    }
}

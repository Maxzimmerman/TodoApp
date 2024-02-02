using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.ModelsIn
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        [Range(0, 365)]
        public int Counter { get; set; }

        public Category() { }

        public Category(string name, int counter)
        {
            Name = name;
            Counter = counter;
        }
    }
}

namespace Todo.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // when the user uses a weekly then the counter will be 7
        // i'll have to make sure that this todo is after 7 days available
        public int Counter { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Todo.Models
{
    public class TodoItem
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
        public bool IsComplete { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}

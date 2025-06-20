using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace To_Do_List.Models
{
    public class Task
    {
        [Key]
        public int Id { set; get; }
        public string Name { set; get; } = string.Empty;
        public string Description { set; get; } = string.Empty;
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
        public bool IsCompleted { get; set; } = false;

        // foreign key to the user who created the task.
        public string UserId { get; set; } = string.Empty; // Assuming you have a UserId to associate with the task
        
        
        // Navigation properties.
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = new();
    }
}

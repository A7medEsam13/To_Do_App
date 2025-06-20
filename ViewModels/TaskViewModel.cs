using System.ComponentModel.DataAnnotations;
using To_Do_List.ViewModel;

namespace To_Do_List.ModelView
{
    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
    public class TaskViewModel
    {
        public int? Id { set; get; }
        public string? UserId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { set; get; } = string.Empty;
        [Required(ErrorMessage = "Description is required.")]
        public string Description { set; get; } = string.Empty;
        [Required(ErrorMessage = "Start date is required.")]
        public DateOnly Start { get; set; }
        [Required(ErrorMessage = "End date is required.")]
        public DateOnly End { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        public bool IsCompleted { get; set; } = false;

        // Navigation properties.
        public virtual ApplicationUser? User { get; set; }
    }
}

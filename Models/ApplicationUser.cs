using Microsoft.AspNetCore.Identity;

namespace To_Do_List.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        // Navigation properties.
        public virtual ICollection<Task> Tasks { get; set; }
        
    }
    
    
}
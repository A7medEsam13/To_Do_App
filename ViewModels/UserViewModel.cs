using Microsoft.AspNetCore.Identity;

namespace To_Do_List.ViewModel
{
    public class UserViewModel : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        // Navigation properties.
        public virtual ICollection<Models.Task>? Tasks { get; set; }
    }
}
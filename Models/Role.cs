namespace To_Do_List.Models
{
    public class Role : IdentityRole
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public string Description { get; set; } = string.Empty;
    }
}

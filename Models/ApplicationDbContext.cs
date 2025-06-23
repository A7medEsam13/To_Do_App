using Microsoft.EntityFrameworkCore;
using To_Do_List.ModelView;


namespace To_Do_List.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>   
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

       

        public DbSet<Models.Task> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<To_Do_List.ModelView.TaskViewModel> TaskViewModel { get; set; } = default!;
    }
}

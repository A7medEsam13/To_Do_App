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
            //modelBuilder.Entity<Task>()
            //    .HasOne(Task => Task.User)
            //    .WithMany(user => user.Tasks)
            //    .HasForeignKey(Task => Task.UserId);

        }
    }
}

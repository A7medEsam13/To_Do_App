


namespace To_Do_List.Services
{
    public class TaskService : ITaskService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        

        public async System.Threading.Tasks.Task Create(Models.Task task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public System.Threading.Tasks.Task<bool> Delete(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return System.Threading.Tasks.Task.FromResult(false);
            }
            _context.Tasks.Remove(task);
            return System.Threading.Tasks.Task.FromResult(true);
        }

        public async System.Threading.Tasks.Task<Models.Task> GetTaskById(int id)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async System.Threading.Tasks.Task<IEnumerable<Models.Task>> GetTasks()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User == null)
            {
                throw new UnauthorizedAccessException("HttpContext or User is null.");
            }

            var user = await _userManager.GetUserAsync(httpContext.User);

            try
            {
                var tasks = await _context.Tasks
                    .Where(t => t.UserId == user.Id)
                    .ToListAsync();
                return tasks;

            }
            catch
            {
                return new List<Models.Task>();

            }
        }
        public async System.Threading.Tasks.Task MarkAsCompleted(int id)
        {
            var task = await GetTaskById(id);
            task.IsCompleted = true;
        }

        public async System.Threading.Tasks.Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Models.Task task)
        {
            _context.Tasks.Update(task);
        }
    }
}

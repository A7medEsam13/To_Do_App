
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using To_Do_List.ModelView;


namespace To_Do_List.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(ITaskService taskService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: TaskController
        public async Task<ActionResult> Index()
        
        
        {
            var tasks = await _taskService.GetTasks();
            var taskViewModels = _mapper.Map<IEnumerable<TaskViewModel>>(tasks);
            return View(taskViewModels);
        }

        // GET: TaskController/Details/5
        public ActionResult Details(int id)
        { 
            return View();
        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TaskViewModel model)
        {
            // if model is not valid.
            if(!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }

            // if model is valid
            // map it to the task model.
            var task = _mapper.Map<Models.Task>(model);

            // set the user id from the current user.
            var user = _userManager.GetUserAsync(User);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User is not authenticated.");
                return View(model);
            }
            task.User = user.Result;
            task.UserId = user.Result.Id;

            // adding the model to task table
            await _taskService.Create(task);
            await _taskService.Save();
            return RedirectToAction(nameof(Index));
        }

        // GET: TaskController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToogleCompleted(int id)
        {
            var task = await _taskService.GetTaskById(id);
            if (task == null) 
            { 
                return NotFound(); 
            }
            task.IsCompleted = !task.IsCompleted;
            await _taskService.Save();

            return Ok(new { taskId = task.Id, isCompleted = task.IsCompleted });
        }
    }
}


using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // get the current user from the http context.
        private async Task<ApplicationUser> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            return user;
        }

        // GET: TaskController
        public async Task<ActionResult> Index()
        {
            var tasks = await _taskService.GetTasks();
            var taskViewModels = _mapper.Map<IEnumerable<TaskViewModel>>(tasks);
            foreach(var task in taskViewModels)
            {
                task.Status = Status(task);
            }
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
            var user = GetCurrentUser();

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
        public async Task<ActionResult> Edit(int id )
        {
            var task = await _taskService.GetTaskById(id);

            var taskVM = _mapper.Map<TaskViewModel>(task);
            ViewBag.StatusOptions = Enum.GetValues(typeof(ModelView.TaskStatus))
                .Cast<ModelView.TaskStatus>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = s.ToString()
                }).ToList();
            return View(taskVM);
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, TaskViewModel model)
        {
            // checks if model is valid
            if(!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Edit));
            }
            // assigns task to completed if status is completed.
            if (model.Status == ModelView.TaskStatus.Completed)
                model.IsCompleted = true;

            // get the current user.
            var user = await GetCurrentUser();

            // mapping the view model to the task model.
            var task = _mapper.Map<Models.Task>(model);
            // set the user.
            task.User = user;
            task.UserId = user.Id;
            //updating the task
            _taskService.Update(task);
            // saving changes
            await _taskService.Save();

            return RedirectToAction(nameof(Index));
        }

        // GET: TaskController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            // get the task from database
            var task = await _taskService.GetTaskById(id);
            // mapping the task model to view model.
            var taskVM = _mapper.Map<TaskViewModel>(task);
            return View(taskVM);
        }

        // POST: TaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, TaskViewModel model)
        {
            if (await _taskService.Delete(id))
            {
                await _taskService.Save();
                return RedirectToAction(nameof(Index));            
            }
            ModelState.AddModelError(string.Empty, "Error deleting task.");
            return View(model);
        }

        [HttpPost]
        [Route("task/togglecompleted/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleCompleted(int id)
        {
            var task = await _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            task.IsCompleted = !task.IsCompleted;
            await _taskService.Save();

            return Json(new
            {
                success = true,
                IsCompleted = task.IsCompleted,
                Id = task.Id
            });
        }

        // date range validation.
        private bool IsValidDateRange(DateOnly start, DateOnly end)
        {
            // Check if the start date is before the end date.
            return start <= end;
        }


        // checks if task range is ended 
        private bool IsTaskEnded(Models.Task task)
        {
            return task.End < DateOnly.FromDateTime(DateTime.Today);
        }


        // set task status depends on completion and range of date.
        private ModelView.TaskStatus Status(ModelView.TaskViewModel model)
        {
            var task = _mapper.Map<Models.Task>(model);
            if (task.IsCompleted)
                return ModelView.TaskStatus.Completed;
            else if (!task.IsCompleted && !IsTaskEnded(task))
                return ModelView.TaskStatus.Pending;
            else 
                return ModelView.TaskStatus.Cancelled;
        }

    }
}

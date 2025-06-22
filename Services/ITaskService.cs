namespace To_Do_List.Services
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<IEnumerable<Models.Task>> GetTasks();
        System.Threading.Tasks.Task<Models.Task> GetTaskById(int id);
        System.Threading.Tasks.Task Create(Models.Task task);
        void Update(Models.Task task);
        System.Threading.Tasks.Task<bool> Delete(int id);
        System.Threading.Tasks.Task MarkAsCompleted(int id);
        System.Threading.Tasks.Task Save();
    }
}

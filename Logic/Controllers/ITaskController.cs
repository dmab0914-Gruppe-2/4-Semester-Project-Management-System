using Logic.Models;

namespace Logic.Controllers
{
    public interface ITaskController
    {
        int CreateTask(string name, string description, User leaderUser);
        int CreateTask(string name, string description);
        Task[] GetTask(string name);
        Task GetTask(int id);
    }
}
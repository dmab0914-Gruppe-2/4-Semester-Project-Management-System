using Logic.Models;

namespace Logic.Controllers
{
    public interface ITaskController
    {
        ReturnValue CreateTask(string title, string description, Priority priority, User leaderUser);
        ReturnValue CreateTask(string title, string description, Priority priority);
        ReturnValue CreateTask(string title, Priority priority);
        ReturnValue CreateTask(string title);
        Task[] GetTask(string title);
        Task GetTask(int id);
        ReturnValue RemoveTask(int id);
    }
}
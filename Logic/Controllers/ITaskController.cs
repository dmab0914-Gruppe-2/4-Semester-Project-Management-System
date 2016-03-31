using Logic.Models;

namespace Logic.Controllers
{
    public interface ITaskController
    {
        int CreateTask(string title, string description, Priority priority, User leaderUser);
        int CreateTask(string title, string description, Priority priority);
        int CreateTask(string title, Priority priority);
        int CreateTask(string title);
        Task[] GetTask(string title);
        Task GetTask(int id);
        int RemoveTask(int id);
        bool CheckInjection(string input);
        string CorrectInjection(string input);
    }
}
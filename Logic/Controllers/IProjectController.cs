using Logic.Models;

namespace Logic.Controllers
{
    public interface IProjectController
    {
        int CreateProject(string name, string description, User leaderUser);
        int CreateProject(string name, string description);
        int CreateProject(string name);
        Project[] GetProject(string name);
        Project GetProject(int id);
        int AddTaskToProject(int taskId, int projectId);
        int RemoveTaskFromProject(int taskId, int projectId);
    }
}
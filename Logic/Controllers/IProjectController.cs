using Logic.Models;

namespace Logic.Controllers
{
    public interface IProjectController
    {
        ReturnValue CreateProject(string name, string description, User leaderUser);
        ReturnValue CreateProject(string name, string description);
        ReturnValue CreateProject(string name);
        ReturnValue RemoveProject(int id);
        Project[] GetProject(string name);
        Project GetProject(int id);
        Project[] GetAllProjects();
        ReturnValue AddTaskToProject(int taskId, int projectId);
        ReturnValue RemoveTaskFromProject(int taskId, int projectId);
        Models.Task[] GetTasksFromProject(int projectId);
    }
}
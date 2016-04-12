using Logic.Models;

namespace Logic.Controllers
{
    public interface IProjectController
    {
        ReturnValue CreateProject(string title, string description, User leaderUser);
        ReturnValue CreateProject(string title, string description);
        ReturnValue CreateProject(string title);
        ReturnValue RemoveProject(int id);
        ReturnValue EditProject(Project project);
        Project[] GetProject(string title);
        Project GetProject(int id);
        Project[] GetAllProjects();
        ReturnValue AddTaskToProject(int taskId, int projectId);
        ReturnValue RemoveTaskFromProject(int taskId, int projectId);
        Models.Task[] GetTasksFromProject(int projectId);
    }
}
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
        Models.Task[] GetTasksFromProject(int projectId);
    }
}
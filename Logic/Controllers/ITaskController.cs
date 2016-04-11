﻿using Logic.Models;

namespace Logic.Controllers
{
    public interface ITaskController
    {
        ReturnValue CreateTask(string title, string description, Priority priority, User leaderUser, int projectId);
        Task[] GetTask(string title);
        Task GetTask(int id);
        ReturnValue RemoveTask(int id);
    }
}
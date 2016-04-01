﻿namespace Logic
{
    public enum TaskStatus
    {
        //Done
        Done = 0, 
        //Assigned but not started
        Assigned = 1, 
        //Unassigned
        Unassigned = 2,
        //In progress
        InProgress = 2
    }

    public enum Priority
    {
        Unassigned = 0,
        Low = 1,
        Normal = 2,
        High = 3,
        Urgent = 4
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_UI.Models.Enums;

namespace Web_UI.Models.Enums
{
    public enum Status
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

public static class TheirEnumExtensions
{
    public static Priority ToWebEnumPriority(Logic.Priority value)
    {
        Priority result = (Priority)value;
        return result;
    }
    public static Status ToWebEnumTaskStatus(Logic.TaskStatus value)
    {
        Status result = (Status)value;
        return result;
    }
}

public static class MyEnumExtensions
{
    public static Logic.Priority ToLogicEnumPriority(Priority value)
    {
        Logic.Priority result = (Logic.Priority)value;
        return result;
    }
    public static Logic.TaskStatus ToLogicEnumStatus(Status value)
    {
        Logic.TaskStatus result = (Logic.TaskStatus)value;
        return result;
    }
}
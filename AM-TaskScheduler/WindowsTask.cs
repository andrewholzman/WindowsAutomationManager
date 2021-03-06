﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AM_TaskScheduler
{
    public class WindowsTask
    {
        public WindowsTask(object tasks)
        {
            PropertyInfo[] props = tasks.GetType().GetProperties();
            Id = (Guid)props[0].GetValue(tasks);
            Name = (string)props[1].GetValue(tasks);
            Description = (string)props[2].GetValue(tasks);
            TriggerType = (string)props[3].GetValue(tasks);
            TriggerString = (string)props[4].GetValue(tasks);
            TriggerAction = (string)props[5].GetValue(tasks);
            ActionFilePath = (string)props[7].GetValue(tasks);
            DateCreated = (DateTime)props[8].GetValue(tasks);
            LastRun = (DateTime)props[9].GetValue(tasks);
            CreatedByUser = (string)props[10].GetValue(tasks);
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TriggerType { get; set; }
        public string TriggerString { get; set; }
        public string TriggerAction { get; set; }
        public string ActionFilePath { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastRun { get; set; }
        public string CreatedByUser { get; set; }
    }
}

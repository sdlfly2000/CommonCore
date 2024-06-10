using System;

namespace Common.Core.AOP.Log
{
    public class LogTraceAttribute : Attribute
    {
        public LogTraceAttribute() : this(string.Empty)
        {
        }

        public LogTraceAttribute(string activityName) 
        {
            ActivityName = activityName;
        }

        public string ActivityName { get; set; }
    }
}

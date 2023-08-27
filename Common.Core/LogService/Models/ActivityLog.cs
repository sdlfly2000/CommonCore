namespace Common.Core.LogService.Models
{
    public class ActivityLog : IActivityLog
    {
        public string ActivityName { get; set; }
        public string ActivityVector { get; set; }
        public string TraceId { get; set; }
        public int Passed { get; set; }
        public string Exception { get; set; }
    }    
}

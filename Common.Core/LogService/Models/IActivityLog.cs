namespace Common.Core.LogService.Models
{
    public interface IActivityLog
    {
        string ActivityName { get; set; }
        string ActivityVector { get; set; }
        string TraceId { get; set; }
        int Passed { get; set; }
        string Exception { get; set; }
    }
}

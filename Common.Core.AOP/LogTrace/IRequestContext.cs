using System;
using System.Collections.Generic;

namespace Common.Core.AOP.LogTrace;

public interface IRequestContext
{
    public string TraceId { get; set; }
    public Guid CurrentUserId { get; set; }
    public List<string> CurrentUserRoles { get; set; }
}

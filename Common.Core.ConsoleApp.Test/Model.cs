using Common.Core.AOP.Cache;

namespace Common.Core.ConsoleApp.Test;

public class Model
{
    [CacheKey]
    public string Name { get; set; }
    public int Age { get; set; }
}

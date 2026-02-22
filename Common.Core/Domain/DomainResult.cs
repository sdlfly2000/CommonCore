namespace Common.Core.Domain;

public class DomainResult<T> where T : class
{
    public T Id { get; set; }

    public string Message { get; set; }

    public bool Success { get; set; }
}

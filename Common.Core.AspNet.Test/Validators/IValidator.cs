namespace Common.Core.AspNet.Test.Validators
{
    public interface IValidator<out T> where T : class
    {
        ValidationResult Validate();
    }

    public record ValidationResult(ValidateResult result, string exception);

    public enum ValidateResult
    {
        Success,
        Fail
    }
}

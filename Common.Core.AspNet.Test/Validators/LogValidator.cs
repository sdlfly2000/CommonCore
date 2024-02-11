using Common.Core.AOP.Cache;
using Common.Core.AspNet.Test.Models;
using Common.Core.DependencyInjection;

namespace Common.Core.AspNet.Test.Validators
{
    // Key point covarience [out] in IValidator
    [ServiceLocate(typeof(IValidator<IReference>))]
    public class LogValidator : IValidator<LogObject>
    {
        public ValidationResult Validate()
        {
            return new ValidationResult(ValidateResult.Success, string.Empty);
        }
    }
}

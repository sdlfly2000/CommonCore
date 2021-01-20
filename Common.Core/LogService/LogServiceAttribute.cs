using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Common.Core.LogService
{
    public class LogServiceAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var args = new StringBuilder();

            var argPairs = context.ActionArguments;

            foreach (var argPair in argPairs)
            {
                args.Append(argPair.Key + " : " + JsonConvert.SerializeObject(argPair.Value) + Environment.NewLine);
            }

            var action = context.ActionDescriptor.DisplayName;

            LogService.Info<LogServiceAttribute>(
                action + Environment.NewLine + args.ToString());
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var result = context.Result as ObjectResult;
            var statusCode = JsonConvert.SerializeObject(result.StatusCode);
            var value = JsonConvert.SerializeObject(result.Value);

            LogService.Info<LogServiceAttribute>(
                "Return: " + Environment.NewLine +
                "StatusCode: " + statusCode + Environment.NewLine +
                "Value: " + value + Environment.NewLine);
        }
    }
}

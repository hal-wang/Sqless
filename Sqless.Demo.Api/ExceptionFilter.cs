using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sqless.Access;
using System.Threading.Tasks;

namespace Demo.Sqless.Api {
    public class ExceptionFilter : IAsyncExceptionFilter {
        public async Task OnExceptionAsync(ExceptionContext context) {
            if (context.Exception is SqlessUnauthorizedAccessException) {
                context.Result = new ObjectResult(new {
                    Message = "Error account or password"
                }) {
                    StatusCode = 400,
                };
                context.ExceptionHandled = true;
            }
            await Task.CompletedTask;
        }
    }
}

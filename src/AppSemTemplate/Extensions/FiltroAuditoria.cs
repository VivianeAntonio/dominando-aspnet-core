using AppSemTemplate.Controllers;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppSemTemplate.Extensions
{
    public class FiltroAuditoria : IActionFilter
    {
        private readonly ILogger<FiltroAuditoria> _logger;

        public FiltroAuditoria(ILogger<FiltroAuditoria> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated) {
                var message = context.HttpContext.User.Identity.Name + " Acessou: " +
                    context.HttpContext.Request.GetDisplayUrl();

                //_logger.LogWarning(message);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // TO DO
        }
    }
}

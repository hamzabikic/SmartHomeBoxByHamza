using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartHomeApi.Helpers
{
    public class AuthAttribute : TypeFilterAttribute
    {
        public AuthAttribute() : base(typeof(AuthFilter))
        {

        }
    }
    public class AuthFilter : IAsyncActionFilter
    {
        private readonly AuthService auth;
        public AuthFilter(AuthService _auth)
        {
            auth = _auth;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var prijavaInfo = await auth.getInfo();
            if(!prijavaInfo.jeLogiran)
            {
                throw new Exception("Nemate pravo pristupa!");
            }
            await next();
        }
    }
}

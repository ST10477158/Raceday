using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace RaceDay.Web.Filters
{
    public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; } = "";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.Session.GetString("User_Role");
            if (string.IsNullOrEmpty(role))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
            if (!string.IsNullOrEmpty(Roles))
            {
                var allowed = Roles.Split(',');
                if (!allowed.Contains(role))
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }
        }
    }
}
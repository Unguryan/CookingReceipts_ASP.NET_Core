using IDS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IDS.Attributes
{
    public class CustomTokenAuthentication : Attribute, IAuthorizationFilter
    {
        private readonly string _roles;

        public CustomTokenAuthentication(string roles)
        {
            _roles = roles;
        }

        public string Roles => _roles;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenManager = (ITokenManager)context.HttpContext.RequestServices.GetService(typeof(ITokenManager));

            var res = true;
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization") &&
                !context.HttpContext.Request.Query.ContainsKey("token"))
            {
                res = false;
            }

            if (res)
            {
                var token = context.HttpContext.Request.Headers["Authorization"];
                    //?? .FirstOrDefault(x => x.Key == "token").Value;
                if (string.IsNullOrEmpty(token))
                {
                    token = context.HttpContext.Request.Query["token"];
                }

                if(token.ToString().Contains("Bearer "))
                {
                    token = token.ToString().Replace("Bearer ", "");
                }
                var result = tokenManager.VerifyToken(token);
                result.Wait();
                if (!result.Result)
                {
                    res = false;
                }
                else
                {
                    var roles = tokenManager.GetUserRoles(token);
                    roles.Wait();

                    var expectedRoles = roles.Result.ToLower().Replace(" ", "").Split(',');
                    var actualRoles = Roles.ToLower().Replace(" ", "").Split(',');

                    if (!expectedRoles.Any(x => actualRoles.FirstOrDefault(y => y.Equals(x)) != null))
                    {
                        res = false;
                    }
                }
            }

            if (!res)
            {
                context.ModelState.AddModelError("Unauthorized", "You are not authorized");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
        }
    }
}

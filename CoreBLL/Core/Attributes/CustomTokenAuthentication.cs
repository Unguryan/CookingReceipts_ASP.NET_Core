using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;

namespace Core.Attributes
{
    public class CustomTokenAuthentication : Attribute, IAuthorizationFilter
    {
        private readonly string _roles;

        private const string BaseUrl = "https://localhost:5001/User";

        public CustomTokenAuthentication(string roles)
        {
            _roles = roles;
        }

        public string Roles => _roles;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var tokenManager = (ITokenManager)context.HttpContext.RequestServices.GetService(typeof(ITokenManager));

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

                //var client = new HttpClient();
                //var json = JsonConvert.SerializeObject(token);
                //var data = new StringContent(json, Encoding.UTF8, "application/json");

                //TODO: Have to fix it
                //var verifyRaw = client.PostAsync($"{BaseUrl}/VerifyToken", data);
                //verifyRaw.Wait();
                //var verified = verifyRaw.Result.Content.ReadAsStringAsync();
                //verified.Wait();

                var result = VerifyToken(token);

                result.Wait();

                if (!result.Result)
                {
                    res = false;
                }
                else
                {
                    var roles = GetUserRoles(token);
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

        private async Task<string> GetUserRoles(string token)
        {
            HttpClient client = new HttpClient();

            var json = JsonConvert.SerializeObject(new {Token = token, UserName = "" });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync($"{BaseUrl}/GetUserRoles", data);

            var roles = await res.Content.ReadAsStringAsync();
            return roles;
        }

        private async Task<bool> VerifyToken(/*ITokenUsernameModel*/string token)
        {
            HttpClient client = new HttpClient();

            var json = JsonConvert.SerializeObject(new { Token = token, UserName = "" });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync($"{BaseUrl}/VerifyToken", data);

            var verified = await res.Content.ReadAsStringAsync();

            return bool.Parse(verified);
        }

    }
}

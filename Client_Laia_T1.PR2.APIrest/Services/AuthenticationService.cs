using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Client_Laia_T1.PR2.APIrest.Models;

namespace Client_Laia_T1.PR2.APIrest.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<(bool Success, string Message)> Login(LoginClient loginModel)
        {
            // Enviar las credenciales
            var content = new StringContent(
                JsonSerializer.Serialize(loginModel),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync("api/authentication/login", content);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Login response body: " + body);
            if (!response.IsSuccessStatusCode)
            {
                return (false, body);
            }

            var token = body.Trim();

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt;
            try
            {
                jwt = handler.ReadJwtToken(token);
            }
            catch
            {
                return (false, "El form at del token no es correcte");
            }

            // Crear identidad 
            var identity = new ClaimsIdentity(
                jwt.Claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = loginModel.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                    AllowRefresh = true
                });

            // guardar el token
            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                "authenticationToken",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = loginModel.RememberMe
                        ? DateTimeOffset.UtcNow.AddDays(7)
                        : DateTimeOffset.UtcNow.AddHours(1)
                });

            return (true, "");
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            var apiTokenCookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("ApiToken", apiTokenCookieOptions);
        }

    }
}

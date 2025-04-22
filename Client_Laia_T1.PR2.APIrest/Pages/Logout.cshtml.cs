using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client_Laia_T1.PR2.APIrest.Pages
{
    public class LogoutModel : PageModel
    {

        public async Task<IActionResult> OnGetAsync()
        {
            
            // quita la cookie principal
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            var apiTokenCookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(-1), 
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };

            HttpContext.Response.Cookies.Delete("authToken", apiTokenCookieOptions);

            TempData["LogoutMessage"] = "Fins aviat!";

            return RedirectToPage("ViewGames");
        }
    }
}

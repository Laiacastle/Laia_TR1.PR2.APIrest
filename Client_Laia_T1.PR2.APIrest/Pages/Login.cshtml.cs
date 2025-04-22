

using Client_Laia_T1.PR2.APIrest.Models;
using Client_Laia_T1.PR2.APIrest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client_Laia_T1.PR2.APIrest.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly AuthenticationService _authService;

        [BindProperty]
        public LoginClient Login { get; set; } = new LoginClient();
        public string? ErrorMessage { get; set; }

        public LoginModel(ILogger<LoginModel> logging, AuthenticationService authService)
        {
            _logger = logging;
            _authService = authService;
        }


        public void OnGet() { TempData.Clear(); }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
                
            using (var client = new HttpClient())
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var result = await _authService.Login(Login);

                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return Page();
                }

                TempData["SuccessMessage"] = "Login correcte";
                return RedirectToPage("ViewGames");
            }
        }
    }
}


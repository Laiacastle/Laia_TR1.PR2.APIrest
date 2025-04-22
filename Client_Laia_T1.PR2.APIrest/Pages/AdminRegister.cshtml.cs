using Api_Laia_T1.PR2.APIrest.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client_Laia_T1.PR2.APIrest.Pages
{
    public class AdminRegisterModel : PageModel
    {
        private readonly ILogger _logger;
        [BindProperty]
        public RegisterDTO Register { get; set; } = new RegisterDTO();
        public string? ErrorMessage { get; set; }

        public AdminRegisterModel(ILogger<AdminRegisterModel> logging)
        {
            _logger = logging;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7062/api/Authentication/");
                    var response = await client.PostAsJsonAsync("admin/register", Register);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Register failed");
                        ErrorMessage = "Error en alguna credencial o l'usuari ja està registrat.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durant el registre");
                ErrorMessage = "Error inesperat. Torna-ho a intentar.";
            }

            return RedirectToPage("Login");
        }
    }
}

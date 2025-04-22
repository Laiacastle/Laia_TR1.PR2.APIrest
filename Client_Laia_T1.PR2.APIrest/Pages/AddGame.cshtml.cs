using Api_Laia_T1.PR2.APIrest.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client_Laia_T1.PR2.APIrest.Pages
{
    public class AddGameModel : PageModel
    {
        private readonly ILogger _logger;
        public string? ErrorMessage { get; set; }
        [BindProperty]
        public GameDTO game { get; set; } = new GameDTO();
        public AddGameModel(ILogger<AddGameModel> logging)
        {
            _logger = logging;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7062");

                    var token = HttpContext.Session.GetString("AuthenticationToken");

                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }
                    var response = await client.PostAsJsonAsync("/api/Game", game);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Add Game failed!");
                        ErrorMessage = "No s'ha pogut afegir el joc";
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durant l'afegiment del joc");
                ErrorMessage = "Error inesperat. Torna-ho a intentar.";
            }
            return RedirectToPage("ViewGames");
        }

    }
}



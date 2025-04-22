using Api_Laia_T1.PR2.APIrest.Models.DTOs;
using Api_Laia_T1.PR2.APIrest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;

namespace Client_Laia_T1.PR2.APIrest.Pages
{
    public class ViewGamesModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public List<Game> games { get; set; }
        public string? ErrorMessage { get; set; }
        //no lo uso ya que no me va la parte del token entonces siempre es falso pero era para no mostrar el boton de votar si era true
        public bool UserLoggedIn { get; private set; } = false;

        public ViewGamesModel(ILogger<ViewGamesModel> logging)
        {
            _logger = logging;
        }

        public async Task OnGetAsync()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    var token = HttpContext.Session.GetString("AuthenticationToken");

                    if (!string.IsNullOrEmpty(token))
                    {
                        UserLoggedIn = true;
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        _logger.LogInformation("Usuari loggejat");
                    }

                    client.BaseAddress = new Uri("https://localhost:7062");
                    var response = await client.GetAsync("/api/Game");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadFromJsonAsync<List<GameDTO>>();
                        if (json != null)
                        {
                            var gamesApi = json.OrderByDescending(n => n.Users.Count).ToList();
                            games = gamesApi.Select(n => new Game
                            {
                                Id = n.Id,
                                Title = n.Title,
                                Description = n.Description,
                                Desenv = n.Desenv,
                                Img = n.Img,
                                Users = n.Users.Select(u => new User { Name = u }).ToList()
                            }).ToList();
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Game list failed!");
                        ErrorMessage = "No s'han pogut carregar els jocs.";
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durant la cárrega dels jocs");
                ErrorMessage = "Error inesperat. Torna-ho a intentar.";
            }

        }

        public async Task<IActionResult> OnPostAddVoteAsync(int id)
        {
            //No me pilla el toquen y da nulo aqui
            var token = HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "No has iniciat sessio!";
                return RedirectToPage();
            }

            var client = _httpClientFactory.CreateClient("ApiGames");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiUrl = $"api/Vote/{id}";
                _logger.LogInformation("Enviant petició POST a {ApiUrl} per afegir vot.", apiUrl);
                var response = await client.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Vot afegit correctament per al joc ID: {id}");
                    TempData["SuccessMessage"] = "Vot afegit amb èxit!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Error en afegir vot per al joc ID {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError( "Error inesperat, torna-ho a intentar: {0}", ex);
                TempData["ErrorMessage"] = "Error en votar-ne el joc";
            }

            return RedirectToPage();
        }

    }
}
    

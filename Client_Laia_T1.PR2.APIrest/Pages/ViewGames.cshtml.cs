using Api_Laia_T1.PR2.APIrest.Models.DTOs;
using Api_Laia_T1.PR2.APIrest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client_Laia_T1.PR2.APIrest.Pages
{
    public class ViewGamesModel : PageModel
    {
        private readonly ILogger _logger;

        public List<Game> games { get; set; }
        public string? ErrorMessage { get; set; }

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
    }
}
    

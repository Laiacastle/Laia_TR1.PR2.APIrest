using Api_Laia_T1.PR2.APIrest.Models.DTOs;
using Client_Laia_T1.PR2.APIrest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client_Laia_T1.PR2.APIrest.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly GamesUtils _gameService;

        public ProfileModel(GamesUtils gameService, IHttpClientFactory httpClientFactory)
        {
            _gameService = gameService;
        }

        public List<GameDTO> VotedGames { get; set; } = new List<GameDTO>();
        public int UserComments { get; set; }
        public bool Admin { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToPage("/Login");
            }

            var userName = HttpContext.User.Identity.Name;

            try
            {
                var Games = await _gameService.GetGames();
                var votedGames = Games.Where(n => n.Users.Select(u => u.Name).Contains(userName)).ToList();
                VotedGames = votedGames.Select(n => new GameDTO
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    Desenv = n.Desenv,
                    Img = n.Img,
                    Users = n.Users.Select(u => u.Name).ToList()
                }).ToList();

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al perfil: {ex.Message}");
                return Page();
            }
        }

    }
}


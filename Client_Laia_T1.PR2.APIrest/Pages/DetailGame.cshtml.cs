using Api_Laia_T1.PR2.APIrest.Models;
using Client_Laia_T1.PR2.APIrest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client_Laia_T1.PR2.APIrest.Pages
{
    public class DetailGameModel : PageModel
    {
        public readonly GamesUtils _gameService;

        public DetailGameModel(GamesUtils gameService)
        {
            _gameService = gameService;
        }

        public Game Game { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Game = await _gameService.GetGameById(id);
            return Page();
        }
    }
}


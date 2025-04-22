using Api_Laia_T1.PR2.APIrest.Models.DTOs;
using Api_Laia_T1.PR2.APIrest.Models;

namespace Client_Laia_T1.PR2.APIrest.Services
{
    public class GamesUtils
    {
        private readonly HttpClient _httpClient;

        public GamesUtils(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthorizedClient");
        }

        public string ApiErrorMessage { get; set; } = string.Empty;

        public async Task<List<Game>> GetGames()
        {

            var response = await _httpClient.GetAsync("/api/Game");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadFromJsonAsync<List<GameDTO>>();
                var gamesApi = json.OrderByDescending(n => n.Users.Count).ToList();
                var games = gamesApi.Select(n => new Game
                {
                    Title = n.Title,
                    Description = n.Description,
                    Desenv = n.Desenv,
                    Img = n.Img,
                    Users = n.Users.Select(u => new User { Name = u }).ToList()
                }).ToList();
                return games ?? new List<Game>();
            }
            else
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error ({response.StatusCode}): {body}");
            }
        }

        public async Task<Game> GetGameById(int gameId)
        {
            var response = await _httpClient.GetAsync($"/api/Game/{gameId}");
            if (response.IsSuccessStatusCode)
            {
                var gameApi = await response.Content.ReadFromJsonAsync<GameDTO>();
                var game = new Game
                {
                    Title = gameApi.Title,
                    Description = gameApi.Description,
                    Desenv = gameApi.Desenv,
                    Img = gameApi.Img,
                    Users = gameApi.Users.Select(u => new User { Name = u }).ToList()
                };
                return game;
            }
            else
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error ({response.StatusCode}): {body}");
            }
        }

        public async Task<bool> Vote(int gameId, string username)
        {
            var url = $"api/Games/vote?gameId={gameId}&userName={username}";
            var response = await _httpClient.PostAsync(url, null);
            return response.IsSuccessStatusCode;
        }

    }
}

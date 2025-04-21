using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laia_T1.PR2.APIrest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameAsyncController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GameAsyncController(ApplicationDbContext context) { _context = context; }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetAll()
        {
            var games = await _context.Games.Include(n => n.Users).ToListAsync();
            if (games.Count == 0)
            {
                return NotFound("Encara no hi han jocs a la base de dades!");
            }

            // M apeamos para pasarlo al dto para evitar el error de infinidad
            var gameDTO = games.Select(n => new GameDTO
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                Desenv = n.Desenv,
                Img = n.Img,
                Users = n.Users.Select(u => u.UserName.ToString()).ToList()
            }).ToList();

            return Ok(gameDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetById(int id)
        {
            var game = await _context.Games.Include(n => n.Users).FirstOrDefaultAsync(n => n.Id == id);
            if (game == null)
            {
                return NotFound("No s'ha trobat el joc");
            }
            var gameDTO = new GameDTO
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Desenv = game.Desenv,
                Img = game.Img,
                Users = game.Users.Select(u => u.UserName).ToList()
            };
            return Ok(gameDTO);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Game>> Add(GameDTO gameDTO)
        {
            var game = new Game { Title = gameDTO.Title, Description = gameDTO.Description, Desenv = gameDTO.Desenv, Img = gameDTO.Img };
            try
            {
                _context.Games.Add(game);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAll), game);
            }
            catch (DbUpdateException)
            {
                return BadRequest("Dades erroneas");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}")]
        public async Task<ActionResult<Game>> Delete(int id)
        {
            var game = await _context.Games.FindAsync(id);
            try
            {
                _context.Games.Remove(game);
                _context.SaveChanges();
                return Ok(game);
            }
            catch (DbUpdateException)
            {
                return BadRequest("No s'ha pogut esborrar el joc");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Game>> Update(GameDTO gameDTO, int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(n => n.Id == gameDTO.Id);
            var newGame = new Game { Id = id, Title = gameDTO.Title, Description = gameDTO.Description, Desenv = gameDTO.Desenv, Img = gameDTO.Img };
            try
            {
                _context.Games.Update(newGame);
                await _context.SaveChangesAsync();
                return Ok(newGame);
            }
            catch (DbUpdateException)
            {
                return BadRequest("no s'ha pogut fer l'update");
            }
        }
        //[Authorize]
        [HttpPut("/Vote/{id}/{idUser}")]
        public async Task<ActionResult> VoteGame(int id, string idUser)
        {
            var game = await _context.Games.FirstOrDefaultAsync(n => n.Id == id);
            var user = await _context.Users.OfType<User>().FirstOrDefaultAsync(n => n.Id == idUser);
            try
            {
                var userHasVoted = game.Users.Any(u => u.Id == idUser);

                if (userHasVoted)
                {
                    user.Games.Remove(game);
                    game.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return Ok($"Vot per {game.Title} eliminat");
                }
                else
                {
                    user.Games.Add(game);
                    game.Users.Add(user);
                    await _context.SaveChangesAsync();
                    return Ok($"Vot per {game.Title} afegit");
                }
            }
            catch (DbUpdateException)
            {
                return BadRequest("No s'ha pogut votar el joc escollit");
            }
        }
    }
}


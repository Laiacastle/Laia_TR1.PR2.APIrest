using Api_Laia_T1.PR2.APIrest.Data;
using Api_Laia_T1.PR2.APIrest.Models.DTOs;
using Api_Laia_T1.PR2.APIrest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Api_Laia_T1.PR2.APIrest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _configuration;
        public AuthenticationController(UserManager<User> userManager, ILogger<AuthenticationController> logger, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _context.Users.OfType<User>().Include(n => n.Games).ToListAsync();
            if (users.Count == 0)
            {
                return NotFound("Encara no hi han usuaris a la base de dades!");
            }
            //mapeamos para q no haya el error de infinidad
            var userDTO = users.Select(n => new UserDTO
            {
                Id = n.Id,
                Email = n.Email,
                Password = n.PasswordHash,
                UserName = n.UserName,
                Name = n.Name,
                Surname = n.Surname,
                Games = n.Games.Select(g => g.Title.ToString()).ToList()
            }).ToList();
            return Ok(userDTO);
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterDTO model)
        {
            var newUser = new User { Name = model.Name, UserName = model.UserName, Email = model.Email, Surname = model.Surname };
            var resultat = await _userManager.CreateAsync(newUser, model.Password);
            if (resultat.Succeeded)
            {
                return Ok("Usuari registrat");
            }
            return BadRequest(resultat.Errors);

        }

        [HttpPost("admin/registre")]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterDTO model)
        {
            var user = new User { Name = model.Name, UserName = model.UserName, Email = model.Email, Surname = model.Surname };
            var result = await _userManager.CreateAsync(user, model.Password);
            var resultRol = new IdentityResult();
            if (result.Succeeded)
            {
                resultRol = await _userManager.AddToRoleAsync(user, "Admin");
                _logger.LogInformation($"Rols assignats a {user.UserName}: {string.Join(", ", resultRol)}");
            }
            if (result.Succeeded && resultRol.Succeeded)
            {
                return Ok("Administrador registrat");
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Mail o contrasenya erronis");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var roles = await _userManager.GetRolesAsync(user);
            if (roles != null && roles.Count > 0)
            {
                foreach (var rol in roles)
                {
                    claims.Add(new Claim("role", rol));
                }
            }
            return Ok(CreateToken(claims.ToArray()));
        }

        private string CreateToken(Claim[] claims)
        {
            var jwtConfig = _configuration.GetSection("jwtSettings");
            var secretKey = jwtConfig["Key"];
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];
            var expirationMinutes = int.Parse(jwtConfig["ExpirationMinutes"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(claims);
        }
    }
}

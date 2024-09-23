using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        // Hash da senha (é importante implementar uma hash segura)
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Não retorna a senha
        return Ok(new { user.Id, user.UserName });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User User)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.UserName);
        if (user == null || !BCrypt.Net.BCrypt.Verify(User.Password, user.Password))
        {
            return Unauthorized("Invalid username or password");
        }

        // Gerar o token JWT
        var token = GenerateJwtToken(user);
        return Ok(new { user.Id, user.UserName, Token = token });
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _context.Users
            .Include(u => u.RegisterUsers)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        var token = GenerateJwtToken(user);
        // Não retorna a senha
        return Ok(new { user.Id, user.UserName, Token = token });
    }

    private string GenerateJwtToken(User user)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes("yourSuperLongSecretKeyWith32Characters!");

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        }),
        Expires = DateTime.UtcNow.AddHours(1), // Defina o tempo de expiração do token
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        Issuer = "yourIssuer",
        Audience = "yourAudience"
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}
}

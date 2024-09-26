// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using api.Data;
// using api.Model;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;

// [ApiController]
// [Route("api/[controller]")]
// public class RegisterUserController : ControllerBase
// {
//     private readonly AppDbContext _context;
//     private readonly IHttpContextAccessor _httpContextAccessor;

//     public RegisterUserController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
//     {
//         _context = context;
//         _httpContextAccessor = httpContextAccessor;
//     }
//     public static ClaimsPrincipal DecodeJwtToken(string token, string secretKey)
//     {
//         var tokenHandler = new JwtSecurityTokenHandler();
//         var key = Convert.FromBase64String(secretKey);
//         var validationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(key),
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = false
//         };

//         try
//         {
//             ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
//             return principal;
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Erro ao validar token: {ex.Message}");
//             return null;
//         }
//     }

//     private async Task<int?> GetCurrentUserIdAsync(int userId)
//     {
//         var user = await _context.Users.FindAsync(userId);
//         if (user == null)
//         {
//             return null;
//         }
//         return userId;
//     }

//     [HttpPost]
//     public async Task<IActionResult> CreateRegisterUser([FromBody] RegisterUser registerUser, SecurityToken token)
//     {
//         var userId = await GetCurrentUserIdAsync(registerUser.UserId);
//         if (userId == null)
//         {
//             return NotFound(new { message = "Usuário não encontrado" });
//         }

//         // registerUser.UserId = userId.Value;

//         // _context.RegisterUsers.Add(registerUser);
//         // await _context.SaveChangesAsync();

//         return Ok(token);
//     }

//     [HttpGet]
//     public async Task<IActionResult> GetRegisterUsers()
//     {
//         var registerUsers = await _context.RegisterUsers.ToListAsync();
//         return Ok(registerUsers);
//     }

//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetRegisterUserById(int id)
//     {
//         var userId = await GetCurrentUserIdAsync(id);

//         var registerUser = await _context.RegisterUsers.FirstOrDefaultAsync(r => r.Id == id);

//         return Ok(registerUser);
//     }

//     [HttpPut("{id}")]
//     public async Task<IActionResult> UpdateRegisterUser(int id, [FromBody] RegisterUser updatedRegisterUser)
//     {
//         var userId = await GetCurrentUserIdAsync(id);

//         var registerUser = await _context.RegisterUsers.FirstOrDefaultAsync(r => r.Id == id);

//         registerUser.Name = updatedRegisterUser.Name;
//         registerUser.Email = updatedRegisterUser.Email;
//         registerUser.Cpf = updatedRegisterUser.Cpf;
//         registerUser.Celphone = updatedRegisterUser.Celphone;
//         registerUser.Address = updatedRegisterUser.Address;

//         await _context.SaveChangesAsync();

//         return Ok(registerUser);
//     }

//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteRegisterUser(int id)
//     {
//         var userId = await GetCurrentUserIdAsync(id);
//         Console.WriteLine(userId);

//         var registerUser = await _context.RegisterUsers.FirstOrDefaultAsync(r => r.Id == id);

//         _context.RegisterUsers.Remove(registerUser);
//         await _context.SaveChangesAsync();

//         return NoContent();
//     }
// }



using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Data;
using api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class RegisterUserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RegisterUserController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public static ClaimsPrincipal DecodeJwtToken(string token, string secretKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(secretKey);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };

        try
        {
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao validar token: {ex.Message}");
            return null;
        }
    }

    private async Task<int?> GetCurrentUserIdAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return null;
        }
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRegisterUser([FromBody] RegisterUser registerUser)
    {
        var userId = await GetCurrentUserIdAsync(registerUser.UserId);
        if (userId == null)
        {
            return NotFound(new { message = "Usuário não encontrado" });
        }

        registerUser.UserId = userId.Value;

        _context.RegisterUsers.Add(registerUser);
        await _context.SaveChangesAsync();

        return Ok(registerUser);
    }

    [HttpGet]
    public async Task<IActionResult> GetRegisterUsers()
    {
        var registerUsers = await _context.RegisterUsers.ToListAsync();
        return Ok(registerUsers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRegisterUserById(int id)
    {
        var userId = await GetCurrentUserIdAsync(id);

        var registerUser = await _context.RegisterUsers.FirstOrDefaultAsync(r => r.Id == id );

        return Ok(registerUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRegisterUser(int id, [FromBody] RegisterUser updatedRegisterUser)
    {
        var userId = await GetCurrentUserIdAsync(id);       

        var registerUser = await _context.RegisterUsers.FirstOrDefaultAsync(r => r.Id == id);

        registerUser.Name = updatedRegisterUser.Name;
        registerUser.Email = updatedRegisterUser.Email;
        registerUser.Cpf = updatedRegisterUser.Cpf;
        registerUser.Celphone = updatedRegisterUser.Celphone;
        registerUser.Address = updatedRegisterUser.Address;

        await _context.SaveChangesAsync();

        return Ok(registerUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRegisterUser(int id)
    {
        var userId = await GetCurrentUserIdAsync(id);
        Console.WriteLine(userId);

        var registerUser = await _context.RegisterUsers.FirstOrDefaultAsync(r => r.Id == id);

        _context.RegisterUsers.Remove(registerUser);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}


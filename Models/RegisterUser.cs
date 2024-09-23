

using api.Models;

namespace api.Model
{

public class RegisterUser
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Cpf { get; set; }
    public required string Celphone { get; set; }
    public required string Address { get; set; }

    // Relacionamento com User
    public int UserId { get; set; }
    // public User? User { get; set; }
}};

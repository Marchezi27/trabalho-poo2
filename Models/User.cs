using api.Model;

namespace api.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public List<RegisterUser>? RegisterUsers { get; set; }
    }
};

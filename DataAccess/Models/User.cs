using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        public string Role { get; set; } = "Guest"; // "Admin", "Manager", "Registered", "Guest"
    }
}

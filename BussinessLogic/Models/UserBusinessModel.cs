
namespace BussinessLogic.Models
{
    public class UserBusinessModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string Role { get; set; } // "Admin", "Manager", "Registered", "Guest"
    }
}

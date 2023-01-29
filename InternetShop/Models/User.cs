using Microsoft.AspNetCore.Identity;

namespace InternetShop.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public int RoleId { get; set; }

       
    }
}

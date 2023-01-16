
namespace FinalProjectBackend.DTOModel
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    


    }
}

namespace FinalProjectBackend.DTOModel
{
    public class LoginDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
     


    }
}




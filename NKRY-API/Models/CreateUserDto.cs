using System.Security.Cryptography;
using System.Text;
using NKRY_API.Utilities;

namespace NKRY_API.Models
{
    public class CreateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        private string? _password;
        public string Password
        {
            get
            {
                try
                {
                    return _password;
                }
                catch (Exception)
                {

                    throw new ArgumentNullException("Password is undefined or not yet created");
                }
            }
            set
            {
                // here we pass a variable "salt" that will be modified by the function this why we use "out"
                // "it must be preceded by the out keyword,
                // indicating that you expect the function to assign a value to it"
                byte[] salt;
                string hashedPassword = value.HashPassword(salt: out salt);
                _password = hashedPassword;
            }
        }
        public int? DepartmentId { get; set; }



    }
}

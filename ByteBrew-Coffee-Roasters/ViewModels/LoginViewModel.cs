using System.ComponentModel.DataAnnotations;

namespace ByteBrew_Coffee_Roasters.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public LoginViewModel()
        {
        }

        public LoginViewModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}

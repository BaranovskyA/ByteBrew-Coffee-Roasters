using System.ComponentModel.DataAnnotations;

namespace ByteBrew_Coffee_Roasters.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Логин должен содержать от 6 до 32 символов")]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 32 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 32 символов")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public Guid RoleId { get; set; }

        public RegisterViewModel()
        {
        }

        public RegisterViewModel(string userName, string password, string confirmPassword, Guid roleId)
        {
            UserName = userName;
            Password = password;
            ConfirmPassword = confirmPassword;
            RoleId = roleId;
        }
    }
}

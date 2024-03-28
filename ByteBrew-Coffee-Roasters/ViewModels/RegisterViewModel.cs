using System.ComponentModel.DataAnnotations;

namespace ByteBrew_Coffee_Roasters.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public Guid RoleId { get; set; }

        public RegisterViewModel()
        {
        }

        public RegisterViewModel(string userName, string password, Guid roleId)
        {
            UserName = userName;
            Password = password;
            RoleId = roleId;
        }
    }
}

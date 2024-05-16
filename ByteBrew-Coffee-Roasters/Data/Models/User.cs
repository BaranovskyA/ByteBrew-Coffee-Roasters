using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteBrew_Coffee_Roasters.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        public override Guid Id { get; set; }
        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }
        [DeleteBehavior(DeleteBehavior.SetNull)]
        public Role? Role { get; set; }

        public User(Guid roleId, string userName)
        {
            Id = Guid.NewGuid();
            RoleId = roleId;
            UserName = userName;
            NormalizedUserName = userName.ToUpper();
        }
    }
}

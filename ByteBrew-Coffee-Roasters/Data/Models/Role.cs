using Microsoft.AspNetCore.Identity;

namespace ByteBrew_Coffee_Roasters.Data.Models
{
    public class Role : IdentityRole<Guid>
    {
        public override Guid Id { get; set; }

        public Role(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}

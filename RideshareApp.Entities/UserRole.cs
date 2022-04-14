using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace RideshareApp.Entities
{
    [Comment("Роли на потребители")]
    public class UserRole : BaseEntity
    {
        [Comment("Потребител")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Comment("Роля")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}

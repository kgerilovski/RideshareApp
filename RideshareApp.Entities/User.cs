using Microsoft.EntityFrameworkCore;
using RideshareApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class User : BaseDeletableEntity
    {
        [Required]
        [Comment("Потребителско име")]
        public string Login { get; set; }

        [Comment("Парола")]
        public string Password { get; set; }

        [Comment("Име")]
        public string FirstName { get; set; }

        [Comment("Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Comment("Електронна поща")]
        public string Email { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public string FullName() => FirstName + " " + LastName;
    }
}
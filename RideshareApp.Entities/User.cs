using Microsoft.EntityFrameworkCore;
using RideshareApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class User : BaseDeletableEntity
    {
        [Required]
        [Comment("������������� ���")]
        public string Login { get; set; }

        [Comment("������")]
        public string Password { get; set; }

        [Comment("���")]
        public string FirstName { get; set; }

        [Comment("�������")]
        public string LastName { get; set; }

        [Required]
        [Comment("���������� ����")]
        public string Email { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public string FullName() => FirstName + " " + LastName;
    }
}
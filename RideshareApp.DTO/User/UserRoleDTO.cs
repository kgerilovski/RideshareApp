namespace RideshareApp.DTO.User
{
    public class UserRoleDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserLogin { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}

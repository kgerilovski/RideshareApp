using Microsoft.EntityFrameworkCore;
using RideshareApp.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideshareApp.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            this.EntityState = EntityStateEnum.Unchanged;
        }

        public BaseEntity(string primaryKeyName) : this()
        {
            this.PrimaryKeyName = primaryKeyName;
        }

        [Comment("Id - идентификатор")]
        public int Id { get; set; }

        [Comment("Дата на insert на записа")]
        public DateTime? SysInsDate { get; set; }

        [Comment("Дата на update на записа")]
        public DateTime? SysUpdDate { get; set; }

        [NotMapped]
        public EntityStateEnum EntityState { get; set; }

        [NotMapped]
        public List<string> ModifiedProperties { get; set; }

        [NotMapped]
        public string PrimaryKeyName { get; private set; }
    }
}

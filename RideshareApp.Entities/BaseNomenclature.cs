using Microsoft.EntityFrameworkCore;
using RideshareApp.Entities.Helpers;
using System.ComponentModel.DataAnnotations;

namespace RideshareApp.Entities
{
    public abstract class BaseNomenclature : BaseEntity
    {
        [Required]
        [Comment("Код")]
        [CustomStringLength(MaxLengthEnum.Code)]
        public string Code { get; set; }

        [Comment("Наименование")]
        [CustomStringLength(MaxLengthEnum.Name)]
        public string Name { get; set; }

        // Below properties are with `new` keyword for column order in creating table in migrations

        [Comment("Дата на insert на записа")]
        public new DateTime? SysInsDate { get; set; }

        [Comment("Дата на update на записа")]
        public new DateTime? SysUpdDate { get; set; }
    }
}

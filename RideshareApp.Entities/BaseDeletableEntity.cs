using Microsoft.EntityFrameworkCore;
using RideshareApp.Entities.Common;

namespace RideshareApp.Entities
{
    public abstract class BaseDeletableEntity : BaseEntity, IDeletableEntity
    {
        [Comment("Изтрит ли е записът")]
        public bool IsDeleted { get; set; }
    }
}

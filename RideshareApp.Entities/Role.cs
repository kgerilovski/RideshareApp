using Microsoft.EntityFrameworkCore;
using RideshareApp.Entities.Helpers;

namespace RideshareApp.Entities
{
    [Comment("Роли")]
    public class Role : BaseEntity
    {
        [Comment("Наименование на роля")]
        [CustomStringLength(MaxLengthEnum.Name)]
        public string Name { get; set; }
    }
}

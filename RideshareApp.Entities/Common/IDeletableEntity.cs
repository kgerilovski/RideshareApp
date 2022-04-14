namespace RideshareApp.Entities.Common
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }
    }
}

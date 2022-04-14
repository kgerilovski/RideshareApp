using Microsoft.EntityFrameworkCore;
using RideshareApp.Services.Infrastructure;

namespace RideshareApp.DataAccess.EFCore
{
    public class DatabaseInitializer : IDataBaseInitializer
    {
        private DataContext context;

        public DatabaseInitializer(DataContext context)
        {
            this.context = context;
        }

        public void Initialize()
        {
            using (context)
            {
                // turn off timeout for initial seeding
                context.Database.SetCommandTimeout(System.TimeSpan.FromDays(1));

                // check data base version and migrate / seed if needed
                context.Database.Migrate();
            }
        }
    }
}

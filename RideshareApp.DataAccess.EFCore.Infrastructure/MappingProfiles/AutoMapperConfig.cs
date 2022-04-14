using AutoMapper;
using System.Reflection;

namespace RideshareApp.DataAccess.EFCore.Infrastructure.MappingProfiles
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfigurationExpression();
            config.AllowNullCollections = false;

            Assembly.Load("RideshareApp.DataAccess.EFCore.Infrastructure")
                .GetTypes()
                .Where(t => IsTypeAssignableFrom(t, typeof(Profile)))
                .ToList()
                .ForEach(p =>
                {
                    config.AddProfile((Profile)Activator.CreateInstance(p));
                });

            return new MapperConfiguration(config);
        }

        private static bool IsTypeAssignableFrom(Type t, Type baseType)
        {
            var result = baseType.IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic;
            return result;
        }
    }
}

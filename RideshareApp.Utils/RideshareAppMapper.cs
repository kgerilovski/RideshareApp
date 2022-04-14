using AutoMapper;
using RideshareApp.Common.Enums;
using RideshareApp.DataAccess.EFCore.Utils;
using RideshareApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideshareApp.Utils
{
    public class RideshareAppMapper
    {
        public static IMapper _mapper;

        public static RideshareAppMapper(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public static TResult MapTo<TResult>(object source)
        {
            _mapper
            return Mapper.Map<TResult>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(TSource aSource, TDestination aDestination)
        {
            return Mapper.Map(aSource, aDestination);
        }

        public static List<TDestination> MapToList<TSource, TDestination>(ICollection<TSource> aSourceList)
        {
            if (aSourceList == null)
            {
                return null;
            }

            List<TDestination> result = new List<TDestination>();

            foreach (var item in aSourceList)
            {
                result.Add(MapTo<TDestination>(item));
            }

            return result;
        }

        public static EntityType MapToEntity<ViewModelType, EntityType>(ViewModelType viewModel, bool isAdded)
            where ViewModelType : class
            where EntityType : BaseEntity
        {
            if (viewModel == null)
            {
                return null;
            }

            EntityType entity = Mapper.Map<ViewModelType, EntityType>(viewModel);
            ApplyChangesToEntity(viewModel, entity, isAdded);

            return entity;
        }

        private static void ApplyChangesToEntity<ViewModelType, EntityType>(ViewModelType viewModel, EntityType entity, bool isAdded)
            where ViewModelType : class
            where EntityType : BaseEntity
        {
            if (isAdded)
            {
                entity.EntityState = EntityStateEnum.Added;

                var navigationProperties = DBContextExtensions.GetNavigationDependencies(entity);
                foreach (var property in navigationProperties)
                {
                    property.EntityState = EntityStateEnum.Added;
                }
            }
            else
            {
                entity.EntityState = EntityStateEnum.Modified;

                var destMappedProperties = GetDestMappedProperties(typeof(ViewModelType), typeof(EntityType));
                if (entity.ModifiedProperties == null)
                {
                    entity.ModifiedProperties = destMappedProperties;
                }
                else
                {
                    var uniqueProperties = new HashSet<string>(entity.ModifiedProperties);
                    uniqueProperties.UnionWith(destMappedProperties);

                    entity.ModifiedProperties = uniqueProperties.ToList();
                }
            }
        }

        public static List<string> GetDestMappedProperties(Type srcType, Type destType)
        {
            TypeMap map = Mapper.Configuration.GetAllTypeMaps()
                        .Where(m => m.SourceType.Equals(srcType) && m.DestinationType.Equals(destType))
                        .FirstOrDefault();

            if (map == null) return null;

            List<PropertyMap> properties = map.GetPropertyMaps()
                .Where(x => x.SourceType != null &&
                        x.SourceType.Namespace != "System.Collections.Generic" &&
                        !x.DestinationPropertyType.IsSubclassOf(typeof(BaseEntity)) &&
                        !x.Ignored).ToList();

            if (properties == null) return null;

            List<string> destProps = properties.Select(x => x.DestinationProperty.Name)
                .Where(x => x != nameof(BaseEntity.Id))
                .Where(x => x != nameof(BaseEntity.EntityState))
                .Where(x => x != nameof(BaseEntity.PrimaryKeyName))
                .ToList();

            return destProps;
        }
    }
}

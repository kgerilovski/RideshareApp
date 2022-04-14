using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RideshareApp.Common.Enums;
using RideshareApp.DataAccess.EFCore.Infrastructure.Contracts;
using RideshareApp.DataAccess.EFCore.Utils;
using RideshareApp.Entities;
using RideshareApp.Services.Infrastructure.Services;

namespace RideshareApp.Services
{
    public abstract class BaseAsyncService<TInDTO, TOutDTO, TEntity, TPk, TContext> : IBaseAsyncService<TInDTO, TOutDTO, TEntity, TPk>
        where TInDTO : class
        where TEntity : BaseEntity
        where TPk : struct
        where TContext : DbContext
    {
        protected IBaseAsyncRepository<TEntity, TPk, TContext> baseAsyncRepository;

        /// <summary>
        /// Dictionary, в което се описва mapping-а между имената на полетата в dto-to и entity-то
        /// </summary>
        protected Dictionary<string, string> dtoFieldsToEntityFields;

        private IMapper _mapper;

        protected BaseAsyncService(IBaseAsyncRepository<TEntity, TPk, TContext> baseAsyncRepository, IMapper mapper)
        {
            this.baseAsyncRepository = baseAsyncRepository;
            this.dtoFieldsToEntityFields = new Dictionary<string, string>();
            _mapper = mapper;
        }

        public virtual List<TOutDTO> SelectAllAsync()
        {
            var query = this.GetSelectAllQueriable();
            var baseQuery = _mapper.ProjectTo<TOutDTO>(query);
            return baseQuery.ToList();
        }

        public ICollection<T> ApplyCheckboxChanges<T>(int[] ids, string foreignKey, bool isAdded, ICollection<T> dbEntities = null) where T : BaseEntity
        {
            dbEntities = dbEntities ?? new List<T>();
            var result = new List<T>();
            var fkProperty = typeof(T).GetProperty(foreignKey);
            var esProperty = typeof(T).GetProperty(nameof(BaseEntity.EntityState));

            // Mark for add
            foreach (var fkId in ids)
            {
                var contains = dbEntities.Any(x => (int)fkProperty.GetValue(x) == fkId);
                if (!contains)
                {
                    var instance = Activator.CreateInstance<T>();
                    fkProperty.SetValue(instance, fkId);
                    esProperty.SetValue(instance, EntityStateEnum.Added);
                    result.Add(instance);
                }
            }

            // Mark for delete
            foreach (var dbEntity in dbEntities)
            {
                var searchId = (int)fkProperty.GetValue(dbEntity);
                if (!ids.Contains(searchId))
                {
                    dbEntity.EntityState = EntityStateEnum.Deleted;
                    result.Add(dbEntity);
                }
            }

            return result;
        }

        public virtual async Task SaveEntityAsync(BaseEntity entity, bool applyToAllLevels = true)
        {
            var dbContext = this.baseAsyncRepository.GetDbContext();
            await dbContext.SaveEntityAsync(entity, applyToAllLevels);
        }

        protected virtual IQueryable<TEntity> GetSelectAllQueriable()
        {
            return this.baseAsyncRepository.SelectAllAsync();
        }

        protected virtual void TransformDataOnInsert(TEntity entity)
        {
        }

        protected abstract bool IsChildRecord(TPk id, List<string> aParentsList);

        public virtual async Task<TEntity> SelectAsync(TPk id)
        {
            TEntity result = await this.baseAsyncRepository.SelectAsync(id);
            return result;
        }

        public virtual async Task<int> InsertAsync(TInDTO aInDto)
        {
            this.ValidateData(aInDto);

            TEntity entity = MapToEntity<TInDTO, TEntity>(aInDto, isAdded: true);
            this.TransformDataOnInsert(entity);
            await this.SaveEntityAsync(entity);
            return entity.Id;
        }

        protected virtual void ValidateData(TInDTO aInDto)
        {
        }

        protected string FormatParentsMessage(List<string> aList)
        {
            string message = "Object is referenced by: ";
            foreach (string parent in aList)
            {
                message += "[" + parent + "]";
            }
            return "Child record found! " + message;
        }

        public EntityType MapToEntity<ViewModelType, EntityType>(ViewModelType viewModel, bool isAdded)
            where ViewModelType : class
            where EntityType : BaseEntity
        {
            if (viewModel == null)
            {
                return null;
            }

            EntityType entity = _mapper.Map<ViewModelType, EntityType>(viewModel);
            ApplyChangesToEntity(viewModel, entity, isAdded);

            return entity;
        }

        private void ApplyChangesToEntity<ViewModelType, EntityType>(ViewModelType viewModel, EntityType entity, bool isAdded)
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

        public List<string> GetDestMappedProperties(Type srcType, Type destType)
        {
            TypeMap map = _mapper.ConfigurationProvider.Internal().GetAllTypeMaps()
                        .Where(m => m.SourceType.Equals(srcType) && m.DestinationType.Equals(destType))
                        .FirstOrDefault();

            if (map == null) return null;

            List<PropertyMap> properties = map.PropertyMaps
                .Where(x => x.SourceType != null &&
                        x.SourceType.Namespace != "System.Collections.Generic" &&
                        !x.DestinationType.IsSubclassOf(typeof(BaseEntity)) &&
                        !x.Ignored).ToList();

            if (properties == null) return null;

            List<string> destProps = properties.Select(x => x.DestinationName)
                .Where(x => x != nameof(BaseEntity.Id))
                .Where(x => x != nameof(BaseEntity.EntityState))
                .Where(x => x != nameof(BaseEntity.PrimaryKeyName))
                .ToList();

            return destProps;
        }

        public virtual async Task UpdateAsync(TPk id, TInDTO inDto)
        {
            TEntity repoObj = await this.baseAsyncRepository.SelectAsync(id);
            if (repoObj == null)
            {
                throw new Exception("Object with id [" + id + "] not found!");
            }

            this.ValidateData(inDto);

            TEntity entity = MapToEntity<TInDTO, TEntity>(inDto, isAdded: false);
            await this.SaveEntityAsync(entity);
        }

        public virtual async Task DeleteAsync(TPk id)
        {
            TEntity repoObj = await this.baseAsyncRepository.SelectAsync(id);
            if (repoObj == null)
            {
                throw new Exception("Object with id [" + id + "] not found!");
            }

            List<string> parentsList = new List<string>();
            if (this.IsChildRecord(id, parentsList))
            {
                string errorMessage = this.FormatParentsMessage(parentsList);
                throw new Exception(errorMessage);
            }

            repoObj.EntityState = EntityStateEnum.Deleted;
            await this.baseAsyncRepository.GetDbContext().SaveEntityAsync(repoObj);
        }
    }
}

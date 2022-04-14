using RideshareApp.Common.Helpers;
using RideshareApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RideshareApp.DataAccess.EFCore.Utils
{
    public class DataExtensions
    {
        public static string NomenclatureUpdateSql()
        {
            var typeList = GetAllTypes();
            var nomenclatures = typeList.Where(x => typeof(BaseNomenclature).IsAssignableFrom(x.EntityType)).ToList();
            var sql = nomenclatures.Select(x => $"insert into nomenclatures(table_name, description) values ('{x.DbName}', '{x.Comment}');").ToList();
            sql.Insert(0, "delete from nomenclatures;");

            var result = string.Join(Environment.NewLine, sql);
            return result;
        }

        public static string InsertIfNotExistsIntoNomenclature(string tableName, string name, string code)
        {
            return @$"INSERT INTO {tableName}
                (code, name)
            SELECT '{code}', '{name}'
            WHERE
                NOT EXISTS(
                    SELECT code FROM {tableName} WHERE code = '{code}'
                ); ";
        }

        public static List<(Type EntityType, string DbName, string Comment)> GetAllTypes()
        {
            var data = typeof(DataContext).GetProperties()
                .Where(x => x.PropertyType.Name.StartsWith("DbSet"))
                .Select(x => new
                {
                    EntityType = x.PropertyType.GenericTypeArguments[0],
                    DbName = StringHelper.GetDbName(x.Name),
                    Comment = StringHelper.GetCommentName(x),
                })
                .OrderBy(x => x.EntityType.Name)
                .Select(x => (x.EntityType, x.DbName, x.Comment))
                .ToList();

            return data;
        }
    }
}

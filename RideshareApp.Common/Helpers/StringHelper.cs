using System.Reflection;
using System.Text.RegularExpressions;

namespace RideshareApp.Common.Helpers
{
    public static class StringHelper
    {
        public static string MultipleReplace(string text, IDictionary<string, string> replacements)
        {
            return Regex.Replace(text,
                                    "(" + String.Join("|", replacements.Keys.ToArray()) + ")",
                                    delegate (Match m) { return replacements[m.Value]; }
                                    );
        }

        public static string GetDbName(string entityName)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                return null;
            }

            var chars = entityName.ToCharArray();
            List<int> upperIndexes = new List<int>();
            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsUpper(chars[i]))
                {
                    upperIndexes.Add(i);
                }
            }

            upperIndexes.Add(entityName.Length);

            List<string> elements = new List<string>();
            for (int i = 0; i < upperIndexes.Count - 1; i++)
            {
                int start = upperIndexes[i];
                int end = upperIndexes[i + 1];
                string element = entityName.Substring(start, end - start).ToLower();
                elements.Add(element);
            }

            string result = string.Join("_", elements);
            return result;
        }

        public static string GetCommentName(PropertyInfo property)
        {
            var type = property.PropertyType.GenericTypeArguments[0];
            var attribute = type.CustomAttributes.Where(x => x.AttributeType.Name == "CommentAttribute").FirstOrDefault();
            if (attribute == null)
            {
                return null;
            }

            var result = attribute.ConstructorArguments[0].Value?.ToString();
            return result;
        }

        public static string ConvertNameToPascalCase(string name)
        {
            var parts = name.Split("_").Select(x => char.ToUpper(x[0]) + x[1..]);
            var result = string.Join("", parts);
            return result;
        }
    }
}

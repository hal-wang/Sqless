using System.Collections.Generic;
using System.Reflection;

namespace Hubery.Sqless.Request
{
    public class SqlessEditRequest : SqlessRequest
    {
        /// fields to edit
        public List<SqlessEditField> Fields { get; set; } = new List<SqlessEditField>();

        public void LoadFromObject(object obj)
        {
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty);
            foreach (var prop in props)
            {
                Fields.Add(new SqlessEditField()
                {
                    Field = prop.Name,
                    Value = prop.GetValue(obj),
                    Type = TypeConverter.TypeToDbType(prop.PropertyType)
                });
            }
        }
    }
}

using Hubery.Sqless.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hubery.Sqless.Request
{
    public class SqlessSelectRequest : SqlessRequest
    {
        public List<SqlessOrder> Orders { get; set; } = new List<SqlessOrder>();
        public List<SqlessField> Fields { get; set; } = new List<SqlessField>();
        public List<SqlessJoin> Joins { get; set; } = new List<SqlessJoin>();

        /// <summary>
        /// zero base
        /// </summary>
        public uint PageIndex { get; set; } = 0;

        public uint PageSize { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="table">if null, the table name is class name of obj param</param>
        public void LoadFromType(System.Type type, string table = null, params string[] exceptFields)
        {
            if (string.IsNullOrEmpty(table))
            {
                var fullTypeStr = type.ToString();
                if (fullTypeStr.StartsWith("<>") && fullTypeStr.Contains("AnonymousType"))
                {
                    throw new ArgumentException("the anonymous type must assign the table name");
                }

                var typeStrs = fullTypeStr.Split('.');
                table = typeStrs[typeStrs.Length - 1];
            }

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty)
                .Where(p => exceptFields.Where(ef => string.Equals(ef, p.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0).ToList();
            foreach (var prop in props)
            {
                Fields.Add(new SqlessField()
                {
                    Table = table,
                    Field = prop.Name,
                });
            }
        }
    }
}

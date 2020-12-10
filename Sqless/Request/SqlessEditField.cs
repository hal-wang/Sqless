using Hubery.Tools;
using System.Data;

namespace Sqless.Request
{
    public class SqlessEditField : ICloneable<SqlessEditField>
    {
        public string Field { get; set; }
        public object Value { get; set; }
        public DbType Type { get; set; } = DbType.String;

        public SqlessEditField DeepClone()
        {
            return new SqlessEditField()
            {
                Field = Field,
                Value = Value,
                Type = Type
            };
        }

        public SqlessEditField ShallowClone()
        {
            return DeepClone();
        }
    }
}

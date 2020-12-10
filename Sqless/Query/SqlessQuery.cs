using Hubery.Tools;

namespace Sqless.Query
{
    public class SqlessQuery : SqlessField, ICloneable<SqlessQuery>
    {
        public SqlessQueryType Type { get; set; }
        public string Value { get; set; }

        public new SqlessQuery DeepClone()
        {
            return new SqlessQuery()
            {
                Field = Field,
                Table = Table,
                Type = Type,
                Value = Value
            };
        }

        public new SqlessQuery ShallowClone()
        {
            return DeepClone();
        }
    }
}

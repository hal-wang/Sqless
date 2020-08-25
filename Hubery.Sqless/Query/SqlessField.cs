using Hubery.Tools;

namespace Hubery.Sqless.Query
{
    public class SqlessField : ICloneable<SqlessField>
    {
        public string Table { get; set; }
        public string Field { get; set; }

        public SqlessField DeepClone()
        {
            return new SqlessField()
            {
                Table = Table,
                Field = Field
            };
        }

        public SqlessField ShallowClone()
        {
            return DeepClone();
        }
    }
}

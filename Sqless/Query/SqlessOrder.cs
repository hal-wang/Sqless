using HTools;

namespace Sqless.Query
{
    public class SqlessOrder : SqlessField, ICloneable<SqlessOrder>
    {
        public SqlessQueryDirection Direction { get; set; } = SqlessQueryDirection.Asc;

        public new SqlessOrder DeepClone()
        {
            return new SqlessOrder()
            {
                Field = Field,
                Direction = Direction,
                Table = Table
            };
        }

        public new SqlessOrder ShallowClone()
        {
            return DeepClone();
        }
    }
}

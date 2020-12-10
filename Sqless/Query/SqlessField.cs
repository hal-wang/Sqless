using Hubery.Tools;

namespace Sqless.Query
{
    public class SqlessField : ICloneable<SqlessField>
    {
        /// <summary>
        /// 简单查询可忽略此属性，Join时必须指定Table
        /// </summary>
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

using Hubery.Tools;

namespace Hubery.Sqless.Query
{
    /// <summary>
    /// SQL Join
    /// 
    /// ex. SELECT * FROM <Table> <SqlessJoinType> <JoinTable> ON <Table.LeftField> = <JoinTable.JoinField> 
    /// </summary>
    public class SqlessJoin : ICloneable<SqlessJoin>
    {
        public SqlessJoinType SqlessJoinType { get; set; } = SqlessJoinType.InnerJoin;
        public string LeftTable { get; set; }
        public string LeftField { get; set; }
        public string RightTable { get; set; }
        public string RightField { get; set; }

        public SqlessJoin DeepClone()
        {
            return new SqlessJoin()
            {
                LeftField = LeftField,
                RightField = RightField,
                LeftTable = LeftTable,
                RightTable = RightTable,
                SqlessJoinType = SqlessJoinType
            };
        }

        public SqlessJoin ShallowClone()
        {
            return DeepClone();
        }
    }
}

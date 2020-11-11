using Hubery.Sqless.Request;
using System.Threading.Tasks;

namespace Hubery.Sqless.Access
{
    /// <summary>
    /// 登录认证
    /// </summary>
    public abstract class SqlessAccess
    {
        public string SqlConStr { get; set; }

        public string AccessTable { get; set; }
        public string UidField { get; set; }

        protected SqlessConfig SqlessConfig => SqlessConfig.GetAllowUnspecifiedConfig(SqlConStr);

        protected SqlessSelectRequest SelectRequestBase
        {
            get
            {
                var result = new SqlessSelectRequest
                {
                    Table = AccessTable
                };
                result.Fields.Add(new Query.SqlessField() { Field = UidField });
                return result;
            }
        }

        public abstract Task<string> GetUid(params string[] strs);
    }
}

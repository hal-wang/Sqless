using Sqless.Request;
using System.Threading.Tasks;

namespace Sqless.Access {
    /// <summary>
    /// 登录认证
    /// </summary>
    public abstract class SqlessAccess {
        public string SqlConStr { get; set; }

        public string AccessTable { get; set; }
        public string UidField { get; set; }

        protected SqlessConfig SqlessConfig => SqlessConfig.GetAllowUnspecifiedConfig(SqlConStr);

        protected virtual SqlessSelectRequest GetRequest(string[] strs) {
            var result = new SqlessSelectRequest {
                Table = AccessTable
            };
            result.Fields.Add(new Query.SqlessField() { Field = UidField });
            return result;
        }

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="strs">第一个参数为Token，第二个参数可选，比如根据Platform有不同Token</param>
        /// <returns></returns>
        public async Task<string> GetUid(params string[] strs) {
            var request = GetRequest(strs);

            using Sqless sqless = new Sqless(SqlessConfig);
            var uid = await sqless.SelectFirstOrDefault<string>(request);
            if (string.IsNullOrEmpty(uid)) {
                throw new SqlessUnauthorizedAccessException();
            }
            return uid;
        }
    }
}

using Hubery.Sqless.Access;
using Hubery.Sqless.Request;
using System.Threading.Tasks;

namespace Hubery.Sqless.Api
{
    public abstract class SqlessAccessController : SqlessBaseController
    {
        protected readonly SqlessAccess SqlessAccess;
        public SqlessAccessController(SqlessAccess sqlessAccess, SqlessConfig sqlessConfig)
            : base(sqlessConfig)
        {
            SqlessAccess = sqlessAccess;
        }

        protected async override Task<Sqless> GetSqless(SqlessRequest request)
        {
            var uid = await SqlessAccess.GetUid(request.AccessParams);
            SqlessConfig.AuthUid = uid;
            return new Sqless(SqlessConfig);
        }
    }
}

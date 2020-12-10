using Microsoft.AspNetCore.Mvc;
using Sqless;
using Sqless.Access;
using Sqless.Api;

namespace Demo.Sqless.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SqlessController : SqlessAccessController
    {
        public SqlessController(SqlessAccess sqlessAccess, SqlessConfig sqlessConfig)
            : base(sqlessAccess, sqlessConfig)
        {

        }
    }
}

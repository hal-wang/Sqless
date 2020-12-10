using Sqless.Access;
using Sqless.Api;
using Microsoft.AspNetCore.Mvc;

namespace Sqless.Demo.Api.Controllers
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

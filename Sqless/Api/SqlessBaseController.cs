using HTools;
using Microsoft.AspNetCore.Mvc;
using Sqless.Request;
using System.Linq;
using System.Threading.Tasks;

namespace Sqless.Api {
    public abstract class SqlessBaseController : ControllerBase {
        protected readonly SqlessConfig SqlessConfig;
        public SqlessBaseController(SqlessConfig sqlessConfig) {
            SqlessConfig = sqlessConfig;
        }

        protected virtual Task<Sqless> GetSqless(SqlessRequest request) {
            return Task.FromResult(new Sqless(SqlessConfig));
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> Select(SqlessSelectRequest request) {
            using Sqless sqless = await GetSqless(request);
            return Ok(await sqless.Select(request));
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> SelectFirst(SqlessSelectRequest request) {
            using Sqless sqless = await GetSqless(request);
            return Ok((await sqless.Select(request)).First());
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> SelectFirstOrDefault(SqlessSelectRequest request) {
            using Sqless sqless = await GetSqless(request);
            return Ok((await sqless.Select(request)).FirstOrDefault());
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> Insert(SqlessEditRequest request) {
            using Sqless sqless = await GetSqless(request);
            return Ok(await sqless.Insert(request));
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> Update(SqlessEditRequest request) {
            using Sqless sqless = await GetSqless(request);
            return Ok(await sqless.Update(request));
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> Upsert(SqlessEditRequest request) {
            using Sqless sqless = await GetSqless(request);
            return Ok(await sqless.Upsert(request));
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> Delete(SqlessDeleteRequest request) {
            using Sqless sqless = await GetSqless(request);
            return Ok(await sqless.Delete(request));
        }
    }

}

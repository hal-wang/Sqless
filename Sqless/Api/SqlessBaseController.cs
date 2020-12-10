using Sqless.Request;
using Hubery.Tools;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Sqless.Api
{
    public abstract class SqlessBaseController : ControllerBase
    {
        protected readonly SqlessConfig SqlessConfig;
        public SqlessBaseController(SqlessConfig sqlessConfig)
        {
            SqlessConfig = sqlessConfig;
        }

        protected virtual async Task<Sqless> GetSqless(SqlessRequest request)
        {
            await TaskExtend.SleepAsync(10);
            return new Sqless(SqlessConfig);
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> Select(SqlessSelectRequest request)
        {
            using Sqless sqless = await GetSqless(request);
            return Ok(await (await GetSqless(request)).Select(request));
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> SelectFirst(SqlessSelectRequest request)
        {
            using Sqless sqless = await GetSqless(request);
            return Ok((await (await GetSqless(request)).Select(request)).First());
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> SelectFirstOrDefault(SqlessSelectRequest request)
        {
            using Sqless sqless = await GetSqless(request);
            return Ok((await (await GetSqless(request)).Select(request)).FirstOrDefault());
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> Insert(SqlessEditRequest request)
        {
            using Sqless sqless = await GetSqless(request);
            return Ok(await sqless.Insert(request));
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> Update(SqlessEditRequest request)
        {
            using Sqless sqless = await GetSqless(request);
            return Ok(await sqless.Update(request));
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> Upsert(SqlessEditRequest request)
        {
            using Sqless sqless = await GetSqless(request);
            return Ok(await sqless.Upsert(request));
        }

        [HttpPost]
        [SqlessApiAction]
        public virtual async Task<ActionResult> Delete(SqlessDeleteRequest request)
        {
            using Sqless sqless = await GetSqless(request);
            return Ok(await sqless.Delete(request));
        }
    }

}

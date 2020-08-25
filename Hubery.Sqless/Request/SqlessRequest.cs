using Hubery.Sqless.Query;
using System.Collections.Generic;

namespace Hubery.Sqless.Request
{
    public abstract class SqlessRequest
    {
        public string Table { get; set; }

        public List<SqlessQuery> Queries { get; set; } = new List<SqlessQuery>();

        public string[] AccessParams { get; set; }
    }
}

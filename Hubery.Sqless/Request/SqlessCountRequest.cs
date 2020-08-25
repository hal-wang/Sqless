namespace Hubery.Sqless.Request
{
    public class SqlessCountRequest : SqlessRequest
    {
        public string Field { get; set; } = null;

        public bool Distinct { get; set; } = false;
    }
}

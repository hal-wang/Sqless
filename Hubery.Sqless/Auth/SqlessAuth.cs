namespace Hubery.Sqless.Auth
{
    public class SqlessAuth
    {
        public bool Writable { get; set; } = false;
        public bool Readable { get; set; } = false;
        public string Table { get; set; }
    }
}

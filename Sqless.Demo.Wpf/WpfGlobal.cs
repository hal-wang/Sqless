using Sqless.Demo.Common;

namespace Sqless.Demo.Wpf
{
    public class WpfGlobal
    {
        public static User LoginUser { get; set; } = null;

        public static string ApiUrl { get; } = "http://localhost:6688/api";
    }
}

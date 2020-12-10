using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Sqless.Common
{
    /// <summary>
    /// tables name
    /// </summary>
    public static class Tables
    {
        public static string User { get; } = nameof(User);
        public static string Order { get; } = nameof(Order);
        public static string Product { get; } = nameof(Product);
        public static string AccessToken { get; } = nameof(AccessToken);
    }
}

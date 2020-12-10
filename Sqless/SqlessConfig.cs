using Sqless.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Sqless
{
    /// <summary>
    /// sqless config
    /// </summary>
    public class SqlessConfig
    {
        public string SqlConStr { get; set; }

        public string AuthUid { get; set; }

        public bool IsUnspecifiedFieldReadable { get; set; } = true;
        public bool IsUnspecifiedFieldWritable { get; set; } = true;

        public bool IsUnspecifiedFreeReadable { get; set; } = false;
        public bool IsUnspecifiedFreeWritable { get; set; } = false;

        public List<SqlessFieldAuth> FieldAuths { get; set; } = new List<SqlessFieldAuth>();

        /// <summary>
        /// the field that shows the row belong to
        /// </summary>
        public List<SqlessFieldAuth> OwnerAuths { get; set; } = new List<SqlessFieldAuth>();

        public List<SqlessAuth> FreeAuths { get; set; } = new List<SqlessAuth>();

        internal void CheckConfigValid()
        {
            if (string.IsNullOrEmpty(SqlConStr))
            {
                throw new ArgumentNullException(nameof(SqlConStr));
            }

            if (string.IsNullOrEmpty(AuthUid) && OwnerAuths.Count > 0)
            {
                throw new ConfigurationErrorsException("If there is owner auth, AuthUid should be specified");
            }

            if (OwnerAuths.Where(oa => FreeAuths.Where(fa => string.Equals(fa.Table, oa.Table, StringComparison.InvariantCultureIgnoreCase)).Count() > 0).Count() > 0 ||
                FreeAuths.Where(fa => OwnerAuths.Where(oa => string.Equals(oa.Table, fa.Table, StringComparison.InvariantCultureIgnoreCase)).Count() > 0).Count() > 0)
            {
                throw new ConfigurationErrorsException("the table can't be in both FreeAuth and OwnerAuth");
            }

            if (FieldAuths.Where(fa => string.IsNullOrEmpty(fa.Table)).Count() > 0 ||
                FieldAuths.Where(fa => string.IsNullOrEmpty(fa.Field)).Count() > 0)
            {
                throw new ArgumentNullException("tables and fields in FieldAuths config can't be null or empty");
            }

            if (OwnerAuths.Where(fa => string.IsNullOrEmpty(fa.Table)).Count() > 0 ||
                OwnerAuths.Where(fa => string.IsNullOrEmpty(fa.Field)).Count() > 0)
            {
                throw new ArgumentNullException("tables and fields in OwnerAuths config can't be null or empty");
            }

            if (FreeAuths.Where(fa => string.IsNullOrEmpty(fa.Table)).Count() > 0)
            {
                throw new ArgumentNullException("tables in FreeAuths config can't be null or empty");
            }
        }

        public static SqlessConfig GetAllowUnspecifiedConfig(string sqlConStr) => new SqlessConfig()
        {
            SqlConStr = sqlConStr,

            IsUnspecifiedFieldReadable = true,
            IsUnspecifiedFieldWritable = true,
            IsUnspecifiedFreeReadable = true,
            IsUnspecifiedFreeWritable = true,
        };

        public static SqlessConfig GetDisallowedUnspecifiedConfig(string sqlConStr) => new SqlessConfig()
        {
            SqlConStr = sqlConStr,

            IsUnspecifiedFieldWritable = false,
            IsUnspecifiedFieldReadable = false,
            IsUnspecifiedFreeReadable = false,
            IsUnspecifiedFreeWritable = false,
        };

        public static SqlessConfig GetOwnerConfig(string sqlConStr, string authUid) => new SqlessConfig()
        {
            SqlConStr = sqlConStr,
            AuthUid = authUid,

            IsUnspecifiedFieldWritable = true,
            IsUnspecifiedFieldReadable = true,
            IsUnspecifiedFreeReadable = true,
            IsUnspecifiedFreeWritable = false,
        };
    }
}

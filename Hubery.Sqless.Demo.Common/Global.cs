using Hubery.Sqless.Access;
using Hubery.Sqless.Auth;
using System;

namespace Hubery.Sqless.Demo.Common
{
    public class Global
    {
        public static string SqlConStr
        {
            get
            {
                var result = Environment.GetEnvironmentVariable("SqlessTestSqlConStr", EnvironmentVariableTarget.User);
                if (string.IsNullOrEmpty(result))
                {
                    throw new Exception("Add an environment variable for the database connection string or change this property and return");
                }
                return result;
            }
        }

        public static SqlessConfig AllowUnspecifiedConfig { get; } = SqlessConfig.GetAllowUnspecifiedConfig(SqlConStr);
        public static SqlessConfig DisallowedUnspecifiedConfig { get; } = SqlessConfig.GetDisallowedUnspecifiedConfig(SqlConStr);

        public static SqlessConfig GetOwnerAccessConfig(string authUid = null)
        {
            var result = SqlessConfig.GetOwnerConfig(SqlConStr, authUid);
            result.AuthUid = authUid;

            result.OwnerAuths.Add(new SqlessFieldAuth()
            {
                Readable = true,
                Writable = true,
                Table = Tables.User,
                Field = "Uid"
            });

            result.OwnerAuths.Add(new SqlessFieldAuth()
            {
                Readable = true,
                Writable = true,
                Table = Tables.Order,
                Field = "Uid"
            });

            result.FieldAuths.Add(new SqlessFieldAuth()
            {
                Readable = false,
                Writable = false,
                Table = Tables.User,
                Field = "Password"
            });

            result.FreeAuths.Add(new SqlessAuth()
            {
                Readable = true,
                Writable = false,
                Table = Tables.Product
            });

            return result;
        }


        public static SqlessAccess PasswordAccessConfig => new SqlessAccessPassword()
        {
            UidField = "Uid",
            SqlConStr = SqlConStr,
            AccessTable = Tables.User,
            AccessAccountField = "Uid",
            AccessPasswordField = "Password"
        };

        public static SqlessAccess TokenAccessConfig => new SqlessAccessToken()
        {
            UidField = "Uid",
            SqlConStr = SqlConStr,
            AccessTable = Tables.AccessToken,
            AccessTokenField = "Token",
            AccessTokenTypeField = "Platform"
        };
    }
}

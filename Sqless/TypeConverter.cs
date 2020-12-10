using System;
using System.Data;
using System.Text.Json;

namespace Sqless
{
    public static class TypeConverter
    {
        public static DbType TypeToDbType(System.Type type)
        {
            if (Enum.TryParse(type.Name, out DbType dbType))
            {
                return dbType;
            }
            else
            {
                return DbType.String;
            }

            //System.Data.SqlTypes.SqlString.
        }

        public static System.Type DbTypeToType(DbType dbType) => dbType switch
        {
            DbType.UInt64 => typeof(ulong),
            DbType.Int64 => typeof(long),
            DbType.Int32 => typeof(int),
            DbType.UInt32 => typeof(uint),
            DbType.Single => typeof(float),
            DbType.Date => typeof(DateTime),
            DbType.DateTime => typeof(DateTime),
            DbType.Time => typeof(DateTime),
            DbType.String => typeof(string),
            DbType.StringFixedLength => typeof(string),
            DbType.AnsiString => typeof(string),
            DbType.AnsiStringFixedLength => typeof(string),
            DbType.UInt16 => typeof(ushort),
            DbType.Int16 => typeof(short),
            DbType.SByte => typeof(byte),
            DbType.Object => typeof(object),
            DbType.VarNumeric => typeof(decimal),
            DbType.Decimal => typeof(decimal),

            DbType.Currency => typeof(double),
            DbType.Binary => typeof(byte[]),
            DbType.Double => typeof(double),
            DbType.Guid => typeof(Guid),
            DbType.Boolean => typeof(bool),

            _ => typeof(string)
        };

        public static object GetValue(this JsonElement jsonElement, Type targetType)
        {
            if (targetType == typeof(int))
            {
                return jsonElement.GetInt32();
            }
            if (targetType == typeof(short))
            {
                return jsonElement.GetInt16();
            }
            if (targetType == typeof(long))
            {
                return jsonElement.GetInt64();
            }
            if (targetType == typeof(uint))
            {
                return jsonElement.GetUInt32();
            }
            if (targetType == typeof(ushort))
            {
                return jsonElement.GetUInt32();
            }
            if (targetType == typeof(ulong))
            {
                return jsonElement.GetUInt64();
            }
            if (targetType == typeof(string))
            {
                return jsonElement.GetString();
            }
            if (targetType == typeof(Guid))
            {
                return jsonElement.GetGuid();
            }
            if (targetType == typeof(double))
            {
                return jsonElement.GetDouble();
            }
            if (targetType == typeof(float))
            {
                return jsonElement.GetSingle();
            }
            if (targetType == typeof(DateTime))
            {
                return jsonElement.GetDateTime();
            }
            if (targetType == typeof(DateTimeOffset))
            {
                return jsonElement.GetDateTimeOffset();
            }
            if (targetType == typeof(bool))
            {
                return jsonElement.GetBoolean();
            }
            if (targetType == typeof(byte))
            {
                return jsonElement.GetByte();
            }
            if (targetType == typeof(decimal))
            {
                return jsonElement.GetDecimal();
            }
            if (targetType == typeof(sbyte))
            {
                return jsonElement.GetSByte();
            }

            return jsonElement.GetString();
        }
    }
}

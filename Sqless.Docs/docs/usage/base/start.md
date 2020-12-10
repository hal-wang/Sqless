---
title: 开始
---

# 开始

## 安装

先安装 Nuget 包。在包管理器中运行以下语句，或搜索`Sqless`并安装到项目。

```Shell
Install-Package Sqless
```

## 使用示例

通过账号查询用户 Id

```CSharp
var account = "123456";
var sqlConStr = "Data Source=127.0.0.1;Initial Catalog=StoreTest;User ID=sa;Password=123";

var request = new SqlessSelectRequest
{
    Table = "User"
};
request.Fields.Add(new Query.SqlessField() { Field = "Uid" });
request.Queries.Add(new Query.SqlessQuery()
{
    Field = "Account",
    Type = Query.SqlessQueryType.Equal,
    Value = account
});

var sqlessConfig = SqlessConfig.GetAllowUnspecifiedConfig(sqlConStr);
using Sqless sqless = new Sqless(sqlessConfig);
return await sqless.SelectFirst<string>(request);
```

## Sqless 配置

与 `Sqless` 配置相关的内容，基本都在 `SqlessConfig` 中。

若要创建 `Sqless` 对象，构造函数需传入 `SqlessConfig` 对象。

### 配置示例 1

未指定的表、字段有独写权限

```CSharp
public static SqlessConfig GetAllowUnspecifiedConfig(string sqlConStr) => new SqlessConfig()
{
    SqlConStr = sqlConStr,

    IsUnspecifiedFieldReadable = true,
    IsUnspecifiedFieldWritable = true,
    IsUnspecifiedFreeReadable = true,
    IsUnspecifiedFreeWritable = true,
};
```

### 配置示例 2

未指定的表、字段无读写权限

```CSharp
public static SqlessConfig GetDisallowedUnspecifiedConfig(string sqlConStr) => new SqlessConfig()
{
    SqlConStr = sqlConStr,

    IsUnspecifiedFieldWritable = false,
    IsUnspecifiedFieldReadable = false,
    IsUnspecifiedFreeReadable = false,
    IsUnspecifiedFreeWritable = false,
};
```

## 创建 Sqless 对象

```CSharp
var sqlessConfig = SqlessConfig.GetAllowUnspecifiedConfig(SqlConStr);
using var sqless = new Sqless(sqlessConfig);
```

## SqlessRequest

创建`Sqless`对象后，通过调用函数并传参 `SqlessRequest`的派生类，对表进行操作。

### 示例

```CSharp
var account = "123456";

var request = new SqlessSelectRequest
{
    Table = "User"
};
request.Fields.Add(new Query.SqlessField() { Field = "Uid" });
request.Queries.Add(new Query.SqlessQuery()
{
    Field = "Account",
    Type = Query.SqlessQueryType.Equal,
    Value = account
});
```

### 基类

基类包含以下字段：

1. `Table`：要操作的表
2. `Queries`：查询条件
3. `AccessParams`：身份认证信息

### 派生类

如`SqlessSelectRequest`,`SqlessDeleteRequest`等，分别对应数据库操作方式。

后续用法有详细介绍。

## 查询条件

大部分数据库操作都需要查询条件，查询条件在 `SqlessRequest`对象的`Queries`中设置。`Queries`是`SqlessQuery`列表，`SqlessQuery`设置查询条件，每个 `SqlessQuery` 对象对应每个查询条件。

### 查询条件示例

有个查询条件为 **User 表中 Account 字段值等于 account**。

```CSharp
var request = new SqlessSelectRequest
{
    Table = "User"
};
request.Queries.Add(new Query.SqlessQuery()
{
    Field = "Account",
    Type = Query.SqlessQueryType.Equal,
    Value = account
});
```

该查询条件翻译成 SQL 部分类似以下语句：

```SQL
WHERE [Account] = @account
```

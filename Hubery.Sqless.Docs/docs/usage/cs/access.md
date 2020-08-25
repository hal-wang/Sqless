---
title: 身份认证
---

# 身份认证

身份认证主要用于 _客户端/服务器_ 架构，客户端通过简单的 API 来安全的操作数据库。

身份认证支持两种方式：

1. Account / Password
2. Token

## Account / Password 方式

### 客户端使用

在`SqlessRequest`对象中，设置 `AccessParams` 属性

```CSharp
var uid = "uid123";
var password = "Wpf";

var request = new SqlessSelectRequest()
{
    AccessParams = new string[] { uid, password },
    Table = 'User'
};
// ...
```

### Api 配置

在`SqlessAccess`对象中设置以下字段：

| 字段                | 说明                    |
| ------------------- | ----------------------- |
| AccessTable         | 用户信息表名            |
| UidField            | 用户信息表中的 Uid 字段 |
| AccessAccountField  | 用户信息表账号字段      |
| AccessPasswordField | 用户信息表密码字段      |
| SqlConStr           | 数据库连接字符串        |

```CSharp
public static SqlessAccess PasswordAccessConfig => new SqlessAccessPassword()
{
    UidField = "Uid",
    SqlConStr = SqlConStr,
    AccessTable = Tables.User,
    AccessAccountField = "Uid",
    AccessPasswordField = "Password"
};
```

### 在 Api 使用

使用配置创建的 SqlessAccess 对象获取 Uid

```CSharp
var uid = await PasswordAccessConfig.GetUid("AccessTestUid", "123456");
```

配合客户端传来的 `SqlessRequest`

```CSharp
private SqlessConfig GetOwnerAccessConfig(string authUid)
{
    var result = SqlessConfig.GetOwnerConfig(SqlConStr, authUid);
    result.AuthUid = authUid;

    // 其他配置 ...

    return result;
}

// 根据 SqlessRequest 创建 Sqless 实例
private async Task<Sqless> GetSqless(SqlessRequest request)
{
    var uid = await PasswordAccessConfig.GetUid(request.AccessParams);
    return new Sqless(GetOwnerAccessConfig(uid));
}
```

## Token 方式

### 客户端使用

在`SqlessRequest`对象中，设置 `AccessParams` 属性

```CSharp
var token = "token123";

var request = new SqlessSelectRequest()
{
    AccessParams = new string[] { token },
    Table = 'User'
};
// ...
```

也可根据不同平台，获取不同 Token

```CSharp
var token = "token123";
var platform = "Wpf";

var request = new SqlessSelectRequest()
{
    AccessParams = new string[] { token, platform },
    Table = 'User'
};
// ...
```

### Api 配置

在`SqlessAccess`对象中设置以下字段：

| 字段                 | 说明                                  |
| -------------------- | ------------------------------------- |
| AccessTable          | Token 表名                            |
| UidField             | Token 表中的 Uid 字段                 |
| AccessTokenField     | Token 表 Token 字段                   |
| AccessTokenTypeField | Token 表 Token 类型字段，如*平台*标识 |
| SqlConStr            | 数据库连接字符串                      |

```CSharp
public static SqlessAccess TokenAccessConfig => new SqlessAccessToken()
{
    UidField = "Uid",
    SqlConStr = SqlConStr,
    AccessTable = Tables.User,
    AccessTokenField = "Uid",
    AccessTokenTypeField = "Password"
};
```

### 在 Api 使用

使用配置创建的 SqlessAccess 对象获取 Uid

```CSharp
var uid = await TokenAccessConfig.GetUid("AccessTestToken");
```

也可根据不同平台来获取

```CSharp
var uid = await TokenAccessConfig.GetUid("AccessTestToken","Wpf");
```

配合客户端传来的`SqlessRequest`

```CSharp
private SqlessConfig GetOwnerAccessConfig(string authUid)
{
    var result = SqlessConfig.GetOwnerConfig(SqlConStr, authUid);
    result.AuthUid = authUid;

    // 其他配置 ...

    return result;
}

// 根据 SqlessRequest 创建 Sqless 实例
private async Task<Sqless> GetSqless(SqlessRequest request)
{
    var uid = await TokenAccessConfig.GetUid(request.AccessParams);
    return new Sqless(GetOwnerAccessConfig(uid));
}
```

## 身份认证前提

前面所说的两种方式，都是建立在已有用户信息或 Token 的前提下。换言之，Sqless 的权限系统，需相关信息已存在于数据库中。

因此。用户管理、Token 管理这些敏感操作，需要单独写 Api 来实现。

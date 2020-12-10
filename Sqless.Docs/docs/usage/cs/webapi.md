---
title: WebApi 配置
---

# WebApi 配置

## Api 依赖注入

在 asp.net core api 项目中添加依赖注入，配置数据库读写权限。
如果需要身份认证，也要配置身份认证信息。

在 Startup.cs 的 ConfigureServices 函数中配置依赖注入。

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    //...
}
```

### Sqless 配置

```CSharp
services.AddTransient<SqlessConfig, SqlessConfig>((ctx) =>
{
    return new SqlessConfig()
    {
        SqlConStr="...",
        // more ...
    };
});
```

在`SqlessConfig`对象中，设置相应属性的内容。权限相关请参考后面详细介绍的 _权限配置_

### 身份认证

身份认证功能，能根据账号密码或 Token，获取用户的 Id，后续才能使用权限功能中的 _所有者表_ 。

#### Account/Password 方式

```CSharp
services.AddTransient<SqlessAccess, SqlessAccessPassword>((ctx) =>
{
    return new SqlessAccessPassword();
});
```

#### Token 方式

```CSharp
services.AddTransient<SqlessAccess, SqlessAccessToken>((ctx) =>
{
    return new SqlessAccessToken();
});
```

#### 示例

```CSharp
// Account/Password
public static SqlessAccess PasswordAccessConfig => new SqlessAccessPassword()
{
    UidField = "Uid",
    SqlConStr = SqlConStr,
    AccessTable = Tables.User,
    AccessAccountField = "Uid",
    AccessPasswordField = "Password"
};

// Token
public static SqlessAccess TokenAccessConfig => new SqlessAccessToken()
{
    UidField = "Uid",
    SqlConStr = SqlConStr,
    AccessTable = Tables.AccessToken,
    AccessTokenField = "Token",
    AccessTokenTypeField = "Platform"
};
```

身份认证详细配置，参考后面详细介绍的 _身份认证_

## Controller

Sqless 带有的基类 Controller 有两种

1. `SqlessBaseController`
2. `SqlessAccessController`

### SqlessBaseController

包含基本 Controller，有增删查改等接口，不包含身份认证功能。

包含虚函数 `GetSqless`，可获取 Sqless 对象。

在依赖注入中，只需要有 Sqless 配置，构造函数也只支持 `SqlessConfig`依赖注入。

```CSharp
protected readonly SqlessConfig SqlessConfig;
public SqlessBaseController(SqlessConfig sqlessConfig)
{
    SqlessConfig = sqlessConfig;
}
```

### SqlessAccessController

包含身份认证的 Controller，从`SqlessBaseController`派生。

使用此 Controller 或从此 Controller 派生，在依赖注入中必须有 _身份认证_。在构造函数支持`SqlessAccess`和`SqlessConfig`

```CSharp
protected readonly SqlessAccess SqlessAccess;
public SqlessAccessController(SqlessAccess sqlessAccess, SqlessConfig sqlessConfig)
    : base(sqlessConfig)
{
    SqlessAccess = sqlessAccess;
}
```

重写的`GetSqless`函数可根据身份认证功能获取到用户 Id

```CSharp
protected async override Task<Sqless> GetSqless(SqlessRequest request)
{
    var uid = await SqlessAccess.GetUid(request.AccessParams);
    SqlessConfig.AuthUid = uid;
    return new Sqless(SqlessConfig);
}
```

### 扩展

简单项目，创建 Controller 并从 `SqlessBaseController`或`SqlessAccessController`派生即可使用，因为已内置如`Select`,`SelectFirst`,`Update`等接口。

如果需求复杂，可在 Controller 的派生类中，使用`GetSqless`函数获取 Sqless 对象，并实现复杂功能。如内置的`SelectFirstOrDefault`

```CSharp
[HttpPost]
public virtual async Task<ActionResult> SelectFirstOrDefault(SqlessSelectRequest request)
{
    using Sqless sqless = await GetSqless(request);
    return Ok((await (await GetSqless(request)).Select(request)).FirstOrDefault());
}
```

如果需求更复杂，可完全重写 Controller，而不从`SqlessBaseController`或`SqlessAccessController`派生。如重置用户密码、更改订单付款信息等敏感操作。

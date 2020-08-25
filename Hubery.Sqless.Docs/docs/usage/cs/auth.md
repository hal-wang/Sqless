---
title: 权限配置
---

# 权限配置

Sqless 对数据库的权限控制分为三种

1. 字段读写权限
2. 记录所有者读写权限
3. 其他表读写权限

在 WebApi 中，配置好权限，就无需担心客户端越权操作数据

## 配置

权限在 SqlessConfig 类中配置，新建 SqlessConfig 对象并设置相关属性，在将 SqlessConfig 对象传给 Sqless 构造函数。

```CSharp
var sqlessConfig= new SqlessConfig()
{
    SqlConStr = sqlConStr, // 数据库连接字符串

    IsUnspecifiedFieldReadable = true,
    IsUnspecifiedFieldWritable = true,
};
using Sqless sqless = new Sqless(sqlessConfig);
// ...
```

以下权限相关的配置，都是指 SqlessConfig 对象相应的字段

## 字段读写权限

可详细控制表的某些字段读写权限。

- IsUnspecifiedFieldReadable：未指定的字段是否可读
- IsUnspecifiedFieldWritable：未指定的字段是否可写
- FieldAuths：指定字段是否可读写的列表

```CSharp
var sqlessConfig= new SqlessConfig()
{
    SqlConStr = sqlConStr, // 数据库连接字符串

    IsUnspecifiedFieldReadable = false,
    IsUnspecifiedFieldWritable = false
};

sqlessConfig.FieldAuths = new System.Collections.Generic.List<Auth.SqlessFieldAuth>()
{
    new Auth.SqlessFieldAuth(){Table=Tables.Product, Field="Name",Readable=true},
    new Auth.SqlessFieldAuth(){Table=Tables.Product, Field="Price",Readable=true}
};
```

在上述示例中，`Product`表的`Name`和`Price`字段被指定可读，但未指定是否可写。由于`IsUnspecifiedFieldWritable`为`false`，因此`Name`和`Price`字段不可写。

## 记录所有者读写权限

即某条记录的所有者，对该条记录是否可读写。如用户的账号信息，用户仅有“修改已登录账号的信息”的权限，但无法修改其他人的账号信息。若要使用此权限，需设置`AuthUid`，在 _客户端/服务器_ 架构中，可使用后面介绍的*身份认证*来根据 `Account/Password`或`Token`来获取 AuthUid。

::: tip
用户信息某些字段，如 Password 等应该通过“字段读写权限”禁止用户读写。Email,Phone 等字段应设置为只读
:::

- OwnerAuths：指定所有者表是否可读写的列表

示例配置如下（假设 sqlConStr 和 authUid 已知）：

```CSharp
var config = SqlessConfig.GetOwnerConfig(sqlConStr, authUid);
config.AuthUid = authUid;

config.OwnerAuths.Add(new SqlessFieldAuth()
{
    Readable = true,
    Writable = true,
    Table = Tables.User,
    Field = "Uid"
});

result.FieldAuths.Add(new SqlessFieldAuth()
{
    Readable = false,
    Writable = false,
    Table = Tables.User,
    Field = "Password"
});

config.OwnerAuths.Add(new SqlessFieldAuth()
{
    Readable = true,
    Writable = true,
    Table = Tables.Order,
    Field = "Uid"
});
```

在上述示例中，`User`表和`Order`表，用户可读写所有者的记录，并且禁止读写`User`表的`Password`字段。

对于`SqlessFieldAuth`对象，在`OwnerAuths`中，`Field`属性为所有者的 Id 字段。在此示例中，`User`表和`Order`表均使用`Uid`存储记录的所有者（数据库设计中，`Uid`字段应该为外键）

## 其他表读写权限

除“所有者”的表外，其他表应与创建者无关。如商品表等，此类表一般只读。

- IsUnspecifiedFreeReadable：未指定的其他表是否可读
- IsUnspecifiedFreeWritable：未指定的其他表是否可写
- FreeAuths：指定其他表是否可读写的列表

```CSharp
var sqlessConfig= new SqlessConfig()
{
    SqlConStr = sqlConStr, // 数据库连接字符串

    IsUnspecifiedFreeReadable = true,
    IsUnspecifiedFreeWritable = false
};

result.FreeAuths.Add(new SqlessAuth()
{
    Readable = false,
    Writable = false,
    Table = Tables.Product
});
```

在上述示例中，`Product`表指定为不可读写。由于设置了`IsUnspecifiedFreeReadable`为`true`,`IsUnspecifiedFreeWritable`为`false`，因此其他未指定的表都是只读。

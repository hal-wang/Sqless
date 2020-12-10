---
title: Upsert
---

# Upsert

根据查询条件，如果查到数据，则更新该数据内容。如果没有查到数据，就插入新数据。

## 示例

```CSharp
var newUid = "UpsertTest_" + Guid.NewGuid().ToString();
var request = new SqlessEditRequest()
{
    Table = Tables.User,
    Queries = new List<Query.SqlessQuery>()
    {
        new Query.SqlessQuery()
        {
            Field="Uid",
            Value=newUid,
            Type=Query.SqlessQueryType.Equal
        }
    }
};
request.LoadFromObject(new
{
    Uid = newUid,
    Name = DateTime.Now.ToString("hhmmss"),
    Password = new Random().Next(100000, 999999).ToString()
});

using Sqless sqless = new Sqless(Global.AllowUnspecifiedConfig);
var result = await sqless.Upsert(request);
```

::: tip
根据 Insert，如果插入的表已设置为 _所有者_ 表，则所有者字段（如`Uid`）自动设置为当前已登录账号的 用户 Id。

该规则在 Upsert 仍然适用。
:::

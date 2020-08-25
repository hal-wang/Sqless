---
title: Update
---

# Update

更新已存在的数据

## 示例

```CSharp
var request = new SqlessEditRequest()
{
    Table = Tables.User,
    Queries = new List<Query.SqlessQuery>()
    {
        new Query.SqlessQuery()
        {
            Field="Uid",
            Value="1",
            Type=Query.SqlessQueryType.Equal
        }
    }
};
request.LoadFromObject(new
{
    Phone = DateTime.Now.ToString("hhmmss"),
    Password = new Random().Next(100000, 999999).ToString()
});
await sqless.Update(request);
```

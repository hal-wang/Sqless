---
title: Delete
---

# Delete

删除满足条件的数据

## 示例

```CSharp
var request = new SqlessDeleteRequest()
{
    Table = Tables.User,
    Queries = new List<Query.SqlessQuery>()
    {
        new Query.SqlessQuery()
        {
            Field="Uid",
            Type=Query.SqlessQueryType.Equal,
            Value="1"
        }
    }
};
await sqless.Delete(request);
```

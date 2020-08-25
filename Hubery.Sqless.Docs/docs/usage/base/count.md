---
title: Count
---

# Count

查询满足条件的数据条数

## 基本用法示例

```CSharp
var request = new SqlessCountRequest()
{
    Table = Tables.User
};
int count = await sqless.Count(request);
```

## Distinct

不相同的记录条数。在下面示例中，查询结果为 Name 不同的记录条数

### 示例

```CSharp
var request = new SqlessCountRequest()
{
    Table = Tables.User,
    Distinct = true,
    Field = "Name"
};
int count = await sqless.Count(request);
```

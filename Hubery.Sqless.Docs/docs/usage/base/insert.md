---
title: Insert
---

# Insert

插入新数据。

::: tip
如果插入的表已设置为 _所有者_ 表，则所有者字段（如`Uid`）自动设置为当前已登录账号的 用户 Id
:::

## 示例

```CSharp
var request = new SqlessEditRequest()
{
    Table = Tables.User
};
request.LoadFromObject(new User
{
    Uid = "InsertTest_" + Guid.NewGuid().ToString("D"),
    Name = DateTime.Now.ToString("hhmmss"),
    Password = new Random().Next(100000, 999999).ToString()
});
await sqless.Insert(request);
```

---
title: Client 使用
---

# Client 使用

在客户端中，用 Post 调用 Api，传参 `SqlessRequest`或其派生类即可。

如 Select 接口，传参`SqlessSelectRequest`。

## SqlessRequest

如 `SqlessSelectRequest`，在 C#中设置示例：

```CSharp
var request = new SqlessSelectRequest()
{
    AccessParams = new string[] { Uid, Password },
    Table = Tables.Product
};
request.LoadFromType(typeof(Product)); // 根据对象类型设置字段
```

在其他语言如 js 中，对象结构与 C#中的相同即可

## SqlessClient

在 C#的客户端中，可使用已封装的 API 调用类 `SqlessClient`。使用示例：

```CSharp
List<Product> products = await SqlessClient.Select<Product>(request);
```

该示例是获取 `Product` 对象列表。

在其他语言中，暂未封装调用 API 的方法，与调用其他 API 方式类似。

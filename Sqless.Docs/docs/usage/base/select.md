---
title: Select
---

# Select

Sqless 支持以下几种 Select：

| 功能                 | 说明                                       | 用法                                  |
| -------------------- | ------------------------------------------ | ------------------------------------- |
| Select 特定列表      | 使用泛型，获取列表                         | `Select<T>(SqlessSelectRequest)`      |
| Select 普通列表      | 使用 `dynamic`                             | `Select(SqlessSelectRequest)`         |
| SelectFirst          | 选择第一条，使用泛型，如果未找到就报错     | `SelectFirst<T>(SqlessSelectRequest)` |
| SelectFirstOrDefault | 选择第一条，使用泛型，如果未找到返回默认值 | `SelectFirst<T>(SqlessSelectRequest)` |

后续`Select`示例中，`sqless`对象为

```CSharp
var sqlConStr = "Data Source=127.0.0.1;Initial Catalog=StoreTest;User ID=sa;Password=123";
var sqlessConfig = SqlessConfig.GetAllowUnspecifiedConfig(sqlConStr);
using Sqless sqless = new Sqless(sqlessConfig);
```

## SqlessRequest

Select 使用`SqlessRequest`的派生类`SqlessSelectRequest`

- Orders 属性： 排序，可设置多个排序条件
- Fields 属性： 查询字段，可利用`LoadFromType`函数快速设置
- Joins 属性： 表连接，可连接多个表
- PageIndex 属性： 分页查询起始页，0 基
- PageSize 属性： 分页查询每页数量
- LoadFromType 函数： 将类型的字段添加到 Fields 列表中

## Select 特定列表

### 示例

```CSharp
var request = new SqlessSelectRequest()
{
    Table = Tables.Product
};
request.LoadFromType(typeof(Product));
List<Product> products =  await sqless.Select<Product>(request);
```

## Select 普通列表

与 _Select 特定列表_ 类似

### 示例

```CSharp
var request = new SqlessSelectRequest()
{
    Table = Tables.Product
};
request.LoadFromType(typeof(Product));
List<dynamic> products =  await sqless.Select(request);
```

## SelectFirst

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

string uid = await sqless.SelectFirst<string>(request);
```

## SelectFirstOrDefault

与`SelectFirst`用法相同，至是如果未找到会返回默认值

### 示例

```CSharp
string uid = await sqless.SelectFirstOrDefault<string>(request);
```

## 分页查询

::: tip
至少存在一个 Orders，分页查询才生效。
:::

### 示例

查询结果为第 2 页（`PageIndex`是 0 基的）， 2 条数据。即查询结果为第三条和第四条数据。

```CSharp
var request=new SqlessSelectRequest()
{
    Table = Tables.User,
    PageSize = 2,
    PageIndex = 1,
    Orders = new List<SqlessOrder>()
    {
        new SqlessOrder() { Field="Uid" }
    },
    Fields = new List<SqlessField>()
    {
        new SqlessField() {  Field="Uid"  },
        new SqlessField() {  Field="Name"  }
    }
};
```

## Join

Sqless 支持以下几种 Join：

1. LeftJoin：与 SQL 中的 LEFT JOIN 对应
2. RightJoin：与 SQL 中的 RIGHT JOIN 对应
3. InnerJoin：与 SQL 中的 INNER JOIN 对应
4. FullJoin：与 SQL 中的 FULL JOIN 对应

::: tip
连接查询时，如果两个表中的字段名相同，只能取其中一个字段
:::

### 示例

`User`表左连接`Order`表，查询条件为 Phone 字段不为空，查询结果取`User`表的`Name`、`Order`表的`Time`2 个字段。

```CSharp
var request = new SqlessSelectRequest()
{
    Table = Tables.User,
    Queries = new List<SqlessQuery>()
    {
        new SqlessQuery()
        {
            Field="Phone",
            Table="User",
            Type=SqlessQueryType.NotNull
        }
    },
    Fields = new List<SqlessField>()
    {
        new SqlessField() {  Table=Tables.User, Field="Name"  },
        new SqlessField() {  Table=Tables.Order, Field="Time"  }
    },
    Joins = new List<SqlessJoin>()
    {
        new SqlessJoin()
        {
            LeftTable=Tables.User,
            LeftField="Uid",
            RightTable=Tables.Order,
            RightField="Uid"
        }
    }
};
```

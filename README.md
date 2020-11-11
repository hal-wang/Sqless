_详细介绍请查看文档： [https://sqless.hubery.wang](https://sqless.hubery.wang)_

**一个客户端安全操作数据库的解决方案**

- 高效： Sqless 使客户端能够安全地操作数据库，而不必在编写客户端同时编写 API。全栈程序员福利！
- 安全：严格的权限控制允许用户读写特定内容，而不必担心数据库安全性。
- 简单：无需 SQL 语句，配置简单。CS 架构只需要简单的配置 WebAPI 。

### 示例

_SELECT_

```CSharp
var request = new SqlessSelectRequest()
{
    Table = "User",
    Fields = new List<SqlessField>()
    {
        new SqlessField() { Field = "Uid" },
        new SqlessField() { Field = "Name" }
    }
};

var conStr = "Data Source=.;Initial Catalog=StoreTest;User ID=sa;Password=123456";
using Sqless sqless = new Sqless(SqlessConfig.GetAllowUnspecifiedConfig(conStr));
List<User> users = await sqless.Select<User>(request);
```

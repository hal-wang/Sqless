---
home: true
heroText: Sqless
tagline: 一个客户端安全操作数据库的解决方案
actionText: 开始  →
actionLink: /usage/base/start
features:
  - title: 高效
    details: Sqless 使客户端能够安全地操作数据库，而不必在编写客户端同时编写API。全栈程序员福利！
  - title: 安全
    details: 严格的权限控制允许用户读写特定内容，而不必担心数据库安全性。
  - title: 简单
    details: 无需SQL语句，配置简单。CS 架构只需要简单的配置 WebAPI 。
---

### 像 1，2，3 一样容易

在 nuget 包管理器运行以下命令，或在 nuget 搜索 `Sqless` 并安装

```Shell
Install-Package Sqless
```

```CSharp
// SELECT

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

---
title: 单元测试
---

如果后续介绍说的不清晰，看看源码中的单元测试项目`Hubery.Sqless.Test`可能会有所帮助。

## 下载测试数据库

下载并还原数据库备份文件：
<ClientOnly>
<DbDownload />
</ClientOnly>

## 环境变量

配置环境变量`SqlessTestSqlConStr`值为 sql server 连接字符串，如

```
Data Source=.;Initial Catalog=StoreTest;User ID=sa;Password=123456
```

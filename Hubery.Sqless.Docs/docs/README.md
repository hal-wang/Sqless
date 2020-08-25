---
home: true
heroImage: /logo.png
heroText: Sqless
tagline: A solution for client manipulation of a database
actionText: Get Started  →
actionLink: /usage/base/start
features:
  - title: Efficient
    details: Sqless enables the client to operate the database safely without having to write the API while writing the client.Full stack programmer benefits!
  - title: Safety
    details: Detailed permission controls allow users to read and write specific content without having to worry about database security even when operating on the client side.
  - title: Ease
    details: No SQL statement, simple configuration to use. The Client/Server architecture requires only simple configuration of WebAPI.
---

### As Easy as 1, 2, 3

Run the following statement in the package manager, or search for 'Sqless' and install

```Shell
Install-Package Sqless
```

```C#
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

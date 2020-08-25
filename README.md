**For more, see the documentation [https://sqless.hubery.wang](https://sqless.hubery.wang)**

- Efficient: Sqless enables the client to operate the database safely without having to write the API while writing the client.Full stack programmer benefits!
- Safety: Detailed permission controls allow users to read and write specific content without having to worry about database security even when operating on the client side.
- Ease: No SQL statement, simple configuration to use. The Client/Server architecture requires only simple configuration of WebAPI.

### Example

_SELECT_

```C#

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

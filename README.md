# Introduction
*Last updated: 3/6/2024*

SQL+ is a natural evolution to the SQL programming language. By simply adding Semantic Tags to your SQL in the form of comments, you can build enterprise-worthy data services in minutes. Feature for feature, SQL+ is the best ORM for C# and SQL.

## 1. Write SQL
Consider the following SQL. Nothing out of the ordinary. Insert a customer, and set the @CustomerId parameter to the new identity. So what's missing? Well, quite a bit, but let's keep this example simple, and focus on validating the parameters. Can the parameters be null, what is the max string length, is the email a valid email? SQL+ can make your code better!

```sql
CREATE PROCEDURE [dbo].[CustomerInsert]
(
    @CustomerId int out,
    @LastName varchar(64),
    @FirstName varchar(64),
    @Email varchar(64)
)
AS
BEGIN

   INSERT INTO [dbo].[Customer]
    (
        [LastName],
        [FirstName],
        [Email]
    )
   VALUES
    (
        @LastName,
        @FirstName,
        @Email
    );

    SET @CustomerId = SCOPE_IDENTITY();

END;
```

## 2. Add Semantic Tags
With SQL+ you can enforce parameter validation by simply adding a few tags. In this example we add required tags, max length tags, and an email tag. The SQL+ Code generation utility will use this information to escalate the validation into the generated services. If you change the underlying SQL, just rerun the builder, and your code is perfectly synchronized. Long story short, you have a single source of truth that you can trust throughout the enterprise.

```sql
CREATE PROCEDURE [dbo].[CustomerInsert]
(
    @CustomerId int out,

    --+Required
    --+MaxLength=64      
    @LastName varchar(64),

    --+Required
    --+MaxLength=64
    @FirstName varchar(64),

    --+Required
    --+MaxLength=64
    --+Email
    @Email varchar(64)
)
AS
BEGIN

   INSERT INTO [dbo].[Customer]
    (
        [LastName],
        [FirstName],
        [Email]
    )
   VALUES
    (
        @LastName,
        @FirstName,
        @Email
    );

    SET @CustomerId = SCOPE_IDENTITY();

END;
```

## 3. Configure Build Options
With your Semantically Tagged SQL in place, you can choose build options that match your exact use case. If you want to implement a particular interface, check a box and you are done.

![SQL+ Code Generation Utility Build Options](/images/build-options.png)

## 4. Generate Code
Just click build and let the tool do the work.

![SQL+ Code Generation Utility Execution](/images/GettingStartedBuild.gif)

## 5. Enjoy
You've now built a service that includes all the validation you need, and you did it by simply adding a few tags to your SQL. Best part of all, your code will run four times faster than Entity Framework and twice as fast as Dapper.

```csharp
[TestMethod]
public void CustomerInsertTest()
{
    CustomerInsertInput input = new CustomerInsertInput
    {
        Email =  "sample@email.com",
        FirstName = "FirstName",
        LastName = "LastName",
    };

    if (input.IsValid())
    {
        CustomerInsertOutput output = service.CustomerInsert(input);
    }
    else
    {
        // TODO: Handle invalid input object

        foreach(var error in input.ValidationResults)
        {
           //.......
        }
    }
}
```

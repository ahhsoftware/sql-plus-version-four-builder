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

# Installation
*Last updated: 3/6/2024*

Let's get you set up and ready to start generating some code. There are a few requirements before using these tools; you will need Visual Studio and SQL Server Management Studio. If you don't have these, you can get the [SQL Developer Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) and the [Visual Studio Community Edition](https://visualstudio.microsoft.com/vs/community/) for free from Microsoft.

## SSMS Snippets
You can utilize code snippets in SQL Server Management Studio to simplify adding the Semantic Tags when building database routines. To install the snippets download and extract the snippets. After extraction, it's helpful to move the "+" folder to a convenient location.

### Code Snippets Download
After downloading and extracting, in SQL Server Management Studio click on **Tools**, **Code Snippets Manager**.

![Launch the SSMS Code Snippets Manager](images/ssms-code-snippets-manager-launch.png)

On the Code Snippets Manager dialog, click **Add**.

![Click Add in the SSMS Code Snippets Manager](images/ssms-code-snippets-manager-click-add.png)

In the Code Snippets Directory dialog, navigate to the "+" folder you just extracted, and click **Select Folder**.

![Select folder in the SSMS Code Snippets Directory dialog](images/ssms-code-snippets-directory.png)

Click **OK** on the Code Snippets Manager Dialog to complete the installation.

![Click Ok in the SSMS Code Snippets Manager](images/ssms-code-snippets-manager-click-ok.png)

With the code snippets in place, you can right click, choose **Insert Snippet**, select the + folder, and add the desired tags.
![CInsert Snippets](images/SSMS-code-snippet-example.gif)
**Keyboard shortcut:** CTRL+K+X

## Visual Studio Code Snippets
You can utilize code snippets in Visual Studio to simplify adding the Semantic Tags when building ad-hoc queries. To install the snippets download and extract the snippets. After extraction, it's helpful to move the "+" folder to a convenient location.

### Code Snippets Download
After downloading and extracting, in Visual Studio click on **Tools**, **Code Snippets Manager**.

![Launch the VS Code Snippets Manager](images/vs-tools-code-snippets-manager-launch.png)

On the Code Snippets Manager dialog, set the language to Microsoft SQL Server Data Tools and click **Add**.

![Click Add in the VS Code Snippets Manager](images/vs-code-snippets-manager-click-add.png)

In the Code Snippets Directory dialog, navigate to the "+" folder you just extracted, and click **Select Folder**.

![Select folder in the VS Code Snippets Directory dialog](images/vs-code-snippets-directory.png)

Click **OK** on the Code Snippets Manager Dialog to complete the installation.

![Click Ok in the VS Code Snippets Manager](images/vs-code-snippets-manager-click-ok.png)

With the code snippets in place, you can right-click, choose **Insert Snippet**, select the "+" folder, and add the desired tags.
![Click Ok in the VS Code Snippets Manager](images/VS-code-snippet-example.gif)
**Keyboard shortcut:** CTRL+K+X

## Code Generator
The [Code Generation Utility](https://marketplace.visualstudio.com/items?itemName=AHHSoftware.V4) is available at the Visual Studio Market Place.

At the Visual Studio Marketplace click on the Download Button to get the SQL+ Code Generation Utility.

![SQL+ Code Generation Utility Download](images/sql-plus-code-generation-utility-download.png)

Navigate to the file you just downloaded and double click to launch the VSIX Installer, then click **Install**.

![Click Install on the VSIX installer](images/install-extension.png)

Let the installer do its thing. When finished click **Close**.

**Extension installation complete**

You are good to go!
```

# Miracle Devs DbPublisher
As part of the Paradigm ORM solution, we are including a small tool to enable quick and seamless database publishing and script generation.
The objective of this small tool to allow the user to work on the database elements (tables, views, procedures, scripts, etc) in separate files, without worrying about the concatenation and later execution of each file.
You can maintain your database structure as one file per element, reversioning them, handling each one as a regular code file, and let the tool run and publish them as one thing.


## Command Line Arguments

| Short           | Long                  | Description 
|-----------------|-----------------------|--------------------------------
| `-c <filename>` | `--config <filename>` | Indicates the path of the configuration file.
| `-h` or `-?`    | `--help`              | Prints the help.

### Examples

```
dbpublisher -c database/dbproject.json
```
```
dbpublisher --config configuration.json
```

## Configuration File
In order for the DbPublisher to work, you must provide a json configuration file containing the database type you want to use, the connection string, the script settings and a list of files and folders. The tool will search the folders and files, executing code for you if its configured to do so. But let's stop with the mumbo jumbo and jump over to the configuration structure.

### Properties

| Property            | Type          | Description
|---------------------|---------------|------------------------------------
| `databaseType`      | **string**    | The same database identification type used by the ORM configuration files. At present, it can be one of the following values: **mysql** \| **postgresql** \| **sql**
| `connectionString`  | **string**    | An regular [ADO.NET](https://msdn.microsoft.com/en-us/library/ms254500(v=vs.110).aspx) connection string. Note that each database may implement and use their own parameters, with their own meaning.
| `generateScript`    | **boolean**   | Indicates whether the tool should generate the publishing script or not. This can come in handy if you are planning to run the script later on using a tool like SQL Server Management Studio, MySql Workbench, etc, or if you need to send a third party IT team the scripts to run in order to deploy a new version of your system.
| `outputFileName`    | **string**    | File name for the generated publish script. I.E.: c:\Users\\[User]\Documents\publish.sql
| `executeScript`     | **boolean**   | Indicates if the tool should run the sql files against the database. This can come in handy on dev stages, to publish changes directly to a local DB. 
| `files`             | **string[]**  | An array of sql files to compile or run. This is the most straightforward approach to working with this tool, because you can reorder the scripts taking in account their mutual dependencies. Referenced DB elements should be processed first, because the tool will execute them in order. At the time of this writing, if you have a situation where there is a cross reference between elements, one of the constraints must be added on a third file, separated from the first table, so the Database has both tables declared. **IMPORTANT**: The filename should be relative to the configuration file, or an absolute path.
| `paths`             | **string[]**  | An array of folders to search for sql files. The tool will enumerate the files inside the folder, and will run the files in alphabetical order. This approach is less common, but if you have a bunch of files with no ordering needed, you can use these instead of manually adding each individual file. **IMPORTANT**: The path should be relative to the configuration file, or an absolute.
| `topDirectoyOnly`   | **boolean**   | If you provide paths, this property tells the system to search only on the top directory. If you want to recursively search for files inside inner folders, set this property to false.

### Example
````JSON
{
    "databaseType": "mysql",
    "connectionString": "SERVER=localhost;UID=root;PASSWORD=*****;Allow User Variables=True;Pooling=True;TreatTinyAsBoolean=true;Connection Reset=false",
    "generateScript": true,
    "executeScript": true,
    "outputFileName": "publish.sql",
    "files": [
        /* PRE-DEPLOYMENT */
        "scripts\\predeployment\\CreateDatabase.sql",

        /* TABLES AND VIEWS */
        "tables\\security\\Role.sql",
        "tables\\security\\User.sql",
        "tables\\security\\RoleView.sql",
        "tables\\security\\UserView.sql",

        "tables\\shared\\AddressType.sql",
        "tables\\shared\\Address.sql",
        "tables\\shared\\File.sql",   
        "tables\\shared\\AddressTypeView.sql",
        "tables\\shared\\AddressView.sql",
        "tables\\shared\\FileView.sql",

        /* STORED PROCEDURES */
        "storedprocedures\\security\\CreateUser.sql",
        "storedprocedures\\security\\ChangePassword.sql",

        /* SYSTEM DATA */   
        "scripts\\postdeployment\\AddressTypeData.sql",
        "scripts\\postdeployment\\RoleData.sql",
        "scripts\\postdeployment\\UserData.sql",
     ]
}
````

## Suggestions
* Always remember; the publish steps are meant to work for every situation, meaning that:
    - Some users won't have the database created at all, so you should provide a creation script.
    - Some users won't have the latest version, so the scripts should include incremental changes, to be executed in an orderly fashion.
    - If two or more objects have mutual references via unique keys, constraints, etc, separate them in two or three files, allowing the database to know the objects definitions before creating the relationship. Remember that if the user is creating the database for the first time, no table, view or procedure will be previously created, and the publish would fail.
* When adding data scripts, to insert system tables like Status, Types, or other pre-filled data, remember to use `MERGE`, `INSERT INTO () ON DUPLICATE KEY UPDATE` or other structures that won't break the publish.
* If you don't want to manually add each file to the configuration, you can numerate the files in order, and the tool will run them in alphabetical order. We may create a visual editor in the future to handle this configuration like a project file.

## Script Pre-Processor
Due to some limitations with Database Clients, script syntax may change between automatically deploying scripts using the ORM or manually executing a publish scripts later on.
For example, on MySql, if you try to create a [Stored Procedure](https://dev.mysql.com/doc/refman/5.7/en/stored-programs-defining.html) via script using MySql Command Line Client, PHP MyAdmin, or so, before creating the stored procedure, you must change the delimiter using the `DELIMITER` sentence, because any `;` inside the Stored Procedure Body will be interpreted as a end of sentence. But this won't happen if you send the creation instruction using a command. Not only that, but the delimiter notation will break your script.
To work around this limitation, DbPublisher utilizes a simple pre-processor notation to alter the code, taking the different notation into account either when compiling scripts and generating the publish file, or when executing the scripts directly in the DB.

### Define code only for the publishing file
````SQL
 #ifcmd 
    /* This code will only appear on the final publishing script */
 #endif
````


### Define code only for the direct execution
````SQL
 #ifexe 
    /* This code will only appear on execution scripts */
 #endif
````

So, if you come across a scenario where you have to decide whether to execute something or not depending on the target, you can use these pre-processor blocks to allow some code to be included only on that scenario.
If you (like us!) are planning on using MySql for your project, and you're thinking: "Oh Man, those beatiful Stored Procedures will accelerate my software response!" you are in for a treat. That is, before  DELIMITER issue drive you insane. So, for that particular scenario here is an example on how you to solve it using the pre-processor:

````SQL
DROP PROCEDURE IF EXISTS `ChangePassword`;
#ifcmd DELIMITER $$ #endif
CREATE PROCEDURE `ChangePassword`
(
	`UserId`	INT,
	`Password`	NVARCHAR(200)
)
BEGIN
	UPDATE `User` u
	   SET u.`PasswordHash` = `Password`
	 WHERE u.`Id` = `UserId`;
#ifcmd END $$ #endif #ifexe END; #endif
#ifcmd DELIMITER ; #endif
````

The example above will generate two different scripts: a compiled sql file to run manually later, and another one when the script is executed directly against the database:

### Publish File
````SQL
DROP PROCEDURE IF EXISTS `ChangePassword`;
DELIMITER $$
CREATE PROCEDURE `ChangePassword`
(
	`UserId`	INT,
	`Password`	NVARCHAR(200)
)
BEGIN
	UPDATE `User` u
	   SET u.`PasswordHash` = `Password`
	 WHERE u.`Id` = `UserId`;
END $$
DELIMITER ;
````

### Executed Script
````SQL
DROP PROCEDURE IF EXISTS `ChangePassword`;
CREATE PROCEDURE `ChangePassword`
(
	`UserId`	INT,
	`Password`	NVARCHAR(200)
)
BEGIN
	UPDATE `User` u
	   SET u.`PasswordHash` = `Password`
	 WHERE u.`Id` = `UserId`;
END;
````

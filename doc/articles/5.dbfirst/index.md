# Miracle Devs DbFirst
One of the main objectives behind Paradigm ORM was the ability to seamlessly integrate Database First workflows. We wanted to have full control of the database, and then derive our domain and logic from the database rather than the other way around. That allows us to leverage the DB muscle to retrieve and sort data: but it also means that the database needs to be designed with the domain in mind. 

Paradigm ORM DbFirst CLI tool allows users to create a object model from a database schema, and use that intermediate model to run the `Code Generation` tool that automatically outputs  the relevant classes. If you are planning on using this tool (and we highly encourage you to do so) have in mind that you'll be using other components of the Paradigm Core Suite.

Below you'll find a brief explanation of the DbFirst tool, its command line interface and how to use it.

> [!NOTE]
> Right now the configuration needs to be manually generated. We intend to add a configuration tool in the near future.

## Command Line Arguments

| Short           | Long                        | Description 
|-----------------|-----------------------------|--------------------------
| `-f`            | `--filenames <filename>`    | Indicates the path of a configuration file.
| `-d`            | `--directories <directory>` | Indicates the path of a directory in which the tool should search for configuration files.
| `-t`            | `--topdirectory`            | If directories were provided, indicates if the system should search only on the top directory.
| `-e`            | `--extension <extension>`   | If directories were provided, indicates the configuration file extension. The default extension is 'json'.
| `-?` or `-h`    | `--help`                    | Prints the help.

## Examples

This example opens two configuration files called module1 and module2.
```
dbfirst -f config/module1.json -f config/module2.json
```

This example provides two folders, "config" and "other/folder" to lookup for configuration files. Also, the `-t` is asking to search only in the top directory. And the `-e` is telling the tool to look for "*.dbf" files.
```
dbfirst -d config -d other/folder -t -e dbf
```

## Configuration
As we saw in the previous point, we must provide at least one configuration file for the tool to work. There are many options to consider when translating a DB schema into an object model, and it's simply not practical to provide all of them in the command line. Instead, we provide the tool a configuration file with all this settings. We'll see how to write this configuration file below.

> [!TIP]
> Internally, the configuration file is structured as a json file. If you have other configuration files (dbpublisher, codegen, etc) in the same folder, you can use different extensions for each to facilitate the search routine to find your files.

| Property           | Type                                                                          | Description
|--------------------|-------------------------------------------------------------------------------|-------------
| `databaseType`     | **string**                                                                    | The database identification type used by the ORM configuration files. It can be one of the following values: **mysql** \| **postgresql** \| **sql**
| `connectionString` | **string**                                                                    | A standard [ADO.NET](https://msdn.microsoft.com/en-us/library/ms254500(v=vs.110).aspx) connection string. Note that each database may implement and use their own parameters, with their own meaning.
| `databaseName`     | **string**                                                                    | Name of the database being used.
| `outputFileName`   | **string**                                                                    | A filename to save the resulting object model file.
| `tables`           | **Array of [TableConfiguration](#table-configuration)**                       | List of table configurations
| `views`            | **Array of [TableConfiguration](#view-configuration)**                        | List of view configurations.
| `storedProcedures` | **Array of [StoredProcedureConfiguration](#stored-procedure-configuration)**  | List of stored procedure configurations.

**Example:**
```json
{
  "databaseType": "mysql",
  "databaseName": "test.database",
  "connectionString": "Server=localhost;Database=test.database;UserId=test;Password=test1234;Pooling=true",
  "outputFileName": "models/testmodel.dbf",

  "tables": [...],
  "views": [...],
  "storedProcedures": [...]
}
```


First of all, you need to provide the database type you're planning to use. The database type needs to be supported by the ORM. Additionally, you need to include a connection string allowing the tool to connect to the database. When configuring the connection string, remember that the user should have permissions to retrieve schema information. Internally, the DbFirst will utilize the @MiracleDevs.ORM.Data.Database.Schema.ISchemaProvider to obtain the tables, views, procedures, columns, constraints and parameters to create the object model.
The configuration also expects the database name and output filename to save the object model. 

> [!NOTE]
> The object model produced by this tool is a `json` file with a specific format that the Code Generator tool understands. When fed to the Code Generator tool, this configuration file allows generation of all the scaffolding code and necessary files, making for a true DbFirst workflow.

The `tables`, `views` and `storedProcedures` parameters need a more thorough explanation, and we'll go through each one in more detail below. But we can say that they will basically help you configure individual elements to be mapped.

### Table Configuration
The `tables` configuration allows mapping customizations on tables, columns and constraints to objects. When mapping these objects, the naming conventions of the origin may not match your code naming conventions, and here you can map tables, columns or constraints to a name other than the one they have on the DB. You can also ignore existing columns or constraints if you don't want to map them, and even add your own. 

> [!NOTE]
> As we'll see in the next point, views use the same configuration as tables. Views don't have constraints like foreign or primary keys. But the DbFirst tool reads these keys to create navigation relationships, so its useful to be able to add your own constraints to elements. For example, adding a primary key to a view is mandatory, but you could also add a foreign key to another view as well and generate a navigation property.


| Property              | Type                                                              | Description
|-----------------------|-------------------------------------------------------------------|-------------
| `name`                | **string**                                                        | The name of the table you want to map.                
| `newName`             | **string**                                                        | You can provide a different name for the entity that will map to the table. This is useful when, for instance, your code convention is upper camel case and the database uses lowercase names.
| `columnsToRename`     | **Array of [RenameConfiguration](#rename-configuration)**         | An array of columns renames. Like the `newName` for the table, this array allows to rename columns.
| `constraintsToRename` | **Array of [RenameConfiguration](#rename-configuration)**         | An array of constraint renames. Like the `columnsToRename`, this array allows to rename constraints.
| `columnsToAdd`        | **Array of [ColumnConfiguration](#column-configuration)**         | Allows you to add new columns to the object. These columns won't be mapped to a table column in the DB, but there are occasions where this can come in handy. In PostgreSql for example, there are hidden system columns like oid. With this array you can add your own columns.
| `constraintsToAdd`    | **Array of [ConstraintConfiguration](#constraint-configuration)** | Like the `columnsToAdd`, allows to add new custom constraints.
| `columnsToRemove`     | **Array of string**                                               | Array of column names that will be removed from the final object; they won't be mapped.
| `constraintsToRemove` | **Array of string**                                               | Array of constraint names that will be removed from the final object; they won't be mapped.

**Example:**

```json
{
    "name": "project",
    "newName": "Project",
    "columsToRename": [...],
    "constraintsToRename": [...],
    "columnsToAdd": [...],
    "constraintsToAdd": [...],
    "columnsToRemove": [...],
    "constraintsToRemove": [...]
}
```

> [!TIP]
> If you are working with a stored procedure that returns a custom result (not a table or view) that is not mapped and does not have schema, you can add a custom table or view, and manually add the columns. The tool will treat it like any other existing database object.

#### Rename Configuration

The rename configuration allows to map both columns and constraints in the DB to a different name in your object model. This can be used to change the name of the entity property in the case of the columns.

| Property  | Type        | Description
|-----------|-------------|-------------
| `name`    | **string**  | The name of the column or constraint you want to change.     
| `newName` | **string**  | The new name for the column or constraint.

**Example:**

```json
{
    "name": "client_id",
    "newName": "ClientId"
}
```


#### Column Configuration

The column configuration allows you to add new columns to a table or view. This is helpful in some cases if you want to add a system column, or if you need to add a new entity.

| Property              | Type          | Description
|-----------------------|---------------|-------------
| `name`                | **string**    | Sets the column name.
| `dataType`            | **string**    | Sets the column database type.
| `maxSize`             | **int**       | Sets the maximum size.
| `precision`           | **int**       | Set the numeric precision.
| `scale`               | **int**       | Set the numeric scale.
| `defaultValue`        | **string**    | Set the default value.
| `isNullable`          | **bool**      | Indicates if the column is nullable.
| `isIdentity`          | **bool**      | Indicates if the column is an identity column (`AUTO_INCREMENT`, `SERIAL`, `IDENTITY`, etc).

**Example:**

```json
{
    "name": "Total",
    "dataType": "decimal",
    "precision": "20",
    "scale": "9",
    "IsNullable": "true"
}
```

#### Constraint Configuration

The constraint configuration allows to add new constraints to a table or view. This can come in handy when mapping views, to add new navigation relationships that are not available in the database.

| Property              | Type              | Description
|-----------------------|-------------------|-------------        
| Name                  | **string**        | Sets the name of the constraint.
| Type                  | **@MiracleDevs.ORM.Data.Database.Schema.Structure.ConstraintType**  | Sets the type of the constraint.
| FromColumnName        | **string**        | Sets the source column.
| ToTableName           | **string**        | Sets the destination table.
| ToColumnName          | **string**        | Sets the destination column.

**Example:**

```json
{
    "name": "Total",
    "dataType": "decimal",
    "precision": "20",
    "scale": "9",
    "IsNullable": "true"
}
```


### View Configuration

As we can see in the table above, the view configuration is a list of [TableConfiguration](#table-configuration) so the previous point is valid for views. You can add new columns, constraints, rename objects, remove objects, etc. using the same structure reviewed above.
If you are planning to use the existing Code Generator templates in conjunction with the Paradigm Core suite, be sure to add a primary key to each view, because if not, repositories will fail. 

**Example:**
```json
"views": [
    {
      "name": "clientview",
      "newName": "ClientView",
      "constraintsToAdd": [
        {
          "type": "PrimaryKey",
          "fromColumnName": "Id"
        }
      ]
    }
]
```


### Stored Procedure Configuration

The stored procedure configuration allows you to map stored procedures. The procedures are mapped in two different objects: `parameters` and `caller`. You can learn more [here](~/articles/4.shared/index.md#46-stored-procedures).
| Property              | Type                                                                    | Description
|-----------------------|-------------------------------------------------------------------------|-------------
| name                  | **string**                                                              | The name of the stored procedure you want to map.
| newName               | **string**                                                              | Sets a new name for the stored procedure mapping.
| type                  | **@MiracleDevs.ORM.Data.Database.Schema.Structure.StoredProcedureType** | Sets the type of stored procedure.
| parametersToRename    | **Array of [RenameConfiguration](#rename-configuration)**               | An array of parameter renames. Allows renaming of each parameter individually.
| resultTypes           | **Array of string**                                                     | An array of result objects. If your procedure retrieves a lists of clients, then the result types should include `Client`.

**Example:**

```json
{
    "name": "SearchClients",
    "type": "Reader",
    "resultTypes": [ "ClientView" ]
}
```
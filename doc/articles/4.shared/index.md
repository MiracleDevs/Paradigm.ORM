# 4. Shared Components
In this document we'll go over a brief description of all shared objects in Paradigm ORM. Shared objects hold all the database-agnostic logic that's reused across implementations. This is the true heart of the ORM functionality, handling mappings and conceptual models.


## 4.1. Attributes
Paradigm ORM makes extensive use of attributes to provide mapping and navigation information. Instead of using external `xml` or `json` configuration, the mapping configuration is provided in the code itself using attributes and decorators.
We'll summarize all the different attributes by type below.

### 4.1.1. Table Attributes

#### @Paradigm.ORM.Data.Attributes.TableAttribute
Indicates that the class or interface will be mapped to a table or view. If the table and class names are the same, then the name can be ignored. You can also provide the schema and catalog for a given table.

> [!NOTE]
> Not all databases allow multiple schemas or catalogs. In MySql the schema refers to the database. In Sql Server, the catalog refers to the database, and the schema to a group inside the database.

```csharp
[Table("client", Schema="mysql.test.database")]
public interface IClientTable
{

}
```

#### @Paradigm.ORM.Data.Attributes.TableTypeAttribute
Indicates that the class will be mapped to a table or view, but the mapping information is provided by another type.
```csharp
[TableType(typeof(IClientTable))]
public class Client
{

}
```

### 4.1.2. Column Attributes

#### @Paradigm.ORM.Data.Attributes.ColumnAttribute
Indicates that a property will be mapped to a table or view column. If the column and property names are the same, the name can be ignored. When working with `SQL Server`, the column type should be provided as well.
```csharp
[Column("name", "varchar")]
public string Name { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.IdentityAttribute
Indicates that the property will be mapped to an identity column. Identity columns are auto numeric, auto increment properties (SQL Server: `IDENTITY`, MySql: `AUTO_INCREMENT`, PostgreSql: `SERIAL`). Each database call them in different ways, and the implementation details may vary. The ORM just need to know if its responsible for the value, or can ignore it.
```csharp
[Identity]
public int Id { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.PrimaryKeyAttribute
Indicates that the column is part of the primary key.
```csharp
[PrimaryKey]
public int Id { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.ForeignKeyAttribute
Indicates that the column is part of a foreign key to another table. This attribute is not necessary if you are planning to create the classes by hand. If you use the dbfirst tool, the tool will populate this information automatically. This data can later be used to provide better error handling, ex. when a foreign key is failing, etc.
```csharp
[ForeignKey("FK_Address_Client", "Address", "ClientId", "Client", "Id")]
public int ClientId { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.UniqueKeyAttribute
Indicates that the column is part of a unique constraint. Like the `ForeignKeyAttribute` above, this attribute is not mandatory and will be automatically provided by the dbfirst tool.
```csharp
[Unique("UX_Client_Name", "name")]
public string Name { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.SizeAttribute
If the column is a sizeable type like `VARCHAR`, `BINARY`, etc, this attribute allows to provide the size for the column.
```csharp
[Size(255)]
public string Description { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.NumericAttribute
If the column type is a numeric type, this attribute provides a description for the numeric scale and precision.
```csharp
[Numeric(20,9)]
public decimal Credit { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.RangeAttribute
If the column type has a minimum and maximum value, this attribute provides the range values. This is helpful for numbers and dates. For example, each database handle date ranges differently.
```csharp
[Range("1000-01-01T00:00:00.0000000", "9999-12-31T23:59:59.0000000")]
public DateTime CreationDate { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.NotNullableAttribute
Indicates if the column does not allow null values.
```csharp
[NotNullable]
public DateTime CreationDate { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.NavigationAttribute
The `NavigationAttribute` **does not map** to a column. It provides a way to indicate that the property references a relation to another entity, and should be taken into account when retrieving or storing data.
```csharp
[Navigation(typeof(Address), "Id", "ClientId")]
public List<IAddress> Addresses { get; set; }
```

### 4.1.3. Routine Attributes

#### @Paradigm.ORM.Data.Attributes.RoutineAttribute
Indicates that the class or interface will be mapped to a stored procedure parameter object. If the stored procedure and class names are the same, the name can be ignored. You can also provide the schema and catalog of a given procedure.

> [!Note]
> We used "Routine" instead of simply "StoredProcedure" because in the future we could map functions as well. Both functions and procedures are considered routines, so we let the doors open.

> [!NOTE]
> Not all databases allow multiple schemas or catalogs. In MySql the schema refers to the database. In Sql Server, the catalog refers to the database, and the schema to a group inside the database.

```csharp
[Routine("SearchClient", Schema="mysql.test.database")]
public interface ISearchClientParameters
{

}
```

#### @Paradigm.ORM.Data.Attributes.RoutineTypeAttribute
Indicates that the class will be mapped to a stored procedure, but the mapping information is provided by another type.
```csharp
[RoutineType(typeof(ISearchClientParameters))]
public class SearchClientParameters: ISearchClientParameters
{

}
```

#### @Paradigm.ORM.Data.Attributes.RoutineResultAttribute
Indicates the result types of a stored procedure. This value is not mandatory, and will be automatically provided by the dbfirst tool.
```csharp
[RoutineResult("Client"))]
public class SearchClientParameters: ISearchClientParameters
{

}
```

### 4.1.4. Parameter Attributes

#### @Paradigm.ORM.Data.Attributes.ParameterAttribute
Indicates that the property will be mapped to a routine parameter. If the parameter and property names are equals, the name can be ignored.

```csharp
[Parameter("name", IsInput = true)]
public string Name { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.SizeAttribute
If the parameter is a sizeable type like `VARCHAR`, `BINARY`, etc, this attribute allows to provide the size for the parameter.
```csharp
[Size(50)]
public string ClientName { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.NumericAttribute
If the parameter type is a numeric type, this attribute provides a description for the numeric scale and precision.
```csharp
[Numeric(20,9)]
public decimal Credit { get; set; }
```

#### @Paradigm.ORM.Data.Attributes.RangeAttribute
If the parameter type has a minimum and maximum value, this attribute allows to provide the range values. This is helpful for numbers and dates. For example, each database handle date ranges differently.
```csharp
[Range("1000-01-01T00:00:00.0000000", "9999-12-31T23:59:59.0000000")]
public DateTime DateFrom { get; set; }
```


## 4.2. Descriptors
Descriptors are classes that handle and provide mapping information between various database object and .NET types. Descriptors are used by many of the ORM core classes like mappers, database access objects, query and stored procedure executers, etc. Below we give a brief overview of each one.

### 4.2.1. Table-Type descriptors
The Table-Type descriptor class describes how a table maps to a .NET type and vice versa. The table type descriptor extracts all the mapping information and produces a structure that is known by the rest of the ORM. The Table-type descriptor in particular has a complex mapping discovery algorithm that we describe below:

1. If the class is decorated with the @Paradigm.ORM.Data.Attributes.TableAttribute, then search for all the properties (own and inherited) and look for attributes in them, or other parent classes or interfaces. Basically, you can decorate mapped properties in any of the hierarchy classes or interfaces.
2. If the class is decorated with the @Paradigm.ORM.Data.Attributes.TableTypeAttribute, the ORM will behave the same as (1) but will also include the referenced type, its base classes and interfaces. In the case of (2), the relation will be made by name. If a property in the referenced type or one of its parents is called like the object property, it will inherit the mapping decorators.

This allow us to mix up different and interesting ways, and map properties across entities, leveraging OOP best practices. You'll find more examples in the [tutorial section](~/tutorials/1.gettingstarted/index.md#net-mappings).

Creating a new table-type descriptor is pretty straightforward. From it you can extract information about mapped properties, primary keys, identity columns and more:

```csharp
var descriptor = new TableTypeDescriptor(typeof(Client));

// retrieves all the properties with mapping decorations.
var allProperties = descriptor.MappingProperties;

// retrieves the properties without the identity column if any.
var simpleProperties = descriptor.SimpleProperties;

// retrieves only the properties that map to a primary key column.
var primaryKeyProperties = descriptor.PrimaryKeys;

// retrieves the identity property if any.
var identityProperty = descriptor.IdentityProperty;

// retrieves the navigation properties.
var navigationProperties = descriptor.NavigationProperties;

// gets the .net type name.
var typeName = descriptor.TypeName;

// gets the table name.
var tableName = descriptor.TableName;

// gets the catalog name if any.
var catalogName = descriptor.CatalogName;

// gets the schema name if any.
var schemaName = descriptor.SchemaName;
```
Having the property mappings, a table-type descriptor also can tell if an object is new or not. Always assuming that the default values won't represent a valid Id.
This method also works with multiple primary keys. If at least one of the primaries does not have value (or is equal to its default value) it will be considered as new:

```csharp
var descriptor = new TableTypeDescriptor(typeof(Client));
var newClient = new Client();

if (descriptor.IsNew(newClient))
{
    System.Console.WriteLine("The client is new.");
}
```

### 4.2.2. Custom-Type descriptors
In some cases, you just need to map a result from a custom query, and you don't need the complex mapping rules, or event the table type; you just need the column mapping. For this purpose the @Paradigm.ORM.Data.Descriptors.CustomTypeDescriptor allows to have only column mappings. This type is used by the @Paradigm.ORM.Data.Querying.CustomQuery`1 to handle the result set. Also, mappers accepts both the custom-type and table-type mappings.

```csharp
var descriptor = new CustomTypeDescriptor(typeof(Client));

// retrieves all the properties with mapping decorations.
var properties = descriptor.MappingProperties;
```

### 4.2.3. Column-Property descriptors
The column-property descriptor class describes how a column is mapped to a property and vice versa. Only table-type and custom-type descriptors can create column property descriptors.

With the column-property information, the ORM knows how to manage primary keys, identity columns, value types, etc.

```csharp
var descriptor = new CustomTypeDescriptor(typeof(Client));
var propertyDescriptor = descriptor.IdentityProperty;

// retrieves the property name.
var propertyName = propertyDescriptor.PropertyName;

// retrieves the column name.
var columnName = propertyDescriptor.ColumnName;

// retrieves the property .net type.
var propertyType = propertyDescriptor.PropertyType;

// if the property type is nullable, returns the generic argument.
var notNullableType = propertyDescriptor.NotNullablePropertyType;

// retrieves the db type.
var dataType = propertyDescriptor.dataType;

// retrieves the dbtype maximum size.
var maxSize = propertyDescriptor.MaxSize;

// retrieves the numeric precision.
var precision = propertyDescriptor.Precision;

// retrieves the numeric scale.
var scale = propertyDescriptor.Scale;

// indicates if the column is the identity of the table.
var isIdentity = propertyDescriptor.IsIdentity;

// indicates if the column is one of the primary keys.
var isPrimaryKey = propertyDescriptor.IsPrimaryKey;

// indicates if the column is part of a foreign key.
var isForeignKey = propertyDescriptor.IsForeignKey;

// indicates if the column is part of a unique key.
var isUniqueKey = propertyDescriptor.IsUniqueKey;

```

### 4.2.4. Navigation descriptors
The navigation-property descriptor class describes how a property from one entity is connected to another entity by either a 1-to-1 or 1-to-many relationship. This is helpful when working with complex entities and domain aggregates. Only a table-type descriptor can create navigation descriptors.

Navigation descriptors are used extensively inside @Paradigm.ORM.Data.DatabaseAccess.DatabaseAccess and @Paradigm.ORM.Data.DatabaseAccess.NavigationDatabaseAccess to handle the relationships when selecting, inserting, updating and deleting entities. If you want these relationships to be automatically handled by the system, try using the @Paradigm.ORM.Data.DatabaseAccess.DatabaseAccess class.

```csharp
var descriptor = new CustomTypeDescriptor(typeof(Client));
var navigationDescriptor = descriptor.NavigationDescriptors.First();

// Gets the name of the property decorated with the navigation information.
var propertyName = navigationDescriptor.PropertyName;

// Gets the type of the property decorated with the navigation information.
var propertyType = navigationDescriptor.PropertyType;

// Gets the table type descriptor for the source type.
var fromDescriptor = navigationDescriptor.FromDescriptor;

// Gets the table type descriptor for the referenced type.
var toDescriptor = navigationDescriptor.ToDescriptor;

// Gets the navigation attributes.
var navigationAttributes = navigationDescriptor.NavigationAttributes;

// Gets the navigation key descriptors.
var navigationKeyDescriptors = navigationDescriptor.NavigationKeyDescriptors;

// Gets a value indicating whether this instance is the aggregate root on the navigation.
var isAggregateRoot = navigationDescriptor.IsAggregateRoot;
```

> [!IMPORTANT]
> The property `IsAggregateRoot` doesn't mean that the "from" entity is the aggregate root of a hierarchy of domain objects. This value describes the entity "from" priority in this current relationship.

### 4.2.5. Routine-Type descriptors
The routine-type descriptor class describes how a stored procedure maps to a .net type and vice versa. Stored procedures will be mapped as a type that we call "parameters".

> [!NOTE]
>A parameter object is a collection of all the stored procedure parameters mapped to properties. When calling stored procedures, you can use these parameter objects to execute them using the proper classes.

The routine-type descriptor works exactly like the table-type descriptor. You have two different attributes to map, and the logic behind the parameter acquisition works in similar way, searching in base classes and interfaces, or referenced types.
For more information on how stored procedures are mapped, go to the [tutorial section](~/tutorials/1.gettingstarted/index.md#stored-procedures).


### 4.2.6. Parameter-Property descriptors
The parameter-property descriptor class describes how a stored procedure parameter maps to a property. Only the routine-type descriptor can create parameter-property descriptors. These descriptors are similar to column-property with some small differences. Some of the mapping attributes can be used with parameters as well. See the attribute section.


## 4.3. Database Reader Mappers
The database reader mappers are in charge of mapping from a database result to one or more class instances.

When performing reading operations with either a select or a stored procedure, the ORM will provide a @Paradigm.ORM.Data.Database.IDatabaseReader instance. This object works as a reading cursor. Instead of manually processing all the results, mappers take the table-type or custom-type descriptor, and map the results automatically.

```csharp
var mapper = new DatabaseReaderMapper(typeof(Client));

var clients = connector.ExecuteReader("SELECT * FROM Client", reader => mapper.Map(reader));
```
As we can see in the example above, first we need to create a mapper for a given type (the type needs to be mapped), and then we execute a `SELECT` reader operation and pass the resulting @Paradigm.ORM.Data.Database.IDatabaseReader to the mapper. The mapper will return a collection of clients.

When working with DbFirst, the tool will create typed mappers, and will override the protected `MapRow` method to avoid the use of reflection, and to map line by line. The DbFirst tool knows the object structure, and can optimize the mapping process.

```csharp
    public interface IAddressTypeDatabaseReaderMapper
        : IDatabaseReaderMapper<AddressType>
    {
    }

    public class AddressTypeDatabaseReaderMapper
        : DatabaseReaderMapper<AddressType>,
          IAddressTypeDatabaseReaderMapper
    {
        #region Protected Methods

        protected override object MapRow(IDatabaseReader reader)
        {
            var entity = new AddressType();

            entity.Id = reader.GetInt32(0);

            entity.Name =  reader.IsDBNull(1)
                ? default(string)
                : reader.GetString(1);

            entity.Code =  reader.IsDBNull(2)
                ? default(string)
                : reader.GetString(2);

           entity.Description =  reader.IsDBNull(3)
                ? default(string)
                : reader.GetString(3);

            return entity;
        }

        protected override async Address<object> MapRowAsync(IDatabaseReader reader)
        {
            var entity = new AddressType();

            entity.Id = reader.GetInt32(0);

            entity.Name = await reader.IsDBNullAsync(1)
                ? default(string)
                : reader.GetString(1);

            entity.Code = await reader.IsDBNullAsync(2)
                ? default(string)
                : reader.GetString(2);

            entity.Description = await reader.IsDBNullAsync(3)
                ? default(string)
                : reader.GetString(3);

            return entity;
        }

        #endregion
    }
}
```
In this example we can see how the DbFirst generates a proper typed mapper for the entity AddressType. First of all, the system generates a mapper interface, and then a mapper implementation. For the implementation, it overrides the methods `MapRow` and `MapRowAsync`. This will provide a little performance boost on entities with several fields, instead of mapping with reflection, boxing and un-boxing values.

The DbFirst tool also provide a summary file with all the registrations needed both for mapping and DI. If we take a look to that file, we can see the following lines:

```csharp
builder.Register<ITaskTypeDatabaseReaderMapper, TaskTypeDatabaseReaderMapper>();
builder.Register<IDatabaseReaderMapper<TaskType>, TaskTypeDatabaseReaderMapper>();
```
In the first line we can see that is registering the interface and the implementation of the previous example. But in the second line it is registering a generic interface `IDatabaseReaderMapper<TaskType>` for the same implementation. This is important: while this line may not make much sense now, it's a key aspect of Paradigm ORM. The inner workings of the ORM doesn't know your concrete interface or class, but can ask the service provider for a `IDatabaseReaderMapper<TEntity>` for a given entity. If the service provider does not respond with a proper mapper, the system will create a regular @Paradigm.ORM.Data.Mappers.IDatabaseReaderMapper instead. If you want the ORM to use your faster mappers, you have to register them under this generic type definition.

> [!NOTE]
> All the ORM classes are prepared to work with dependency injection and most of them expect a `IServiceProvider` reference in one of their constructors.


## 4.4. Database Access
The database access object provides a way to create, read, update and delete entities (CRUD). If you are familiar with Entity Framework, database access is similar to a DbContext. There are some key differences though. The database access object handles one entity in particular, and all its navigation relationships to other entities. When creating a database access object, a .NET type or a table-type descriptor needs to be provided. The database access object does not need any type of model or context to work: a table-type descriptor is enough to allow these objects to create all the commands, and create navigation database objects for each one of the navigation properties.

See:
- @Paradigm.ORM.Data.DatabaseAccess.IDatabaseAccess
- @Paradigm.ORM.Data.DatabaseAccess.DatabaseAccess
- @Paradigm.ORM.Data.DatabaseAccess.Generic.IDatabaseAccess`1
- @Paradigm.ORM.Data.DatabaseAccess.Generic.DatabaseAccess`1

> [!NOTE]
> In these documents we explain all the different classes that make up the ORM, but the @Paradigm.ORM.Data.DatabaseAccess.DatabaseAccess is the most important class to understand. It holds all the logic to work domain and relational models.

Let's see how to create and use a Database Access object.

**Creating a new instance**
```csharp
using(var databaseAccess = new DatabaseAccess<Client>(connector))
{
}
```

**Inserting a new clients**
```csharp
databaseAccess.Insert(clients);
```

**Reading clients**
```csharp
var clients = databaseObject.Select();
```

**Reading some of the clients**
```csharp
var clients = databaseObject.Select($"`{nameof(Client.Name)}` LIKE '%{value}'");
```

**Updating clients**
```csharp
databaseAccess.Update(clients);
```

**Deleting clients**
```csharp
databaseAccess.Delete(clients);
```

All operations are straightforward enough to use and understand. This being said, there are some implementation details to discuss. First of all, the database access object differs from a DbContext in the sense that the database object does not try to mimic a Unit of Work. In other words, when you execute the `INSERT`, `UPDATE` or `DELETE`, the action will be immediately executed on the database. If you want to emulate the functionality of entity framework and its [SaveChanges](https://msdn.microsoft.com/en-us/library/system.data.entity.dbcontext.savechanges(v=vs.113).aspx), you could use a transaction scope as detailed above.

Another important fact about the database object is the batch capability. The `INSERT`, `UPDATE` and `DELETE` methods can be called for a single entity, or for many. Whenever possible you should actually try and calling it for multiple entities, as database access object batch all the operations and save roundtrips to the database ultimately enhancing performance.

When working with DbFirst, the tool will create a concrete interface and implementation for a specific type, like we saw in the previous point with the mappers. Database access object can work with dependency injection as well.
Let's take a look to the auto-generated code:

```csharp
public partial interface IClientDatabaseAccess : IDatabaseAccess<Client>
{
    #region Properties

    IAddressDatabaseAccess AddressDatabaseAccess { get; }

    IProjectDatabaseAccess ProjectDatabaseAccess { get; }

    #endregion
}

public partial class ClientDatabaseAccess : DatabaseAccess<Client>, IClientDatabaseAccess
{
    #region Properties

    public IAddressDatabaseAccess AddressDatabaseAccess { get; private set; }

    public IProjectDatabaseAccess ProjectDatabaseAccess { get; private set; }

    #endregion

    #region Constructor

    public ClientDatabaseAccess(IServiceProvider provider) : base(provider)
    {
    }

    #endregion

    #region Protected Methods

    protected override void AfterInitialize()
    {
        base.AfterInitialize();

        this.AddressDatabaseAccess = this.NavigationDatabaseAccesses.FirstOrDefault(x => x.NavigationPropertyDescriptor.PropertyName == nameof(Client.Address))?.DatabaseAccess as IAddressDatabaseAccess;

        if (this.AddressDatabaseAccess == null)
            throw new Exception("ClientDatabaseAccess couldn't retrieve a reference to AddressDatabaseAccess.");

        this.ProjectDatabaseAccess = this.NavigationDatabaseAccesses.FirstOrDefault(x => x.NavigationPropertyDescriptor.PropertyName == nameof(Client.Projects))?.DatabaseAccess as IProjectDatabaseAccess;

        if (this.ProjectDatabaseAccess == null)
            throw new Exception("ClientDatabaseAccess couldn't retrieve a reference to ProjectDatabaseAccess.");

    }

    #endregion
}
```

Like we saw with mappers, the DbFirst tool creates the interface and the implementation for us. Both are concrete, related to one specific type. The DbFirst also knows the navigation relationships of a given type and will save a reference to the related database access object in case it's needed. Also, like all the other auto-generated files, both the interface and the class are declared as partials just in case you need to add custom logic to the object. Be mindful to add that logic somewhere else as these partial classes will be overridden every time the DBFirst tool is run.
The DbFirst tool also generates a summary file with all the registrations, as we saw before. Let's take a look to this specific registration:

```csharp
builder.Register<IClientDatabaseAccess, ClientDatabaseAccess>();
builder.Register<IDatabaseAccess<Client>, ClientDatabaseAccess>();
```

Again, it's registering the concrete interface, and the generic interface for type `Client` to the concrete class. The logic is the same as with mappers. Whenever the ORM needs a database access object for a given type, will ask for @Paradigm.ORM.Data.DatabaseAccess.Generic.IDatabaseAccess`1 where TEntity is `Client`. So it's important that both are registered. If not, the ORM will utilize the base @Paradigm.ORM.Data.DatabaseAccess.DatabaseAccess instead.

## 4.5. Queries
The ORM provides a way to easily execute `SELECT` queries from known types.
There are two query types:

- @Paradigm.ORM.Data.Querying.Query`1
- @Paradigm.ORM.Data.Querying.CustomQuery`1

The main difference between both, is how they manage the `SELECT` command inside.

### 4.5.1. Standard Query
The standard query object, utilizes a @Paradigm.ORM.Data.Descriptors.ITableTypeDescriptor and a  @Paradigm.ORM.Data.CommandBuilders.ISelectCommandBuilder to create a standard `SELECT` query. The execution methods allows the user to pass a custom `WHERE` clause, but the content of the select can not be altered in any way. Also, all the properties of an object will be retrieved.
So, as a brief summary:

- Creates the `SELECT` statement automatically.
- All the mapped properties are selected.
- Can receive a custom `WHERE` clause on each execution.
- Does not retrieve navigation entities, only the main entity.

**Example**
```csharp
using(var query = new Query<Client>(connector))
{
    var clients = query.Execute();
}
```

The standard query is a good match when you have to return a table or view that you already mapped. The query object is currently being used by the @Paradigm.ORM.Data.Database.Schema.ISchemaProvider to select the tables, views and stored procedures.

### 4.5.2. Custom Query
The custom query object, uses a @Paradigm.ORM.Data.Descriptors.ICustomTypeDescriptor to know how to map a result, but won't create the `SELECT` statement. The user must provide the `SELECT` body without a `WHERE` clause.

- Does not creates the `SELECT` statement automatically, expects the user to provide it.
- All the mapped properties are selected.
- Can receive a custom `WHERE` clause on each execution.
- Does not retrieve navigation entities, only the main entity.

**Example**

```csharp
using(var query = new CustomQuery<Client>(connector, "SELECT * FROM `client`"))
{
    var clients = query.Execute();
}
```

The custom query can be used when the `SELECT` requires more logic than only listing columns, for example using a `CASE WHEN` or call a function. The custom query object is currently being used by the @Paradigm.ORM.Data.Database.Schema.ISchemaProvider to select the columns, constraints and parameters.

## 4.6. Stored Procedures
The ORM provides a way to execute stored procedures out of the box. There are 3 types of stored procedures, depending on how they'll be executed: Reader, Scalar and Non Query. All the types share the same approach to execute, with some minor differences.

Before showing each type of stored procedure, we'll describe how the ORM handles the procedures. For the ORM, a stored procedure is made of at least two objects: The procedure parameters object, and the procedure caller.
The procedure parameter object is a POCO object (plain old csharp object) containing all the stored procedure parameters, mapped to properties.

**Example**
```csharp
[Routine("SearchClients")]
public class SearchClientsParameters
{
    [Parameter]
    public string ClientName { get; set; }

    [Parameter]
    public string Address { get; set; }

    [Parameter]
    public decimal CreditFrom { get; set; }

    [Parameter]
    public decimal CreditTo { get; set; }
}
```
> [!IMPORTANT]
> The @Paradigm.ORM.Data.Attributes.RoutineAttribute is mandatory for the caller to know what stored procedure to execute. The parameters object also supports the @Paradigm.ORM.Data.Attributes.RoutineTypeAttribute, to separate the mapping from the object itself. The same rules we saw on TableTypeDescriptors apply for RoutineTypeDescriptors.

The idea behind the stored procedure parameters object is to pass all the parameters at once, instead of passing each one individually. This can also come in handy if the stored procedure firm changes: you just need to add or remove parameters. Instead of having to change all the layers from the client to the data access layer, you just alter one object and call it a day.

As we mentioned before, there are two type of objects, the parameters and the callers. The ORM provides 3 different callers, one for each type of stored procedures. Each one is explained in detail below.

### 4.6.1. Reader
A reader procedures returns a reading cursor to the database, and retrieves one or more results. The ORM supports up to 8 different result sets from a reading operation. If your stored procedure returns more results, you can extend and create your own callers.

See:
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`2
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`3
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`4
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`5
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`6
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`7
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`8
- @Paradigm.ORM.Data.StoredProcedures.ReaderStoredProcedure`9

When executing stored procedures with more than one result, the caller will return a tuple of lists, where each tuple item is one of the results.

```csharp
var caller = new ReaderStoredProcedure<SearchClientsParameters, Client, ClientAdditionalInformation, Address>(connector);
var results = caller.Execute(parameters);
```

### 4.6.2. Scalar
A scalar procedure returns a scalar value as a result. They are similar to reader procedures, but have less parameters and possibilities:

See: @Paradigm.ORM.Data.StoredProcedures.ScalarStoredProcedure`2

```csharp
var caller = new ScalarStoredProcedure<GetClientsCount, int>(connector);
var clientsCount = caller.Execute(parameters);
```

### 4.6.3. Non Query
A non query procedure does not returns any type of result. The non query procedures are perfect to execute operations on the database that don't need to inform back to the application. They are faster as well because there is no need to open a reading cursor to the database.

See: @Paradigm.ORM.Data.StoredProcedures.NonQueryStoredProcedure`1

```csharp
var caller = new NonQueryStoredProcedure<AfterSaveClient>(connector);
caller.Execute(parameters);
```

## 4.7. Batch
As we saw in [4.5](#44-database-access) the database access object will try to batch operations to avoid unnecessary roundtrips. Internally, each database access object will use an instance of @Paradigm.ORM.Data.Batching.BatchManager. The batch manager, as the name implies, manage one or more batches. Either because you explicitly told the manager to add a new batch, or because the manager realized the batch reached its maximum, there can be multiple batches to execute.

A batch manager contains one or more @Paradigm.ORM.Data.Batching.CommandBatch which in time contains one or more @Paradigm.ORM.Data.Batching.CommandBatchStep. Each time you add a new command to be executed by the batch manager, the manager will try to add the command to the current command batch, if its full, it will create a new one to add the command.

But adding up commands is not the only thing a batch manager does. It also provides a way to react to the results of a batch execution either at a command level, or at a batch level. When adding new commands or asking for a new batch, you can pass a callback action to be called after the execution.

```csharp
var batchManager = new BatchManager(connector);

batchManager.Add(new CommandBatchStep(command1));
batchManager.Add(new CommandBatchStep(command2, reader => /* do something here */));

batchManager.AddNewBatch(() => /* do something after the first batch executed */);

batchManager.Add(new CommandBatchStep(command3, reader => /* do something here */));

batchManager.Execute();
```

So, in this example, the batch manager will run two batches, one made of `command1` and `command2`, and then another batch made of `command3`.
The manager will execute the batches in order. After executing the first batch, will see if their commands have a callback, and will call the action method passing the reader as parameter. Once the callback has been executed, the batch will move the reading cursor to the next result.
# Badger.Data
Simple data access for .net

![Badger badger badger](https://media.giphy.com/media/zXHZWGLWNQkrS/giphy.gif)

[![Build Status](https://travis-ci.org/timbarker/Badger.Data.svg?branch=master)](https://travis-ci.org/timbarker/Badger.Data)
[![Nuget Package](https://img.shields.io/nuget/v/Badger.Data.svg?style=flat)](https://www.nuget.org/packages/Badger.Data/)

## Examples

### Inserting a row into a table

```csharp
class InsertPersonCommand : ICommand
{
    private readonly string name;
    private readonly DateTime dob;

    public InsertPersonCommand(string name, DateTime dob)
    {
        this.name = name;
        this.dob = dob;
    }
    
    public IPreparedCommand Prepare(ICommandBuilder builder)
    {
        return builder
            .WithSql("insert into people(name, dob) values (@name, @dob)")
            .WithParameter("name", this.name)
            .WithParameter("dob", this.dob)
            .Build();
    }
}

class Program 
{
    static async Task Main() 
    {
        var sessionFactory = new SessionFactory.With(config =>
        {
            config
                .WithProviderFactory(SqliteFactory.Instance)
                .WithConnectionString("Data Source='database.db'");
        });

        using (var session = sessionFactory.CreateCommandSession())
        {
            await session.ExecuteAsync(new InsertPersonCommand("Bob", new DateTime(2000, 1, 1)));

            session.Commit();
        }
    }
}
```

### Query for many rows
```csharp

public class Person
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime Dob { get; set; }
}

class GetAllPeopleQuery : IQuery<IEnumerable<Person>>
{
    public IPreparedQuery<IEnumerable<Person>> Prepare(IQueryBuilder builder)
    {
        return builder
            .WithSql("select id, name, dob from people")
            .WithMapper(r => new Person 
                { 
                    Id = r.Get<long>("id"), 
                    Name = r.Get<string>("name"), 
                    Dob = r.Get<DateTime>("dob")
                })
            .Build();
    }
}

class Program 
{
    static async Task Main() 
    {
        var sessionFactory = new SessionFactory.With(config =>
        {
            config
                .WithProviderFactory(SqliteFactory.Instance)
                .WithConnectionString("Data Source='database.db'");
        });

        using (var session = sessionFactory.CreateQuerySession())
        {
            var people = await session.ExecuteAsync(new GetAllPeopleQuery());

            foreach (var person in people)
            {
                Console.WriteLine($"{person.Name} born on {person.Dob}");
            }
        }
    }
}
```

### Query for a single row
```csharp
class FindPersonByNameQuery : IQuery<Person>
{
    private readonly string name;

    public FindPersonByNameQuery(string name)
    {
        this.name = name;
    }

    public IPreparedQuery<Person> Prepare(IQueryBuilder builder)
    {
        return builder
            .WithSql("select name, dob from people where name = @name")
            .WithParameter("name", this.name)
            .WithSingleMapper(row => new Person 
            {
                Name = row.Get<string>("name"),
                Dob = row.Get<DateTime>("dob")
            })
            .Build();
    }
}

class Program 
{
    static async Task Main() 
    {
        var sessionFactory = new SessionFactory.With(config =>
        {
            config
                .WithProviderFactory(SqliteFactory.Instance)
                .WithConnectionString("Data Source='database.db'");
        });

        using (var session = sessionFactory.CreateQuerySession())
        {
            var person = await session.ExecuteAsync(new FindPersonByNameQuery("bob"));

            Console.WriteLine($"{person.Name} born on {person.Dob}");
        }
    }
}
```

### Query for a single value
```csharp
class CountPeopleQuery : IQuery<long>
{
    public IPreparedQuery<long> Prepare(IQueryBuilder builder)
    {
        return builder
            .WithSql("select count(*) from people")
            .WithScalar<long>()
            .Build();
    }
}

class Program 
{
    static async Task Main() 
    {
        var sessionFactory = new SessionFactory.With(config =>
        {
            config
                .WithProviderFactory(SqliteFactory.Instance)
                .WithConnectionString("Data Source='database.db'");
        });

        using (var session = sessionFactory.CreateQuerySession())
        {
            var count = await session.ExecuteAsync(new CountPeopleQuery());

            Console.WriteLine($"There are {count} people");
        }
    }
}

```

### Configuring a SessionFactory
```csharp
class Program
{
    static void Main()
    {
        var sessionFactory = SessionFactory.With(config =>
        {
            config
                .WithProviderFactory(SqlClientFactory.Instance)
                .WithConnectionString("Data Source='database.db'")
                .WithTableParameterHandler<long>((value, parameter) =>
                {
                    /* Database engines such as MSSQL Server do not
                       support array types natively, therefore one can
                       declare a custom table type and map your array onto it.
                       The code below, allows an IEnumerable<long> to be mapped to a
                       custom table `t_BigIntArray` with a column `id` 
                       of `SqlDbType.BigInt`
                    */

                    var sqlParameter = (SqlParameter)parameter;

                    SqlMetaData[] tvpDefinition = { new SqlMetaData("id", SqlDbType.BigInt) };
                    sqlParameter.Value = value.Select(i =>
                    {
                        var sqlDataRecord = new SqlDataRecord(tvpDefinition);
                        sqlDataRecord.SetInt64(0, i);
                        return sqlDataRecord;
                    }).ToList();

                    sqlParameter.SqlDbType = SqlDbType.Structured;
                    sqlParameter.TypeName = "t_BigIntArray";
                })
                .WithParameterHandler<Person>((value, parameter) =>
                {
                    /* Similarly, one can define how custom types are mapped
                       into database parameters. The example below maps a `Person`
                       into a DbType.String by taking the `Name` property and assigning 
                       to the parameter.
                    */

                    parameter.Value = value.Name;
                    parameter.DbType = DbType.String;
                });
        });
    }
}
```

### Query by table parameter
```csharp
class GetPeopleIdsQuery : IQuery<IEnumerable<long>>
{
    private readonly long[] _ids;

    public GetPeopleIdsQuery(params long[] ids)
    {
        _ids = ids;
    }

    public IPreparedQuery<IEnumerable<string>> Prepare(IQueryBuilder queryBuilder)
    {
        return queryBuilder
            .WithSql("select p.name from people p inner join @ids i on i.id = p.id")
            .WithTableParameter("@ids", _ids)
            .WithMapper(r => r.Get<string>("id"))
            .Build();
    }
}

class Program
{
    static void Main()
    {
        using (var session = sessionFactory.CreateQuerySession())
        {
            var peopleIds = new[] {1L, 2L};
            var names = session.Execute(new GetPeopleIdsQuery(peopleIds));

            Console.WriteLine($"The people are {string.Join(", ", names)}");
        }
    }
}
```

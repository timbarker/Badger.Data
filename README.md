# Badger.Data
Simple data access for .net

[![Build Status](https://travis-ci.org/timbarker/Badger.Data.svg?branch=master)](https://travis-ci.org/timbarker/Badger.Data)

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
        var sessionFactory = new SessionFactory(SqliteFactory.Instance, "Data Source='database.db'");

        using (var session = sessionFactory.CreateAsyncSession())
        {
            await session.ExecuteCommandAsync(new InsertPersonCommand("Bob", new DateTime(2000, 1, 1)));
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
        var sessionFactory = new SessionFactory(SqliteFactory.Instance, "Data Source='database.db'");

        using (var session = sessionFactory.CreateAsyncSession())
        {
            var people = await session.ExecuteQueryAsync(new GetAllPeopleQuery());

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
        var sessionFactory = new SessionFactory(SqliteFactory.Instance, "Data Source='database.db'");

        using (var session = sessionFactory.CreateAsyncSession())
        {
            var person = await session.ExecuteQueryAsync(new FindPersonByNameQuery("bob"));

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
        var sessionFactory = new SessionFactory(SqliteFactory.Instance, "Data Source='database.db'");

        using (var session = sessionFactory.CreateAsyncSession())
        {
            var count = await session.ExecuteQueryAsync(new CountPeopleQuery());

            Console.WriteLine($"There are {count} people");
        }
    }
}

```

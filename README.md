## Badger.Data
Simple data access for .net

### Example

Executing a database command

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

    public int Execute(IDbCommandBuilder builder)
    {
        return builder
            .WithSql("insert into people(name, dob) values (@name, @dob)")
            .WithParameter("name", this.name)
            .WithParameter("dob", this.dob)
            .Execute();
    }
}

class Program 
{
    static void Main() 
    {
        var sessionFactory = new DbSessionFactory(SqliteFactory.Instance, "Data Source='database.db'");

        using (var session = sessionFactory.CreateSession())
        {
            session.ExecuteCommand(new InsertPersonCommand("Bob", new DateTime(2000, 1, 1)));
        }
    }
}
```

Executing a database query
```csharp

public class Person
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime Dob { get; set; }
}

class GetAllPeopleQuery : IQuery<IEnumerable<Person>>
{
    public IEnumerable<Person> Execute(IDbQueryBuilder builder)
    {
        return builder
            .WithSql("select id, name, dob from people")
            .Execute(r => new Person 
                { 
                    Id = r.Get<long>("id"), 
                    Name = r.Get<string>("name"), 
                    Dob = r.Get<DateTime>("dob")
                });
    }
}

class Program 
{
    static void Main() 
    {
        var sessionFactory = new DbSessionFactory(SqliteFactory.Instance, "Data Source='database.db'");

        using (var session = sessionFactory.CreateSession())
        {
            var people = session.ExecuteQuery(new GetAllPeopleQuery());

            foreach (var person in people)
            {
                Console.WriteLine($"{person.Name} born on {person.Dob}");
            }
        }
    }
}
```
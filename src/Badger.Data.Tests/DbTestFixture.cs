using Badger.Data.Tests.Queries;
using Dapper;
using System;
using System.Data.Common;

namespace Badger.Data.Tests;

public abstract class DbTestFixture(DbProviderFactory providerFactory, QueryFactory queryFactory = null) : IDisposable
{
    public DbConnection Connection { get; private set; }
    public DbProviderFactory ProviderFactory { get; } = providerFactory;
    public abstract string ConnectionString { get; }
    protected string TestDatabase { get; } = "badgerdata" + Guid.NewGuid().ToString("N");
    public QueryFactory QueryFactory { get; } = queryFactory ?? new QueryFactory();

    public readonly Person TestPerson1 = new()
    {
        Name = "Bill",
        Dob = new DateTime(2000, 1, 1),
        Height = 180
    };
    public readonly Person TestPerson2 = new()
    {
        Name = "Ben",
        Dob = new DateTime(2001, 1, 1),
        Address = "1 Badger Row"
    };

    protected void InitTestDatabase()
    {
        CreateTestDatabase();
        OpenTestConnection();
        CreateTestTables();
        InsertTestData();
    }

    protected virtual void CreateTestDatabase() { }

    private void OpenTestConnection()
    {
        Connection = ProviderFactory.CreateConnection();
        Connection.ConnectionString = ConnectionString;
        Connection.Open();
    }
    protected abstract void CreateTestTables();

    public virtual ISessionFactory CreateSessionFactory()
    {
        return SessionFactory.With(config =>
            config.WithConnectionString(ConnectionString)
                  .WithProviderFactory(ProviderFactory));
    }

    protected void InsertTestData()
    {
        var insertSql = @"insert into people (name, dob, height, address) 
                              values (@Name, @Dob, @Height, @Address)";
        Connection.Execute(insertSql, TestPerson1);

        Connection.Execute(insertSql, TestPerson2);
    }
    public virtual void Dispose()
    {
        Connection.Dispose();
        DestroyTestDatabase();
    }

    protected abstract void DestroyTestDatabase();
}

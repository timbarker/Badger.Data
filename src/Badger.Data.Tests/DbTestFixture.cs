using Dapper;
using System;
using System.Data.Common;

namespace Badger.Data.Tests
{
    public abstract class DbTestFixture : IDisposable
    {
        public DbConnection Connection { get; private set;}
        public DbProviderFactory ProviderFactory { get; }
        public abstract string ConnectionString { get; }
        public string TestDatabase { get; }
        public readonly Person TestPerson1 = new Person 
            { 
                Name = "Bill", 
                Dob = new DateTime(2000, 1, 1),
                Height = 180
            };
        public readonly Person TestPerson2 = new Person 
            { 
                Name = "Ben", 
                Dob = new DateTime(2001, 1, 1),
                Address = "1 Badger Row"
            };

        protected DbTestFixture(DbProviderFactory providerFactory)
        {
            this.TestDatabase = "badgerdata" + Guid.NewGuid().ToString("N");

            this.ProviderFactory = providerFactory;
        }

        protected void InitTestDatabase()
        {
            CreateTestDatabase();
            OpenTestConnection();
            CreateTestTables();
            InsertTestData();
        }

        protected virtual void CreateTestDatabase() {}

        private void OpenTestConnection()
        {
            this.Connection = this.ProviderFactory.CreateConnection();
            this.Connection.ConnectionString = this.ConnectionString;
            this.Connection.Open();
        }
        protected abstract void CreateTestTables();

        protected void InsertTestData()
        {
            var insertSql = @"insert into people (name, dob, height, address) 
                              values (@Name, @Dob, @Height, @Address)";
            this.Connection.Execute(insertSql, TestPerson1);

            this.Connection.Execute(insertSql, TestPerson2);
        }
        public virtual void Dispose()
        {
            this.Connection.Dispose();
            DestroyTestDatabase();
        }

        protected abstract void DestroyTestDatabase();
    }
}

using System;

namespace Badger.Data.Postgres
{
    public class InsertResult<T>
    {
        public int RowsAffected { get; private set; }
        public T Id { get; private set; }
        public bool IdSuccessfullyCast { get; private set; }

        public static InsertResult<T> CreateResult(int rowsAffected, object id)
        {
            if (id is T idAsT)
            {
                return new InsertResult<T>
                {
                    Id = idAsT,
                    RowsAffected = rowsAffected,
                    IdSuccessfullyCast = true
                };
            }
            try
            {
                return new InsertResult<T>
                {
                    Id = (T)Convert.ChangeType(id, typeof(T)),
                    RowsAffected = rowsAffected,
                    IdSuccessfullyCast = true
                };
            }
            catch (InvalidCastException)
            {
                return new InsertResult<T>
                {
                    Id = default,
                    RowsAffected = rowsAffected
                };
            }
        }
    }
}
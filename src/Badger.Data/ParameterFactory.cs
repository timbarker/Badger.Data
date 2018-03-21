using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Badger.Data
{
    internal class ParameterFactory
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly Dictionary<Type, Action<object, DbParameter>> _parameterHandlers;

        public ParameterFactory(DbProviderFactory dbProviderFactory, IEnumerable<ParameterHandler> parameterHandlers)
        {
            _dbProviderFactory = dbProviderFactory;
            _parameterHandlers = parameterHandlers.ToDictionary(h => h.Type, h => h.Handle);
        }

        public DbParameter Create(string name, object value, int? size = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Paramter name must not be null or empty", nameof(name));

            var parameter = _dbProviderFactory.CreateParameter();
            parameter.ParameterName = name;

            if (_parameterHandlers.TryGetValue(value.GetType(), out var handler))
                handler.Invoke(value, parameter);
            else
                DefaultParameterHandler(value, parameter, size);

            return parameter;
        }

        public DbParameter CreateOutputParameter(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Paramter name must not be null or empty", nameof(name));

            var parameter = _dbProviderFactory.CreateParameter();
            parameter.ParameterName = name;
            parameter.Direction = ParameterDirection.Output;

            return parameter;
        }

        public DbParameter Create<T>(string name, IEnumerable<T> value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Paramter name must not be null or empty", nameof(name));

            var parameter = _dbProviderFactory.CreateParameter();
            parameter.ParameterName = name;

            if (_parameterHandlers.TryGetValue(typeof(IEnumerable<T>), out var handler))
                handler.Invoke(value, parameter);
            else
                DefaultParameterHandler(value, parameter);

            return parameter;
        }

        private static void DefaultParameterHandler(object value, DbParameter parameter, int? size = null)
        {
            parameter.Value = value;

            if (size.HasValue)
                parameter.Size = size.Value;
            else if (value is string s)
                parameter.Size = s.Length;
        }
    }
}

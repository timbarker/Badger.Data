using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Badger.Data
{
    sealed class ParameterHandler
    {
        public ParameterHandler(Type type, Action<object, DbParameter> handle)
        {
            Type = type;
            Handle = handle;
        }

        public Type Type { get; }

        public Action<object, DbParameter> Handle { get; }
    }

    public class Config
    {
        public Config WithConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            return this;
        }

        public Config WithProviderFactory(DbProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
            return this;
        }

        public Config WithParameterHandler<T>(Action<T, DbParameter> handler)
        {
            _parameterHandlers.Add(new ParameterHandler(typeof(T), (o, parameter) => handler.Invoke((T) o, parameter)));
            return this;
        }

        public Config WithTableParameterHandler<T>(Action<IEnumerable<T>, DbParameter> handler)
        {
            _parameterHandlers.Add(new ParameterHandler(typeof(IEnumerable<T>), (o, parameter) => handler.Invoke((IEnumerable<T>) o, parameter)));
            return this;
        }

        internal DbProviderFactory ProviderFactory { get; private set; }

        internal string ConnectionString { get; private set; } = string.Empty;

        private readonly List<ParameterHandler> _parameterHandlers = new List<ParameterHandler>();

        internal IEnumerable<ParameterHandler> ParameterHandlers => _parameterHandlers;
    }
}

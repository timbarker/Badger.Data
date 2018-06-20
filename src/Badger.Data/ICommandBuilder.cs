using System;
using System.Collections.Generic;

namespace Badger.Data
{
    /// <summary>
    /// Builder to create a PreparedCommand
    /// </summary>
    public interface ICommandBuilder
    {
        /// <summary>
        /// Sets the SQL that will be executed.
        /// </summary>
        /// <param name="sql">the SQL text.</param>
        ICommandBuilder WithSql(string sql);

        /// <summary>
        /// Sets a command parameter.
        /// </summary>
        /// <param name="name">the name of the command parameter.</param>
        /// <param name="value">the parameter value.</param>
        ICommandBuilder WithParameter<T>(string name, T value);

        ICommandBuilder WithTableParameter<T>(string name, IEnumerable<T> value);

        /// <summary>
        /// Sets a string command parameter with a length.
        /// </summary>
        /// <param name="name">the name of the query parameter.</param>
        /// <param name="value">the parameter value.</param>
        /// <param name="length">the max length of the string.</param>
        ICommandBuilder WithParameter(string name, string value, int length);

        /// <summary>
        /// Specifies a command timeout for the query
        /// </summary>
        /// <param name="timeout">the desired timeout.</param>
        /// <returns></returns>
        ICommandBuilder WithTimeout(TimeSpan timeout);

        /// <summary>
        /// Builds the prepared command.
        /// </summary>
        IPreparedCommand Build();
    }
}

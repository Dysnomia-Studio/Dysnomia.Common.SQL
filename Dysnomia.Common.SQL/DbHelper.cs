﻿/*
	MIT License

	Copyright (c) 2019 Axel "Elanis" Soupé

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dysnomia.Common.SQL {
	public static class DbHelper {
		private static void BindParameters(this IDbCommand command, Dictionary<string, object> parametersData) {
			if (parametersData != null) {
				foreach (KeyValuePair<string, object> kvp in parametersData) {
					if (kvp.Value is IDataParameter) {
						command.Parameters.Add(kvp.Value);
					} else {
						var parameter = command.CreateParameter();
						parameter.ParameterName = kvp.Key;
						parameter.Value = kvp.Value;

						command.Parameters.Add(parameter);
					}
				}
			}
		}

		private static void OpenConnection(this IDbConnection connection) {
			if (connection.State == ConnectionState.Broken) {
				connection.Close();
			}

			if (connection.State == ConnectionState.Closed) {
				connection.Open();
			}
		}

		/// <summary>
		/// Execute SQL stored procedure with reader as a result
		/// </summary>
		/// <param name="connection">Database connection</param>
		/// <param name="procName">Procedure to execute</param>
		/// <param name="parameters">Statement parameters</param>
		/// <param name="transaction">(Optional) Transaction this statement should be in</param>
		/// <returns></returns>
		public async static Task<IDataReader> ExecStoredProcedure(this IDbConnection connection, string procName, Dictionary<string, object> parameters = null, IDbTransaction transaction = null, int timeout = 30) {
			using (IDbCommand command = connection.CreateCommand()) {
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = procName;
				command.CommandTimeout = timeout;

				if (transaction != null) {
					command.Transaction = transaction;
				}

				command.Connection.OpenConnection();

				command.BindParameters(parameters);

				return await Task.Run(() => command.ExecuteReader());
			}
		}

		/// <summary>
		/// Execute SQL query with reader as a result
		/// </summary>
		/// <param name="connection">Database connection</param>
		/// <param name="sqlStatement">Statement to execute</param>
		/// <param name="parameters">Statement parameters</param>
		/// <param name="transaction">(Optional) Transaction this statement should be in</param>
		/// <returns></returns>
		public async static Task<IDataReader> ExecuteQuery(this IDbConnection connection, string sqlStatement, Dictionary<string, object> parameters = null, IDbTransaction transaction = null, int timeout = 30) {
			using (IDbCommand command = connection.CreateCommand()) {
				command.CommandType = CommandType.Text;
				command.CommandText = sqlStatement;
				command.CommandTimeout = timeout;

				if (transaction != null) {
					command.Transaction = transaction;
				}

				command.Connection.OpenConnection();

				command.BindParameters(parameters);

				return await Task.Run(() => command.ExecuteReader());
			}
		}

		/// <summary>
		/// Execute SQL query without any result returned
		/// </summary>
		/// <param name="connection">Database connection</param>
		/// <param name="sqlStatement">Statement to execute</param>
		/// <param name="parameters">Statement parameters</param>
		/// <param name="transaction">(Optional) Transaction this statement should be in</param>
		/// <returns></returns>
		public async static Task<int> ExecuteNonQuery(this IDbConnection connection, string sqlStatement, Dictionary<string, object> parameters = null, IDbTransaction transaction = null, int timeout = 30) {
			using (IDbCommand command = connection.CreateCommand()) {
				command.CommandType = CommandType.Text;
				command.CommandText = sqlStatement;
				command.CommandTimeout = timeout;

				if (transaction != null) {
					command.Transaction = transaction;
				}

				command.Connection.OpenConnection();

				command.BindParameters(parameters);

				return await Task.Run(() => command.ExecuteNonQuery());
			}
		}
	}
}

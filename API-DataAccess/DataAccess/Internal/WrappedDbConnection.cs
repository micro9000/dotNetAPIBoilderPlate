using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_DataAccess.Internal
{
    public class WrappedDbConnection : IDbConnection
    {
		private readonly IDbConnection _conn;
		public WrappedDbConnection(IDbConnection connection)
		{
			if (connection == null)
				throw new ArgumentNullException(nameof(connection)); // 

			_conn = connection;
		}

		public string ConnectionString
		{
			get
			{
				return _conn.ConnectionString;
			}
			set
			{
				_conn.ConnectionString = value;
			}
		}

		public int ConnectionTimeout
		{
			get
			{
				return _conn.ConnectionTimeout;
			}
		}

		public string Database
		{
			get
			{
				return _conn.Database;
			}
		}

		public ConnectionState State
		{
			get
			{
				return _conn.State;
			}
		}

		public IDbTransaction BeginTransaction()
		{
			return _conn.BeginTransaction();
		}

		public IDbTransaction BeginTransaction(IsolationLevel il)
		{
			return _conn.BeginTransaction(il);
		}

		public void ChangeDatabase(string databaseName)
		{
			_conn.ChangeDatabase(databaseName);
		}

		public void Close()
		{
			_conn.Close();
		}

		public IDbCommand CreateCommand()
		{
			return new WrappedDbCommand(_conn.CreateCommand());
		}

		public void Dispose()
		{
			_conn.Dispose();
		}

		public void Open()
		{
			_conn.Open();
		}
	}
}

using Burgoo.MetaBase.Objects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.ComponentModel;

namespace Burgoo.MetaBase 
{
	public class SQLiteMetaProvider : MetaProviderBase
	{
		//
		// connection used while object remains alive
		// when in memory is used, when the connection is closed
		// that in memory database is destroyed
		//
		private SQLiteConnection conn;

		public override void Open()
		{
			conn = new SQLiteConnection(this._ConnectionString);
			conn.Open();
			CreateSchema(); 
		}

		public override void Close()
		{
			conn.Close();
		}

		public override MetaAttributeList FindAttributes(string Name, string Value, string Schema = "")
		{
			var rtrn = new MetaAttributeList();

			using (var comm = conn.CreateCommand())
			{
				var sql = string.Format("select id, entity_id, name, value from {0}attribute where name = @name and value = @value;", Schema);

				comm.CommandText = sql.ToString();

				comm.Parameters.AddWithValue("@name", Name );
				comm.Parameters.AddWithValue("@value", Value) ;

				var rdr = comm.ExecuteReader();

				while (rdr.Read())
				{
					rtrn.Add
					(
						rdr.GetGuid(0)
						,
						new MetaAttribute() 
						{
							ID = rdr.GetGuid(0)
							, EntityID = rdr.GetGuid(1)
							, Name = rdr.GetString(2)
							, Value = rdr.GetString(3)
						}
					);
				}
			}

			return rtrn;
		}


		public override GuidList GetEntities(string TypeName, string Schema)
		{
			var rtrn = new GuidList();

			using (var comm = conn.CreateCommand())
			{
				var sql = string.Format("select id, type from {0}entity where type = @type;", Schema);

				comm.CommandText = sql.ToString();
				comm.Parameters.AddWithValue("@type", TypeName);

				var rdr = comm.ExecuteReader();  

				while (rdr.Read())
				{
					rtrn.Add( rdr.GetGuid(0) );
				}
			}

			return rtrn;
		}


		public override MetaAttributeList GetAttributes(GuidList EntityIDs, string Schema)
		{
			var rtrn = new MetaAttributeList();

			using (var comm = conn.CreateCommand())
			{
				// where entity_id in ({1})
				var sql = string.Format("select id, entity_id, name, value from {0}attribute where entity_id in ({1}) ;", Schema, EntityIDs.ToStringOfBinary());
				comm.CommandText = sql.ToString();

				Trace(comm);
	
				
				var rdr = comm.ExecuteReader();

				while (rdr.Read())
				{
					rtrn.Add
					(
						rdr.GetGuid(0)
						,
						new MetaAttribute() 
						{
							ID = rdr.GetGuid(0)
							,EntityID = rdr.GetGuid(1)
							, Name = rdr.GetString (2)   
							, Value = rdr["value"]
						}
					);
				}
			}

			return rtrn;
		}






		public override Guid CreateAttribute<t>(Guid EntityID, string Name, t Value, string Schema = "")
		{
			var sql = new StringBuilder();
			var guid = Guid.NewGuid();
			sql.AppendLine(string.Format("insert into {0}attribute (id, entity_id, name, value) values(@id, @entity_id, @name, @value);", Schema));

			using (var comm = conn.CreateCommand())
			{
				comm.Parameters.AddWithValue("@id", guid.ToString("N") );
				comm.Parameters.AddWithValue("@entity_id", EntityID.ToString("N") );
				comm.Parameters.AddWithValue("@name", Name);
				comm.Parameters.AddWithValue("@value", Value);
				comm.CommandText = sql.ToString();

				Trace(comm);

				comm.ExecuteNonQuery();

				// PrintTables(Schema);
			}

			return guid;
		}


		public override Guid CreateEntity(string TypeName, string Schema = "") 
		{
			var sql = new StringBuilder();
			var guid = Guid.NewGuid();
			sql.AppendLine(string.Format("insert into {0}entity (id, type) values(@id, @type);", Schema));

			using (var comm = conn.CreateCommand())
			{
				comm.Parameters.AddWithValue("@id", guid.ToString("N") );
				comm.Parameters.AddWithValue("@type", TypeName);
				comm.CommandText = sql.ToString();

				Trace(comm);

				comm.ExecuteNonQuery();
			}

			return guid;
		}


		public override void CreateSchema(string Schema = "")
		{
			using (var comm = conn.CreateCommand())
			{
				var sql = new StringBuilder();
					
				sql.AppendLine(string.Format("create table {0}entity (id blob(16) not null primary key, type varchar(100));", Schema));
				sql.AppendLine(string.Format("create table {0}attribute (id blob(16) not null primary key, entity_id blob(16), name varchar(100), value varchar(100));", Schema));


				comm.CommandText = sql.ToString();

				Trace(comm);
				
				var rtrn = comm.ExecuteNonQuery();

			}
		}


		private void Trace(string Message)
		{
			System.Diagnostics.Trace.WriteLine(Message, "SQL");
		}


		private void Trace(SQLiteCommand comm)
		{
			Trace(comm.CommandText);

			foreach (SQLiteParameter i in comm.Parameters)
			{
				Trace(string.Format("{0} = {1}", i.ParameterName, i.Value));
			}
		}

		public override DataSet ExportTables(string Schema)
		{
			using (var comm = conn.CreateCommand())
			{
				var sql = new StringBuilder();

				sql.AppendLine(string.Format("select * from {0}entity;select * From attribute{0}", Schema));
				comm.CommandText = sql.ToString();
				var rtrn = comm.ExecuteNonQuery();

				var da = new SQLiteDataAdapter(comm);
				var ds = new DataSet();

				da.Fill(ds);

				return ds;
			}
		}



		public T GetValue<T>(System.Data.IDataReader theReader, string theColumnName)
		{
			//
			// Read the value out of the reader by string (column name); returns object
			object theValue = theReader[theColumnName];

			// Cast to the generic type applied to this method (i.e. int?)
			Type theValueType = typeof(T);

			// Check for null value from the database
			if (System.DBNull.Value != theValue)
			{
				// We have a null, do we have a nullable type for T?
				if (!IsNullableType(theValueType))
				{

					if (theValueType == typeof(Guid))
					{
						if (theValue.GetType()  == typeof(byte[]))
						{
							var a = (T)Convert.ChangeType(theValue, typeof(T));


						}
						{
							throw new System.Exception("Invalid Guid");
						}

					}
					else
					{
						return (T)Convert.ChangeType(theValue, theValueType);
					}

				}
				else
				{
					// Yes, this is a nullable type so change the value's type from object to the underlying type of T
					NullableConverter theNullableConverter = new NullableConverter(theValueType);

					return (T)Convert.ChangeType(theValue, theNullableConverter.UnderlyingType);
				}
			}

			// The value was null in the database, so return the default value for T; this will vary based on what T is (i.e. int has a default of 0)
			return default(T);
		}

		private bool IsNullableType(Type theValueType)
		{
			return (theValueType.IsGenericType && theValueType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
		}

		//
		// properties
		//
		private string _ConnectionString { get; set; }
		private string _ApplicationName { get; set; }

		public string ConnectionStringName
		{
			get 
			{
				return _ConnectionString;
			}
			set 
			{
				_ConnectionString = value;
			}
		}


		public override string ApplicationName
		{
			get
			{
				return _ApplicationName;
			}
			set
			{
				_ApplicationName = value;
			}

		}


		public override void Initialize(string name, NameValueCollection config)
		{
			// Verify that config isn't null
			if (config == null)
				throw new ArgumentNullException("config");

			// Assign the provider a default name if it doesn't have one
			if (string.IsNullOrEmpty(name))
				name = "Generic Data Provider";

			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", "SQLite Meta Data Provider.");
			}

			// Call the base class's Initialize method
			base.Initialize(name, config);

			// Initialize _applicationName
			ApplicationName = config["applicationName"];

			if (string.IsNullOrEmpty(ApplicationName))
				ApplicationName = "/";

			config.Remove("applicationName");

			// Initialize _connectionString
			string connect = config["connectionStringName"];

			if (string.IsNullOrEmpty(connect))
				throw new ProviderException("Empty or missing connectionStringName");

			config.Remove("connectionStringName");


			ConnectionStringSettings conn = null/* TODO Change to default(_) if this is not a reference type */;

			foreach (ConnectionStringSettings i in ConfigurationManager.ConnectionStrings)
			{
				if (string.Compare(i.Name, connect, true) == 0)
				{
					conn = i;
				}
			}

			if (conn == null)
				throw new ProviderException(string.Format("Missing connection string, {0}", connect));

			_ConnectionString = conn.ConnectionString;

			if (string.IsNullOrEmpty(_ConnectionString))
				throw new ProviderException("Empty connection string");

			// Throw an exception if unrecognized attributes remain
			if (config.Count > 0)
			{
				string attr = config.GetKey(0);
				if ((!string.IsNullOrEmpty(attr)))
					throw new ProviderException("Unrecognized attribute: " + attr);
			}
		}





	}
}

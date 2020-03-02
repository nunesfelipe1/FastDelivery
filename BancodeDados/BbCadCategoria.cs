using Android.Database.Sqlite;
using System.IO;

namespace Menu
{
	class DBCadCategoria 
	{
		private SQLiteDatabase sqldb;
		private string sqldb_query;
		private string sqldb_message;
		private bool sqldb_available;

		public DBCadCategoria()
		{
			sqldb_message = "";
			sqldb_available = false;
		}

		public DBCadCategoria(string sqldb_name)
		{
			try
			{
				sqldb_message = "";
				sqldb_available = false;
				CreateDatabase(sqldb_name);
			}
			catch (SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		public bool DatabaseAvailable
		{
			get{ return sqldb_available; }
			set{ sqldb_available = value; }
		}

		public string Message
		{
			get{ return sqldb_message; }
			set{ sqldb_message = value; }
		}

		public void CreateDatabase(string sqldb_name)
		{
			try
			{
				sqldb_message = "";
				string sqldb_location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				string sqldb_path = Path.Combine(sqldb_location, sqldb_name);
				bool sqldb_exists = File.Exists(sqldb_path);

				//if(!sqldb_exists)
				//{

				sqldb = SQLiteDatabase.OpenOrCreateDatabase(sqldb_path,null);

				//sqldb_query = "DROP TABLE categoria;";
				//sqldb.ExecSQL(sqldb_query);
				//sqldb_message = "Database: " + sqldb_name + " created";

				sqldb_query = "CREATE TABLE IF NOT EXISTS categoria (_id INTEGER PRIMARY KEY, descricao VARCHAR, " +
					"ativo VARCHAR, imagem BLOB);";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Database: " + sqldb_name + " created";
				//}
				//else
				//{
					sqldb = SQLiteDatabase.OpenDatabase(sqldb_path, null, DatabaseOpenFlags.OpenReadwrite);
					sqldb_message = "Database: " + sqldb_name + " opened";
				//}
				sqldb_available=true;
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		//ADICIONA PRODUTO
		public void AddRecord(int iId, string sDescricao, string sAtivo)
		{
			try
			{
				sqldb_query = "INSERT INTO categoria (_id, descricao, ativo) VALUES ('" + iId + "','" + sDescricao + "','" + sAtivo + "');";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record saved";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		//ALTERA PRODUTO
		public void UpdateRecord(int iId, string sDescricao, string sAtivo)
		{
			try
			{
				sqldb_query="UPDATE categoria SET descricao ='" + sDescricao + "', ativo ='" + sAtivo + "' WHERE _id ='" + iId + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record " + iId + " updated";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}

		//DELETA PRODUTO
		public void DeleteRecord(int iId)
		{
			try
			{
				sqldb_query = "DELETE FROM categoria WHERE _id ='" + iId + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record " + iId + " deleted";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		public void DeleteRecord()
		{
			try
			{
				sqldb_query = "DELETE FROM categoria;";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record deleted";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		//SELECT NA TABELA SEM CAMPOS...SELECT DIRETO
		public Android.Database.ICursor GetRecordCursor()
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT _id, descricao, ativo, imagem FROM categoria;";
				sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
				if(!(sqldb_cursor != null))
				{
					sqldb_message = "Record not found";
				}
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}

		public Android.Database.ICursor GetRecordCursor(int iId)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT _id, descricao, ativo, imagem FROM categoria WHERE _id = '" + iId + "';";
				sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
				if(!(sqldb_cursor != null))
				{
					sqldb_message = "Record not found";
				}
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}

		//SELECT NA TABELA PASSANDO COLUNA E VALOR
		public Android.Database.ICursor GetRecordCursor(string sDescricao, string sValue)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT _id, descricao, ativo, imagem FROM categoria WHERE " + sDescricao + " LIKE '" + sValue + "%';";
				sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
				if(!(sqldb_cursor != null))
				{
					sqldb_message = "Record not found";
				}
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}

	}
}
using Android.Database.Sqlite;
using System.IO;

namespace Menu
{
	class DBCadPedido
	{
		private SQLiteDatabase sqldb;
		private string sqldb_query;
		private string sqldb_message;
		private bool sqldb_available;

		public DBCadPedido()
		{
			sqldb_message = "";
			sqldb_available = false;
		}

		public DBCadPedido(string sqldb_name)
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
				sqldb = SQLiteDatabase.OpenOrCreateDatabase(sqldb_path, null);

				//sqldb_query = "DROP TABLE pedido;";
				//sqldb.ExecSQL(sqldb_query);
				//sqldb_message = "Database: " + sqldb_name + " created";

				sqldb_query = "CREATE TABLE IF NOT EXISTS pedido (_codPedido INTEGER, _codEmpresa INTEGER, numMesa INTEGER, dataPedido VARCHAR,  " + 
					" valorTotal VARCHAR, numSerial VARCHAR, numCelular VARCHAR, dataFechado TEXT, codigoMesa VARCHAR);";
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
				sqldb_message = "erro";
			}
		}

		//ADICIONA PEDIDO
		public void AddRecord(int iCodPedido, int iCodEmpresa, int iNumMesa, string sDataPedido, string dValorTotal, string sNumSerial, string sNumCelular, string sCodigoMesa)
		{
			try
			{
				sqldb_query = "INSERT INTO pedido (_codPedido, _codEmpresa, numMesa, dataPedido, valorTotal, numSerial, numCelular, codigoMesa) VALUES ('" + 
					iCodPedido + "','" + iCodEmpresa + "','" + iNumMesa + "','" + sDataPedido + "','" + dValorTotal + "','" + sNumSerial + "','" + sNumCelular + "','" + sCodigoMesa + "');";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "ok";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
				sqldb_message = "erro";
			}
		}

		//ALTERA PEDIDO
		public void UpdateRecord(int codEmpresa, int codPedido, int numMesa, string valorTotal)
		{
			try
			{
				sqldb_query="UPDATE pedido SET valorTotal = valorTotal + '" + valorTotal + 											   
					"' WHERE _codEmpresa ='" + codEmpresa + "' and _codPedido = '" + codPedido + "' and numMesa = '" + numMesa + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "ok";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}

		public void UpdateRecord(string sSQL)
		{
			try
			{
				sqldb_query = sSQL + ";";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "ok";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}

		//DELETA PEDIDO		por passagem de parametro
		public void DeleteRecord(string sSQL)
		{
			try
			{
				sqldb_query = sSQL+ ";";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record deleted";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		//DELETA tabela inteira
		public void DeleteRecord()
		{
			try
			{
				sqldb_query = "DELETE FROM pedido;";
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
				sqldb_query = "SELECT _codPedido, _codEmpresa numMesa, dataPedido, valorTotal, numSerial, numCelular FROM pedido;";
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

		//SELECT NA TABELA SEM CAMPOS...SELECT DIRETO
		public Android.Database.ICursor GetRecordCursor(string sSQL)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = sSQL + ";";
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

		//SELECT NA TABELA produto buscando pelas chaves IdCategoria, CodProduto
		public Android.Database.ICursor GetRecordCursor(int iCodPedido, int iCodEmpresa)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT _codPedido, _codEmpresa, numMesa, dataPedido, valorTotal, numSerial, numCelular " + 
					" FROM pedido WHERE _codPedido ='" + iCodPedido + "' and _codEmpresa = '" + iCodEmpresa + "';";
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
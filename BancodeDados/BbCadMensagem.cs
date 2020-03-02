using Android.Database.Sqlite;
using System.IO;

namespace Menu
{
	class DBCadMensagem
	{
		private SQLiteDatabase sqldb;
		private string sqldb_query;
		private string sqldb_message;
		private bool sqldb_available;

		public DBCadMensagem()
		{
			sqldb_message = "";
			sqldb_available = false;
		}

		public DBCadMensagem(string sqldb_name)
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


				sqldb = SQLiteDatabase.OpenOrCreateDatabase(sqldb_path, null);

				//sqldb_query = "DROP TABLE carrinho;";
				//sqldb.ExecSQL(sqldb_query);
				//sqldb_message = "Database: " + sqldb_name + " created";

				sqldb_query = "CREATE TABLE IF NOT EXISTS mensagem (_codEmpresa INTEGER, _codMensagem INTEGER,  " + 
					" titulo VARCHAR, mensagem VARCHAR, data VARCHAR, subtitulo VARCHAR);";
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
		public void AddRecord(int iCodEmpresa, int iCodMensagem, string sTitulo, string sMensagem, string sData, string subtitulo)
		{
			try
			{
				sqldb_query = "INSERT INTO mensagem (_codEmpresa, _codMensagem, titulo, mensagem, data, subtitulo) VALUES ('" + 
					iCodEmpresa + "','" + iCodMensagem + "','" + sTitulo + "','" + sMensagem + "','" + sData + "','" + subtitulo + "');";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record saved";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
				sqldb_message = "erro";
			}
		}

		//ALTERA PEDIDO
		public void UpdateRecord(int codEmpresa, int numMesa, int codPedido, double valorTotal)
		{
			try
			{
				/*sqldb_query="UPDATE carrinho SET valorTotal ='" + valorTotal + 											   
					"' WHERE _codEmpresa ='" + codEmpresa + "' and _codPedido = '" + codPedido + "' and numMesa = '" + numMesa + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record updated";*/
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
				sqldb_query = "DELETE FROM mensagem;";
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
				sqldb_query = "SELECT _codEmpresa, _codMensagem, titulo, mensagem, data, subtitulo FROM mensagem;";
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
		public Android.Database.ICursor GetRecordCursor(int iCodEmpresa, int iCodMensagem)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT _codEmpresa, _codMensagem, titulo, mensagem, data, subtitulo FROM mensagem " + 
					" WHERE _codEmpresa ='" + iCodEmpresa + "' and _codMensagem = '" + iCodMensagem + "';";
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
using Android.Database.Sqlite;
using System.IO;

namespace Menu
{
	class DBCadCarrinhoItem
	{
		private SQLiteDatabase sqldb;
		private string sqldb_query;
		private string sqldb_message;
		private bool sqldb_available;

		public DBCadCarrinhoItem()
		{
			sqldb_message = "";
			sqldb_available = false;
		}

		public DBCadCarrinhoItem(string sqldb_name)
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

				//sqldb_query = "DROP TABLE carrinho_item;";
				//s/qldb.ExecSQL(sqldb_query);
				//sqldb_message = "Database: " + sqldb_name + " created";

				sqldb_query = "CREATE TABLE IF NOT EXISTS carrinho_item (_codEmpresa INTEGER, _codpedido INTEGER, _itemPedido INTEGER, _codProduto INTEGER,  " + 
					"qtde REAL, precoUnit VARCHAR, numSerial VARCHAR, numCelular VARCHAR, lembrete VARCHAR, enviado VARCHAR, PRIMARY KEY (_codpedido, _codProduto, _itemPedido, _codEmpresa ));";
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
		public void AddRecord(int iCodEmpresa, int iCodpedido, int iItemPedido, int iCodProduto, double dQtde, string dPrecoUnit, string sNumSerial, string sNumCelular, string sLembrete, string sEnviado)
		{
			try         
			{
				sqldb_query = "INSERT INTO carrinho_item (_codEmpresa, _codpedido, _itemPedido, _codProduto, qtde, precoUnit, numSerial, numCelular, lembrete, enviado) VALUES ('" + 
					iCodEmpresa + "','" + iCodpedido + "','" + iItemPedido + "','" + iCodProduto + "','" + dQtde + "','" + 
					dPrecoUnit + "','" + sNumSerial + "','" + sNumCelular + "','" + sLembrete + "','" + sEnviado + "');";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "OK";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		//ALTERA PRODUTO
		public void UpdateRecord(int codEmpresa, int codpedido)
		{
			try
			{
				sqldb_query="UPDATE carrinho_item SET enviado ='S'"	+
					" WHERE _codEmpresa ='" + codEmpresa + "' and _codpedido = '" + codpedido + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "ok";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
				sqldb_message = "erro";
			}
		}

		//DELETA PRODUTO por passagem de parametro
		public void DeleteRecord(string sSQL)
		{
			try
			{
				sqldb_query = sSQL + ";";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Records deleted";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		//SELECT NA TABELA SEM CAMPOS...SELECT DIRETO
		public Android.Database.ICursor GetRecordCursor(string sSQL)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = sSQL;
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
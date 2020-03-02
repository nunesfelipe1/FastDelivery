using Android.Database.Sqlite;
using System.IO;

namespace Menu
{
	class DBCadPedidoItem
	{
		private SQLiteDatabase sqldb;
		private string sqldb_query;
		private string sqldb_message;
		private bool sqldb_available;

		public DBCadPedidoItem()
		{
			sqldb_message = "";
			sqldb_available = false;
		}

		public DBCadPedidoItem(string sqldb_name)
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

				//sqldb_query = "DROP TABLE pedido_item;";
				//sqldb.ExecSQL(sqldb_query);
				//sqldb_message = "Database: " + sqldb_name + " created";

				sqldb_query = "CREATE TABLE IF NOT EXISTS pedido_item (_codEmpresa INTEGER, _codpedido INTEGER, _itemPedido INTEGER, _codProduto INTEGER,  " + 
					"qtde REAL, precoUnit REAL, numSerial VARCHAR, numCelular VARCHAR, lembrete VARCHAR);";
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
		public void AddRecord(int iCodEmpresa, int iCodpedido, int iItemPedido, int iCodProduto, double dQtde, double dPrecoUnit, string sNumSerial, string sNumCelular, string sLembrete)
		{
			try         
			{
				sqldb_query = "INSERT INTO pedido_item (_codEmpresa, _codpedido, _itemPedido, _codProduto, qtde, precoUnit, numSerial, numCelular, lembrete) VALUES ('" + 
					iCodEmpresa + "','" + iCodpedido + "','" + iItemPedido + "','" + iCodProduto + "','" + dQtde + "','" + 
					dPrecoUnit + "','" + sNumSerial + "','" + sNumCelular + "','" + sLembrete + "');";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "ok";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		//ALTERA PRODUTO
		public void UpdateRecord(int iIdCategoria, int iCodProduto, string sDescricao, string sAtivo, string sDescricaoItens, double dPrecoUnitario)
		{
			try
			{
				sqldb_query="UPDATE pedido_item SET descricao ='" + sDescricao + 
											   "', sAtivo ='" + sAtivo + 
											   "', descricaoItens ='" + sDescricaoItens + 
											   "', precoUnitario ='" + dPrecoUnitario + 
					"' WHERE _idCategoria ='" + iIdCategoria + "' and _codProduto = '" + iCodProduto + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record " + iCodProduto + " updated";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
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
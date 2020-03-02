using Android.Database.Sqlite;
using System.IO;

namespace Menu
{
	class DBCadProduto
	{
		private SQLiteDatabase sqldb;
		private string sqldb_query;
		private string sqldb_message;
		private bool sqldb_available;

		public DBCadProduto()
		{
			sqldb_message = "";
			sqldb_available = false;
		}

		public DBCadProduto(string sqldb_name)
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

				//sqldb_query = "DROP TABLE produto;";
				//sqldb.ExecSQL(sqldb_query);
				//sqldb_message = "Database: " + sqldb_name + " created";

				sqldb_query = "CREATE TABLE IF NOT EXISTS produto (_idCategoria INTEGER, _codProduto INTEGER PRIMARY KEY, " + 
					"descricao VARCHAR, ativo VARCHAR, descricaoItens VARCHAR, precoUnitario VARCHAR);";
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
		public void AddRecord(int iIdCategoria, int iCodProduto, string sDescricao, string sAtivo, string sDescricaoItens, string dPrecoUnitario)
		{
			try
			{
				sqldb_query = "INSERT INTO produto (_idCategoria, _codProduto, descricao, ativo, descricaoItens, precoUnitario) VALUES ('" + 
					iIdCategoria + "','" + iCodProduto + "','" + sDescricao + "','" + sAtivo + "','" + sDescricaoItens + "','" + dPrecoUnitario + "');";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record saved";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}

		//ALTERA PRODUTO
		public void UpdateRecord(int iIdCategoria, int iCodProduto, string sDescricao, string sAtivo, string sDescricaoItens, string dPrecoUnitario)
		{
			try
			{
				sqldb_query="UPDATE produto SET descricao ='" + sDescricao + 
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
		public void DeleteRecord(int iIdCategoria, int iCodProduto)
		{
			try
			{
				sqldb_query = "DELETE FROM produto WHERE _idCategoria ='" + iIdCategoria + "' and _codProduto = '" + iCodProduto + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record " + iCodProduto + " deleted";
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
				sqldb_query = "DELETE FROM produto;";
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
				sqldb_query = "SELECT _idCategoria, _codProduto, descricao, ativo, descricaoItens, precoUnitario FROM produto;";
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
		public Android.Database.ICursor GetRecordCursor(int iIdCategoria, int iCodProduto)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT _idCategoria, _codProduto, descricao, ativo, descricaoItens, precoUnitario FROM produto WHERE _idCategoria ='" + iIdCategoria + "' and _codProduto = '" + iCodProduto + "';";
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

		//SELECT NA TABELA de produto buscando somente o produto com idCategoria
		public Android.Database.ICursor GetRecordCursor(int iIdCategoria)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT _idCategoria, _codProduto, descricao, ativo, descricaoItens, precoUnitario FROM produto WHERE _idCategoria ='" + iIdCategoria + "';";
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
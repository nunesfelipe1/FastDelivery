using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Database.Sqlite;
using System.Collections.Generic;
using System.Linq;

namespace Menu
{
	[Activity (Label = "Conexao", MainLauncher = true)]
	public class CardLoja : Activity
	{
		List<ListaCategoria> categoria = new List<ListaCategoria> ();
		DBCadCategoria sqldb;
		TextView txtTeste;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.frmCardLoja);

			//aqui ira trocar pelo webservice, ou seja, assim que retornar do retaguarda o sistema irá preencher uma lista
			//e gravar no banco as categorias retornadas
			//categoria = preencheLista ();

			sqldb = new DBCadCategoria("categoria_db");

			ListView lwCategoria = FindViewById<ListView> (Resource.Id.lwCategoria);
			ImageButton btAnt = FindViewById<ImageButton> (Resource.Id.btAnt);
			txtTeste = FindViewById<TextView> (Resource.Id.txtTeste);

			//Deleta todas as categorias para inserir as categorias atualizadas pela webservice
			sqldb.DeleteRecord ();

			//grava no banco de dados e atribui ao objeto listacategoria
			//procurar uma forma de atribuir direto
			categoria = gravaCategoria ();

			lwCategoria.Adapter = new adapter_listview (this, categoria);
			lwCategoria.ItemClick += OnListItemClick;

			btAnt.Click += new EventHandler (btAnt_Click);
		}

		void btAnt_Click(object sender, EventArgs e){
			StartActivity (typeof(InfLoja));
		}

		void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
		{
			var listView = sender as ListView;
			var l = categoria [e.Position];
		
			MainActivity.id_categoria = Convert.ToInt32(l.codigo);

			if (listView != null)
			{
				StartActivity(typeof(CardItemLoja));
			}
		}

		private List<ListaCategoria> gravaCategoria(){

			Android.Database.ICursor sql_cursor = null;
			List<ListaCategoria> lsCategoria = new List<ListaCategoria> ();

			//grava as categorias no banco depois de receber o webservice
			sqldb.AddRecord (1, "CAIXA", "S");
			sqldb.AddRecord (2, "FISCAL", "S");
			sqldb.AddRecord (3, "FINANCEIRO", "S");
			sqldb.AddRecord (4, "COMPRAS", "S");
			sqldb.AddRecord (5, "TI", "S");
			sqldb.AddRecord (6, "PCP", "S");
			sqldb.AddRecord (7, "ADMINISTRATIVO", "S");
			sqldb.AddRecord (8, "COMERCIAL", "S");
			sqldb.AddRecord (9, "PRODUÇAO", "S");
			sqldb.AddRecord (10, "GERÊNCIA", "S");

			//percorrendo o retorno do select pelo getrecordcursor e retornando um objeto listacategoria
			sql_cursor = sqldb.GetRecordCursor ();

			if (sql_cursor != null) {
				sql_cursor.MoveToFirst ();

				while (sql_cursor.Position < sql_cursor.Count){
					lsCategoria.Add (insCategoria (	sql_cursor.GetInt (0), 
													sql_cursor.GetString (1).ToString (), 
													sql_cursor.GetString (2).ToString ()));

					sql_cursor.MoveToNext();
				}
			}

			return lsCategoria;
		}

		private ListaCategoria insCategoria(int codigo, string descricao, string ativo){

			ListaCategoria lstCategoria = new ListaCategoria (codigo, descricao, ativo);
			return lstCategoria;
		}
	}
}



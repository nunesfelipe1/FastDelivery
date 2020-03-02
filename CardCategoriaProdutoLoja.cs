using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Linq;

namespace Menu
{
	[Activity (Label = "FastDelivery", MainLauncher = false)]
	public class CardCategoriaProdutoLoja : Activity
	{
		List<ListaCategoriaProduto> item_categoria = new List<ListaCategoriaProduto> ();
		DBCadProduto sqldb;

		public override bool OnKeyDown(Keycode keycode, KeyEvent @event)
		{
			bool handled = false;

			try{
				Finish();
				StartActivity (typeof(CardCategoriaLoja));
				handled = true;
			}
			catch {
				handled = false;
			}
			return handled || base.OnKeyDown (keycode, @event);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.frmCardCategoriaProdutoLoja);

			//item_categoria = preencheLista ();

			sqldb = new DBCadProduto("delivery_db");

			ListView lwItemCategoria = FindViewById<ListView> (Resource.Id.lwItemCategoria);
			ImageButton btAnt = FindViewById<ImageButton> (Resource.Id.btAnt);
			ImageButton btMenu = FindViewById<ImageButton> (Resource.Id.btMenu);
			ImageButton btCarrinho = FindViewById<ImageButton> (Resource.Id.imgBtCarrinho);
			TextView txtLoja = FindViewById<TextView> (Resource.Id.txtLoja);

			item_categoria = carregProduto ();

			lwItemCategoria.Adapter = new adapter_listitem (this, item_categoria);
			lwItemCategoria.ItemClick += OnListItemClick;

			btAnt.Click += new EventHandler (btAnt_Click);
			btMenu.Click += new EventHandler (btMenu_Click);
			btCarrinho.Click += new EventHandler (btCarrinho_Click);
			txtLoja.Text = MainActivity.sNomeEmpresa;
		}

		void btCarrinho_Click(object sender, EventArgs e){
			Finish();
			StartActivity (typeof(Carrinho));
		}

		void btAnt_Click(object sender, EventArgs e){
			Finish();
			StartActivity (typeof(CardCategoriaLoja));
		}

		void btMenu_Click(object sender, EventArgs e){
			Finish();
			StartActivity (typeof(CardCategoriaLoja));
		}

		void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
		{
			var listView = sender as ListView;
			var l = item_categoria [e.Position];

			MainActivity.id_item_categorira = Convert.ToInt32(l.codigo);

			if (listView != null)
			{
				Finish();
				StartActivity(typeof(CardItemDetalhes));
			}
		}

		private ListaCategoriaProduto insItemCategoria(int id_categoria, int codigo, string descricao, string ativo, string descricaoItens, string preco_un){

			ListaCategoriaProduto lstItemCategoria = new ListaCategoriaProduto (id_categoria, codigo, descricao, ativo, descricaoItens, preco_un);
			return lstItemCategoria;

		}

		private List<ListaCategoriaProduto> carregProduto(){

			Android.Database.ICursor sql_cursor = null;
			List<ListaCategoriaProduto> lsItemCategoria = new List<ListaCategoriaProduto> ();

			//percorrendo o retorno do select pelo getrecordcursor e retornando um objeto listacategoriaproduto
			sql_cursor = sqldb.GetRecordCursor (MainActivity.id_categoria);
			//sql_cursor = sqldb.GetRecordCursor ();

			if (sql_cursor != null) {
				sql_cursor.MoveToFirst ();

				while (sql_cursor.Position < sql_cursor.Count){
					lsItemCategoria.Add (insItemCategoria(
						sql_cursor.GetInt(0), //id_categoria
						sql_cursor.GetInt (1),  //cod_produto
						sql_cursor.GetString (2).ToString (), //descricao produto
						sql_cursor.GetString (3).ToString (), //ativo
						sql_cursor.GetString (4).ToString (), //descricao dos itens que compoe o produto
						sql_cursor.GetString (5).ToString())); //preco unitario

					sql_cursor.MoveToNext();
				}
			}

			return lsItemCategoria;
		}
	}
}



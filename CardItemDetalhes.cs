using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Menu
{
	[Activity (Label = "FastDelivery", MainLauncher = false)]
	public class CardItemDetalhes : Activity
	{
		private ArrayAdapter<String> adapter;
		private DBCadProduto sqldb_prod;
		private DBCadCategoria sqldb_cat;
		private DBCadCarrinho sqldb_carrinho;
		private DBCadCarrinhoItem sqldb_carrinhoItem;

		private Android.Database.ICursor sql_cursor = null;

		private TextView txtTotal;
		private TextView txtPrUn;
		private EditText txtLembrete;

		private double iPrecoUnitario;
		private double dValorTotal;
		private int iQtde;

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

			SetContentView (Resource.Layout.frmCardItemDetalhes);

			if (MainActivity.iCodEmpresa > 0) {

				sqldb_prod = new DBCadProduto ("delivery_db");
				sqldb_cat = new DBCadCategoria ("delivery_db");
				sqldb_carrinho = new DBCadCarrinho ("delivery_db");
				sqldb_carrinhoItem = new DBCadCarrinhoItem ("delivery_db");

				ImageButton btAnt = FindViewById<ImageButton> (Resource.Id.btAnt);
				ImageButton btMenu = FindViewById<ImageButton> (Resource.Id.btMenu);
				Button btAdiciona = FindViewById<Button> (Resource.Id.btAdiciona);
				TextView txtProd = FindViewById<TextView> (Resource.Id.txtProd);
				TextView txtCategoria = FindViewById<TextView> (Resource.Id.txtCategoria);
				txtLembrete = FindViewById<EditText> (Resource.Id.txtLembrete);
				TextView txtItens = FindViewById<TextView> (Resource.Id.txtItens);
				TextView txtLoja = FindViewById<TextView> (Resource.Id.txtLoja);
				txtPrUn = FindViewById<TextView> (Resource.Id.txtPrUn);
				txtTotal = FindViewById<TextView> (Resource.Id.txtTotal);
				Spinner cbQtde = FindViewById<Spinner> (Resource.Id.spinner1);
				ImageButton btCarrinho = FindViewById<ImageButton> (Resource.Id.imgBtCarrinho);

				//buscando a descrição da categoria
				sql_cursor = sqldb_cat.GetRecordCursor (MainActivity.id_categoria);

				if (sql_cursor != null) {
					sql_cursor.MoveToFirst ();

					txtCategoria.Text = "Categoria.: " + sql_cursor.GetString (1).ToString ();
				} else {
					txtCategoria.Text = "Categoria não encontrada";
				}

				//buscando a descrição da categoria
				sql_cursor = null;
				sql_cursor = sqldb_prod.GetRecordCursor (MainActivity.id_categoria, MainActivity.id_item_categorira);

				if (sql_cursor.Count > 0) {
					sql_cursor.MoveToFirst ();

					txtProd.Text = sql_cursor.GetString (2).ToString ();
					txtItens.Text = "Item(ns): " + sql_cursor.GetString (4).ToString ();
					txtPrUn.Text = String.Format("R$ : {0:0.00}  ", Convert.ToDouble(sql_cursor.GetString (5)));
					iPrecoUnitario = Convert.ToDouble(sql_cursor.GetString (5));
				} else {
					txtProd.Text = "Produto não encontrado";
					txtItens.Text = "Item(ns): Não encontrado!";
					txtPrUn.Text = "R : 0.00";
					iPrecoUnitario = 0;
				}

				List<string> list = new List<string> ();
				list.Add (" ");
				list.Add ("1");
				list.Add ("2");
				list.Add ("3");
				list.Add ("4");
				list.Add ("5");

				ArrayAdapter<string> dataAdapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerItem, list);
				dataAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
				adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerDropDownItem);
				cbQtde.Adapter = dataAdapter;

				btAnt.Click += new EventHandler (btAnt_Click);
				btAnt.Click += new EventHandler (btAnt_Click);
				btAdiciona.Click += new EventHandler (btAdiciona_Click);
				btMenu.Click += new EventHandler (btMenu_Click);
				cbQtde.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_ItemSelected);
				btCarrinho.Click += new EventHandler (btCarrinho_Click);

				txtLoja.Text = MainActivity.sNomeEmpresa;
			}
			else 
			{
				Finish();
				StartActivity (typeof(MainActivity));
			}
		}

		void btCarrinho_Click(object sender, EventArgs e){
			Finish();
			StartActivity (typeof(Carrinho));
		}

		void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			double dPrun;

			if (spinner.GetItemAtPosition (e.Position).ToString() != " ") {

				iQtde = e.Position;
				dPrun = iPrecoUnitario;
				dValorTotal = (dPrun * iQtde);

				txtTotal.Text = String.Format ("R$: {0:0.00}  ", dValorTotal);
			} else {
				iQtde = 0;
				dValorTotal = 0;
				txtTotal.Text = String.Format ("R$: {0:0.00}  ", 0);
			}
		}

		void btMenu_Click(object sender, EventArgs e){
			Finish();
			StartActivity (typeof(CardCategoriaLoja));
		}

		void btAdiciona_Click(object sender, EventArgs e){
			if (dValorTotal > 0) {

				AdicionaItem ();
			
			} else {
			
				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle("ATENÇÃO");
				builder.SetMessage("Ola! O valor total e quantidade do item estão zerado, por favor informe a quantidade!");
				//builder.SetCancelable(false);
				builder.SetPositiveButton("OK", delegate {});
				builder.Show();

			}
		}

		void btAnt_Click(object sender, EventArgs e){
			Finish();
			StartActivity (typeof(CardCategoriaProdutoLoja));
		}

		public void AdicionaItem() 
		{ 
			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.SetTitle("CONFIRMAÇÃO");
			builder.SetIcon(Android.Resource.Drawable.IcDialogAlert);
			builder.SetMessage("Oi ! Você confirma enviar este item para seu carrinho?");
			builder.SetPositiveButton("Sim", delegate {

				sql_cursor = sqldb_carrinhoItem.GetRecordCursor("select MAX(_itemPedido) + 1 as ITEM from CARRINHO_ITEM " +
					" where _codEmpresa = '" + MainActivity.iCodEmpresa + 
					"' and _codpedido = '" + MainActivity.iCodPedido + "'");

				if (sql_cursor.Count > 0){
					sql_cursor.MoveToFirst();

					int iItem;

					if (sql_cursor.GetInt(0) == 0) {
						iItem = sql_cursor.GetInt(0) + 1;
					}
					else{
						iItem = sql_cursor.GetInt(0);
					}

					sqldb_carrinhoItem.AddRecord(MainActivity.iCodEmpresa,
						MainActivity.iCodPedido,
						iItem,
						MainActivity.id_item_categorira,
						iQtde,
						Convert.ToString(iPrecoUnitario),
						MainActivity.sIMEI,
						MainActivity.sNumCelular,
						txtLembrete.Text,
						"N");

					if (sqldb_carrinhoItem.Message == "OK"){

						Toast.MakeText (this, "Adicionado com sucesso!", ToastLength.Short).Show();
						Thread.Sleep (500);
						Toast.MakeText (this, "Continue comprando, acesso o icone do Menu!", ToastLength.Short).Show();

						Finish();
						StartActivity(typeof(Carrinho));

					}else{
						Toast.MakeText (this, "Erro ao adicionar!", ToastLength.Short).Show();
					}
				}
			});

			builder.SetNegativeButton ("Não", delegate { });
			//builder.SetCancelable(false);
			builder.Show();
		}
	}
}
	



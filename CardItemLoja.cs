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
	[Activity (Label = "Conexao", MainLauncher = true)]

	public class CardItemLoja : Activity
	{

		List<ListaItemCategoria> item_categoria = new List<ListaItemCategoria> ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.frmCardItemLoja);

			item_categoria = preencheLista ();

			ListView lwItemCategoria = FindViewById<ListView> (Resource.Id.lwItemCategoria);
			ImageButton btAnt = FindViewById<ImageButton> (Resource.Id.btAnt);
			ImageButton btMenu = FindViewById<ImageButton> (Resource.Id.btMenu);

			lwItemCategoria.Adapter = new adapter_listitem (this, item_categoria);
			lwItemCategoria.ItemClick += OnListItemClick;

			btAnt.Click += new EventHandler (btAnt_Click);
			btMenu.Click += new EventHandler (btMenu_Click);
		}

		void btAnt_Click(object sender, EventArgs e){
			StartActivity (typeof(CardLoja));
		}

		void btMenu_Click(object sender, EventArgs e){
			StartActivity (typeof(CardLoja));
		}

		void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
		{
			var listView = sender as ListView;
			var l = item_categoria [e.Position];

			MainActivity.id_item_categorira = Convert.ToInt32(l.codigo);

			if (listView != null)
			{
				StartActivity(typeof(CardItemDetalhes));
			}
		}

		private ListaItemCategoria insItemCategoria(int id_categoria, int codigo, int id_img, string descricao, double preco_un){

			ListaItemCategoria lstItemCategoria = new ListaItemCategoria (id_categoria, codigo, id_img, descricao, preco_un);
			return lstItemCategoria;

		}

		private List<ListaItemCategoria> preencheLista(){
			List<ListaItemCategoria> lsItemCategoria = new List<ListaItemCategoria> ();

			if (MainActivity.id_categoria == 1) {
				lsItemCategoria.Add (insItemCategoria (1, 1, 999, "CAIXA-01", 2.30));
			}
			else if (MainActivity.id_categoria == 2) {
				lsItemCategoria.Add (insItemCategoria (2, 6, 994, "FISCAL-01", 3.30));
				lsItemCategoria.Add (insItemCategoria (2, 7, 993, "FISCAL-02", 5.90));
				lsItemCategoria.Add (insItemCategoria (2, 8, 992, "FISCAL-03", 3.50));
				lsItemCategoria.Add (insItemCategoria (2, 9, 991, "FISCAL-04", 56.20));
				lsItemCategoria.Add (insItemCategoria (2, 10, 990, "FISCAL-05", 99.90));
			} else if (MainActivity.id_categoria == 3) {

				lsItemCategoria.Add (insItemCategoria (3, 11, 989, "FINANCEIRO-01", 11.50));
				lsItemCategoria.Add (insItemCategoria (3, 12, 988, "FINANCEIRO-02", 10.50));
				lsItemCategoria.Add (insItemCategoria (3, 13, 987, "FINANCEIRO-03", 9.90));
				lsItemCategoria.Add (insItemCategoria (3, 14, 986, "FINANCEIRO-04", 6.50));
			} else if (MainActivity.id_categoria == 4) {

				lsItemCategoria.Add (insItemCategoria (4, 16, 989, "COMPRAS-01", 3.30));
				lsItemCategoria.Add (insItemCategoria (4, 17, 988, "COMPRAS-02", 2.50));
				lsItemCategoria.Add (insItemCategoria (4, 18, 987, "COMPRAS-03", 7.50));
				lsItemCategoria.Add (insItemCategoria (4, 19, 986, "COMPRAS-04", 8.20));
				lsItemCategoria.Add (insItemCategoria (4, 20, 985, "COMPRAS-05", 2.50));
			} else if (MainActivity.id_categoria == 5) {
				lsItemCategoria.Add (insItemCategoria (5, 16, 989, "TI-01", 120.00));
				lsItemCategoria.Add (insItemCategoria (5, 17, 988, "TI-02", 99.50));
			}

			return lsItemCategoria;
		}
	}


}



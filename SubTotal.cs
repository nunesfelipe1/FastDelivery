using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Menu
{
	[Activity (Label = "FastDelivery", MainLauncher = false)]
	public class SubTotal : Activity
	{
		private List<ListaSubTotal> item_subtotal = new List<ListaSubTotal> ();
		IAsyncResult arCarregaSubTotal;
		private string resultadoSubTotal;
		private double dSubTotal;
		private TextView txtSubTotal;

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

			SetContentView (Resource.Layout.frmSubTotal);

			ListView lwSubTotal = FindViewById<ListView> (Resource.Id.lwSubTotal);
			ImageButton btAnt = FindViewById<ImageButton> (Resource.Id.btAnt);
			txtSubTotal = FindViewById<TextView> (Resource.Id.txtSTotal);

			item_subtotal = carregaSubTotal ();

			lwSubTotal.Adapter = new adapter_subtotal (this, item_subtotal);
			btAnt.Click += new EventHandler (btAnt_Click);
		}

		void btAnt_Click(object sender, EventArgs e){
			Finish();
			StartActivity (typeof(CardCategoriaLoja));
		}

		private ListaSubTotal insSubTotal(string qtde, string descricao, string preco_un){

			ListaSubTotal lstSubTotal = new ListaSubTotal (qtde, descricao, preco_un);
			return lstSubTotal;

		}

		private List<ListaSubTotal> carregaSubTotal(){
			List<ListaSubTotal> lsSubTotal = new List<ListaSubTotal> ();		

			List<ListaEmpresas> lsEmpresas = new List<ListaEmpresas> ();

			var webservice = new WS.IdmServerservice (MainActivity.sCaminhoWS);

			arCarregaSubTotal = webservice.BeginRetornaSubTotal (MainActivity.iCodEmpresa, MainActivity.iNumMesa, null, webservice);

			resultadoSubTotal = webservice.EndRetornaSubTotal (arCarregaSubTotal);

			if (resultadoSubTotal != "" && resultadoSubTotal != "erro") {	

				int i;
				int j;
				string temp;
				string letra;

				string qte;
				string descricao;
				string preco_un;

				i = resultadoSubTotal.Length;
				j = 0;
				temp = "";
				letra = "";
				dSubTotal = 0;

				qte = "";
				descricao = "";
				preco_un = "";

				while (j < i) {

					letra = resultadoSubTotal [j].ToString ();

					if (letra != "|" & letra != "%") {
						temp = temp + letra;
					}

					if (letra == "|") {
						if (qte == "")
							qte = temp;
						else if (preco_un == "")
							preco_un = temp;
						else if (descricao == "")
							descricao = temp;

						temp = "";
					}

					if (letra == "%") {

						lsSubTotal.Add (insSubTotal (
							qte, //qtde
							descricao,  //cod_produto
							preco_un)); //preco unitario

						dSubTotal = dSubTotal + (Convert.ToDouble (qte) * Convert.ToDouble (preco_un));

						temp = "";

						qte = "";
						descricao = "";
						preco_un = "";
					}

					j++;
				}
			} else if (resultadoSubTotal == "erro") {

				dSubTotal = 0;

				Toast.MakeText (this, "Pedido vazio. Vamos as compras!!!", ToastLength.Short).Show();
			}

			txtSubTotal.Text = String.Format ("R$: {0:0.00}  ", dSubTotal);

			return lsSubTotal;
		}
	}
}




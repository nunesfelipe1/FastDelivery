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
using System.Threading;
using System.Globalization;

namespace Menu
{
	[Activity (Label = "FastDelivery", MainLauncher = false)]
	public class Carrinho : Activity
	{
		DBCadPedidoItem sqldb_pedido_item;
		DBCadCarrinho sqldb_carrinho;
		DBCadCarrinhoItem sqldb_carrinho_item;
		DBCadPedido sqldb_pedido;

		List<ListaCarrinho> carrinho = new List<ListaCarrinho> ();

		ListView lwCarrinho;
		TextView txtSubTotal;

		ProgressDialog progressDialog;

		IAsyncResult arPedido;
		IAsyncResult arPedidoItem;
		IAsyncResult arAtualizaPedido;
		IAsyncResult arVerificaPedido;
		IAsyncResult arVerificaMesa;
		IAsyncResult arEstatisticas;

		string resultadoPedidoItem;
		string resultadoPedido;
		string resultadoVerificaMesa;
		string resultadoEstatisticas;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.frmCarrinho);

			if (MainActivity.iCodEmpresa > 0) {

				sqldb_carrinho = new DBCadCarrinho ("delivery_db");
				sqldb_carrinho_item = new DBCadCarrinhoItem ("delivery_db");
				sqldb_pedido = new DBCadPedido ("delivery_db");
				sqldb_pedido_item = new DBCadPedidoItem ("delivery_db");

				lwCarrinho = FindViewById<ListView> (Resource.Id.lwCarrinho);
				ImageButton btAnt = FindViewById<ImageButton> (Resource.Id.btAnt);
				ImageButton btCarrinho = FindViewById<ImageButton> (Resource.Id.imgBtCarrinho);
				ImageButton imgBtMenu = FindViewById<ImageButton> (Resource.Id.imgBtMenu);
				ImageButton btInseriPedido = FindViewById<ImageButton> (Resource.Id.btInseriPedido);
				ImageButton btSubTotal = FindViewById<ImageButton> (Resource.Id.btSubTotal);
				txtSubTotal = FindViewById<TextView> (Resource.Id.txtSTotal);

				carrinho = carregaCarrinho ();	

				lwCarrinho.Adapter = new adapter_carrinho (this, carrinho);
				lwCarrinho.ItemClick += OnListItemClick;

				btAnt.Click += new EventHandler (btAnt_Click);
				imgBtMenu.Click += new EventHandler (btMenu_Click);
				btInseriPedido.Click += new EventHandler (btInseriPedido_Click);
				btSubTotal.Click += new EventHandler (btSubTotal_Click);
			} 
			else 
			{
				Finish();
				StartActivity (typeof(MainActivity));
			}
		}

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

		void btSubTotal_Click(object sender, EventArgs e){
			var progressDialog = ProgressDialog.Show (this, "Acessando o Caixa...", "Carregando os pedidos realizados.", true);

			new Thread (new ThreadStart (delegate {		
			
				Finish();
				StartActivity (typeof(SubTotal));

				RunOnUiThread (() => progressDialog.Hide ());

			})).Start ();

		}

		void btInseriPedido_Click(object sender, EventArgs e)
		{
			if (lwCarrinho.Count > 0) {
				AlertDialog.Builder builder1 = new AlertDialog.Builder (this);
				builder1.SetTitle ("CONFIRMAÇÃO");
				builder1.SetIcon (Android.Resource.Drawable.IcDialogAlert);
				builder1.SetMessage ("Oi ! Você confirma solicitação desse(s) produto(s)?");
				builder1.SetPositiveButton ("Sim", delegate {

					progressDialog = ProgressDialog.Show (this, "CONFIRMAÇÃO", "Enviando o Pedido", true);

					new Thread (new ThreadStart (delegate {		

						enviaPedido ();

					})).Start ();
				});

				builder1.SetNegativeButton ("Não", delegate {

				});
				builder1.Show ();
			} else {
				Toast.MakeText (this, "O carrinho esta vazio, vamos as compras!!!", ToastLength.Short).Show ();
			}
		}

		void enviaPedido()
		{
			try {
				//verifica se as informações do numero da mesa batem com o codigo apresentado pelo garçon
				var webservice = new WS.IdmServerservice (MainActivity.sCaminhoWS.ToString ());
				arVerificaMesa = webservice.BeginVerificaCodigoMesa (MainActivity.iCodMesa.ToString (), MainActivity.iCodEmpresa, MainActivity.iNumMesa, null, webservice);
				resultadoVerificaMesa = "";
				resultadoVerificaMesa = webservice.EndVerificaCodigoMesa (arVerificaMesa);

				//ao enviar um item para a caixa, verificar se a mesa ja não foi fechada
				if (resultadoVerificaMesa == "true") {

					gravaPedido ();

				} else if (resultadoVerificaMesa == "false") {
					AlertDialog.Builder builder = new AlertDialog.Builder (this);
					builder.SetTitle ("ATENÇÃO");
					builder.SetMessage ("Ola Cliente! Os números estão errados, verifique por favor com o Garćon! Obrigado");
					builder.SetPositiveButton ("OK", delegate {

						StartActivity (typeof(InfLoja));

						sqldb_carrinho.DeleteRecord ("delete from CARRINHO where _codPedido = '" + MainActivity.iCodPedido + "'" +
						" and numMesa = '" + MainActivity.iNumMesa + "'");

						sqldb_carrinho_item.DeleteRecord ("delete from CARRINHO_ITEM where _codPedido = '" + MainActivity.iCodPedido + "'");
					});

					builder.Show ();
				} else if (resultadoVerificaMesa != "true" && resultadoVerificaMesa != "false") {

					Android.Database.ICursor sql_cursor = null;

					sql_cursor = sqldb_pedido.GetRecordCursor ("select * from PEDIDO where _codEmpresa = '" + MainActivity.iCodEmpresa +
					"' and numMesa = '" + MainActivity.iNumMesa + "' and dataFechado is null " +
					" and _codPedido = '" + MainActivity.iCodPedido + "'" +
					" and codigoMesa = '" + MainActivity.iCodMesa.ToString () + "'");

					if (sql_cursor.Count > 0) {
						DateTime dData = DateTime.Now;
						string sDataPedido;

						sDataPedido = dData.ToString ("dd/MM/yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
						sDataPedido = resultadoVerificaMesa;

						sqldb_pedido.UpdateRecord (" UPDATE pedido set dataFechado = '" + sDataPedido +
						"' WHERE _codEmpresa ='" + MainActivity.iCodEmpresa + "' and _codPedido = '" + MainActivity.iCodPedido +
						"' and numMesa = '" + MainActivity.iNumMesa + "'");

						if (sqldb_pedido.Message == "ok") {
							AlertDialog.Builder builder = new AlertDialog.Builder (this);
							builder.SetTitle ("FINALIZAÇÃO");
							builder.SetIcon (Android.Resource.Drawable.IcDialogAlert);
							builder.SetMessage ("Ola Cliente! Informamos que essa mesa com o código informado ja esta fechada, qualquer dúvida procure o GC. Obrigado!");
							builder.SetPositiveButton ("OK", delegate {

								Finish();
								StartActivity (typeof(InfLoja));

								sqldb_carrinho.DeleteRecord ("delete from CARRINHO where _codPedido = '" + MainActivity.iCodPedido + "'" +
								" and numMesa = '" + MainActivity.iNumMesa + "'");

								sqldb_carrinho_item.DeleteRecord ("delete from CARRINHO_ITEM where _codPedido = '" + MainActivity.iCodPedido + "'");
							});
							builder.Show ();
						}
					} else {

						AlertDialog.Builder builder = new AlertDialog.Builder (this);
						builder.SetTitle ("ATENÇÃO");
						builder.SetMessage ("Ola Cliente! Os números estão errados, verifique por favor com o Garćon! Obrigado");
						builder.SetPositiveButton ("OK", delegate {

							Finish();
							StartActivity (typeof(InfLoja));

						});
						builder.Show ();
					}
				}
			} catch {
				AlertDialog.Builder builder = new AlertDialog.Builder (this);
				builder.SetTitle ("ERRO DE CONEXÃO");
				builder.SetIcon (Android.Resource.Drawable.IcDialogAlert);
				builder.SetMessage ("Ops! Verifique a conexão da sua internet ou procure um Garçon!");
				//builder.SetCancelable (false);
				builder.SetPositiveButton ("OK", delegate {

					Finish();
					StartActivity (typeof(MainActivity));

				});
			}
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
			var l = carrinho [e.Position];

			if (lwCarrinho.Count > 0) {
				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle("CONFIRMAÇÃO");
				builder.SetIcon(Android.Resource.Drawable.IcDialogAlert);
				builder.SetMessage("Oi ! Você confirma exclusão desses item?");
				builder.SetPositiveButton("Sim", delegate {

					Android.Database.ICursor sql_cursor = null;

					sql_cursor = sqldb_carrinho_item.GetRecordCursor ("select pi.* " + 
						" from carrinho_item pi  " + 
						" where pi.enviado = 'N' " +
						"   and pi._codEmpresa = '" + l.codEmpresa + "' " +
						"   and pi._codPedido = '" + l.codPedido +"'" +
						"   and pi._itemPedido = '" + l.itemPedido + "'");

					if (sql_cursor.Count > 0){
						sqldb_carrinho_item.DeleteRecord (" delete from carrinho_item  " + 
							" where enviado = 'N' " +
							"   and _codEmpresa = '" + l.codEmpresa + "' " +
							"   and _codPedido = '" + l.codPedido +"'" +
							"   and _itemPedido = '" + l.itemPedido + "'");

						carrinho.Remove (carrinho [e.Position]);

						Finish();
						StartActivity(typeof(Carrinho));
					}

				});

				builder.SetNegativeButton ("Não", delegate { });
				builder.SetCancelable(false);
				builder.Show();

			}else
				Toast.MakeText (this, "O carrinho esta vazio, vamos as compras!!!", ToastLength.Short).Show();
		}

		private List<ListaCarrinho> carregaCarrinho(){

			List<ListaCarrinho> lsCarrinho = new List<ListaCarrinho> ();
			double dSubTotal = 0;

			Android.Database.ICursor sql_cursor = null;

			//percorrendo o retorno do select pelo getrecordcursor e retornando um objeto listacategoria
			sql_cursor = sqldb_carrinho_item.GetRecordCursor ("select pi.*, pr.descricao " + 
				" from carrinho_item pi inner join produto pr on (pi._codproduto = pr._codProduto) " + 
				" where  pi.enviado = 'N' and pi._codEmpresa = '" + MainActivity.iCodEmpresa + "' and pi._codPedido = '" + MainActivity.iCodPedido +"'");

			if (sql_cursor.Count > 0) {
				sql_cursor.MoveToFirst ();

				while (sql_cursor.Position < sql_cursor.Count){
					lsCarrinho.Add (insCarrinho (sql_cursor.GetInt (0), //codempresa
						sql_cursor.GetInt (1), //codpedido
						sql_cursor.GetInt (2), //itempedido
						sql_cursor.GetInt (3), //codproduto
						sql_cursor.GetString (10).ToString(), //descricao
						sql_cursor.GetFloat (4),//quantidade
						Convert.ToDouble(sql_cursor.GetString (5)),//valorunitario
						sql_cursor.GetString (6).ToString (), //numero serial
						sql_cursor.GetString (7).ToString (),//numero serial
						sql_cursor.GetString (8).ToString ())); //lembrete

					dSubTotal = dSubTotal + (Convert.ToDouble(sql_cursor.GetString (4)) * Convert.ToDouble(sql_cursor.GetString (5)));

					sql_cursor.MoveToNext();
				}
			}

			txtSubTotal.Text = String.Format ("R$: {0:0.00}  ", dSubTotal);

			sqldb_carrinho.UpdateRecord(MainActivity.iCodEmpresa, MainActivity.iNumMesa, MainActivity.iCodPedido, dSubTotal);

			return lsCarrinho;
		}

		private ListaCarrinho insCarrinho(int codEmpresa, int codPedido, int itemPedido, 
			int codProduto, string descricao, double qtde, double vlrUnit, string numSerial, string numCelular, string lembrete){

			ListaCarrinho lstCarrinho = new ListaCarrinho (codEmpresa, codPedido, itemPedido, codProduto, 
				descricao, qtde, vlrUnit, numSerial, numCelular, lembrete);
			return lstCarrinho;
		}

		private void gravaPedido()
		{
			try {

				var webservice = new WS.IdmServerservice (MainActivity.sCaminhoWS.ToString ());

				Android.Database.ICursor sql_cursor = null;

				//carregando as informações do pedido
				sql_cursor = sqldb_carrinho.GetRecordCursor ("select * from CARRINHO " +
				"  where _codPedido  = '" + MainActivity.iCodPedido + "'" +
				"    and _codEmpresa = '" + MainActivity.iCodEmpresa + "'");

				if (sql_cursor.Count > 0) {
					sql_cursor.MoveToFirst ();

					arVerificaPedido = webservice.BeginVerificaPedido (
						sql_cursor.GetInt (1),//codigoempresa
						sql_cursor.GetInt (0),//codigo pedido
						sql_cursor.GetInt (2),//numero da mesa
						null, webservice);

					Thread.Sleep (500);

					resultadoPedido = webservice.EndVerificaPedido (arVerificaPedido);

					if (resultadoPedido == "false") {

						arPedido = webservice.BeginGravaPedido (
							sql_cursor.GetInt (0),//codigo pedido
							sql_cursor.GetInt (1),//codigoempresa
							sql_cursor.GetInt (2),//numero da mesa
							sql_cursor.GetString (3),//data pedidos
							sql_cursor.GetString (4).Replace (",", "."),//valortotal
							sql_cursor.GetString (5),//numero serial
							sql_cursor.GetString (6),//numerocelular
							sql_cursor.GetString (7),//data fechado
							sql_cursor.GetString (8),//codigo mesa
							null, webservice);		

						Thread.Sleep (500);

						resultadoPedido = webservice.EndGravaPedido (arPedido);

						if (resultadoPedido == "ok")
						{
							DateTime dData = DateTime.Now;
							string sDataPedido;

							sDataPedido = dData.ToString ("dd/MM/yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo);

							var webserviceFast = new WS.IdmServerservice (MainActivity.sCaminhoWSFast);

							arEstatisticas = webserviceFast.BeginEstatistica(
								"Pedido",//codigoempresa
								sDataPedido,//data
								sql_cursor.GetInt (1),//codigoempresa
								sql_cursor.GetInt (2),//numero da mesa
								null, webserviceFast);

							Thread.Sleep (500);

							resultadoEstatisticas = webserviceFast.EndEstatistica (arEstatisticas);
						}
					} else {

						arAtualizaPedido = webservice.BeginAtualizaPedido (
							sql_cursor.GetInt (1),//codigoempresa
							sql_cursor.GetInt (0),//codigo pedido
							sql_cursor.GetInt (2),//numero da mesa
							sql_cursor.GetString (4).Replace (",", "."),//valortotal
							null, webservice);		

						Thread.Sleep (500);

						resultadoPedido = webservice.EndAtualizaPedido (arAtualizaPedido);

						if (resultadoPedido == "ok")
						{
							DateTime dData = DateTime.Now;
							string sDataPedido;

							sDataPedido = dData.ToString ("dd/MM/yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo);

							var webserviceFast = new WS.IdmServerservice (MainActivity.sCaminhoWSFast);
							//var webserviceFast = new WS.IdmServerservice ("http://10.0.2.2:8091/wsfastdelivery/WsFastDelivery.dll/soap/IdmServer");

							arEstatisticas = webserviceFast.BeginEstatistica(
								"Pedido",//codigoempresa
								sDataPedido,//data
								sql_cursor.GetInt (1),//codigoempresa
								sql_cursor.GetInt (2),//numero da mesa
								null, webserviceFast);

							Thread.Sleep (500);

							resultadoEstatisticas = webserviceFast.EndEstatistica (arEstatisticas);
						}
					}
				}

				if (resultadoPedido == "ok") {
					//carregando as informações do pedido
					sql_cursor = sqldb_carrinho.GetRecordCursor ("select * from CARRINHO " +
					"  where _codPedido  = '" + MainActivity.iCodPedido + "'" +
					"    and _codEmpresa = '" + MainActivity.iCodEmpresa + "'");

					if (sql_cursor.Count > 0) {
						sql_cursor.MoveToFirst ();

						Android.Database.ICursor sql_cursor_pedido = null;

						sql_cursor_pedido = sqldb_pedido.GetRecordCursor ("select * from PEDIDO " +
						"  where _codPedido  = '" + MainActivity.iCodPedido + "'" +
						"    and _codEmpresa = '" + MainActivity.iCodEmpresa + "'");

						if (sql_cursor_pedido.Count > 0) {
							sql_cursor.MoveToFirst ();

							sqldb_pedido.UpdateRecord (
								sql_cursor.GetInt (1),//codigoempresa
								sql_cursor.GetInt (0),//codigo pedido
								sql_cursor.GetInt (2),//numero da mesa
								sql_cursor.GetString (4).Replace (",", "."));//valortotal
						} else {
							//envia o carrinho para a tabela de pedido no local no tablet
							sqldb_pedido.AddRecord (
								sql_cursor.GetInt (0),//codigo pedido
								sql_cursor.GetInt (1),//codigoempresa
								sql_cursor.GetInt (2),//numero da mesa
								sql_cursor.GetString (3),//data pedidos
								sql_cursor.GetString (4).Replace (",", "."),//valortotal
								sql_cursor.GetString (5),//numero serial
								sql_cursor.GetString (6),//numerocelular
								sql_cursor.GetString (8));//codigo mesa		
						}
					}

					if (sqldb_pedido.Message == "ok") {

						sqldb_carrinho.UpdateRecord (MainActivity.iCodEmpresa, MainActivity.iNumMesa, MainActivity.iCodPedido, 0);

					}
				} else {
									Toast.MakeText (this, "Nossa que feio - Tchau!!", ToastLength.Short).Show ();
				}

				//PEDIDO ITEM
				var webserviceItem = new WS.IdmServerservice (MainActivity.sCaminhoWS.ToString ());

				//carregando as informações do pedido
				sql_cursor = sqldb_carrinho_item.GetRecordCursor ("select * from CARRINHO_ITEM " +
				"  where _codPedido  = '" + MainActivity.iCodPedido + "'" +
				"    and _codEmpresa = '" + MainActivity.iCodEmpresa + "'" +
				"    and enviado = 'N' order by _itemPedido ");

				if (sql_cursor.Count > 0) {
					sql_cursor.MoveToFirst ();

					DateTime agora = DateTime.Now;
					string sDataEnvio;
					sDataEnvio = agora.ToString ("dd/MM/yyyy HH:mm:ss:FFF", DateTimeFormatInfo.InvariantInfo);

					while (sql_cursor.Position < sql_cursor.Count) {

						arPedidoItem = webserviceItem.BeginGravaPedidoItem (
							sql_cursor.GetInt (0),//codigo empresa
							sql_cursor.GetInt (1),//codigo pedido
							sql_cursor.GetInt (2),//item pedido
							sql_cursor.GetInt (3),//coduto produto
							sql_cursor.GetString (4),//quantidade
							sql_cursor.GetString (5).Replace (",", "."),//preço unitário
							sql_cursor.GetString (6),//numero serial
							sql_cursor.GetString (7),//numerocelular
							sql_cursor.GetString (8),//lembrete
							sql_cursor.Count.ToString(),//quantidade de itens enviando
							sDataEnvio,//codigo de envio ---Alterar calculo
							null, webserviceItem);

						Thread.Sleep (500);
						resultadoPedidoItem = webserviceItem.EndGravaPedidoItem (arPedidoItem);
						sql_cursor.MoveToNext ();
					}

					if (resultadoPedidoItem == "ok") {
						//carregando as informações do pedido
						sql_cursor = sqldb_carrinho_item.GetRecordCursor ("select * from CARRINHO_ITEM " +
						"  where _codPedido  = '" + MainActivity.iCodPedido + "'" +
						"    and _codEmpresa = '" + MainActivity.iCodEmpresa + "'" +
						"    and enviado = 'N'");

						if (sql_cursor.Count > 0) {
							sql_cursor.MoveToFirst ();

							while (sql_cursor.Position < sql_cursor.Count) {

								sqldb_pedido_item.AddRecord (
									sql_cursor.GetInt (0),//codigo empresa
									sql_cursor.GetInt (1),//codigo pedido
									sql_cursor.GetInt (2),//item pedido
									sql_cursor.GetInt (3),//coduto produto
									sql_cursor.GetFloat (4),//quantidade
									sql_cursor.GetFloat (5),//preço unitário
									sql_cursor.GetString (6),//numero serial
									sql_cursor.GetString (7),//numerocelular
									sql_cursor.GetString (8));

								sql_cursor.MoveToNext ();
							}	

							if (sqldb_pedido_item.Message == "ok") {

								sqldb_carrinho_item.UpdateRecord (MainActivity.iCodEmpresa, MainActivity.iCodPedido);

								RunOnUiThread (() => progressDialog.Hide ());	

								this.RunOnUiThread (delegate {

									AlertDialog.Builder builder3 = new AlertDialog.Builder (this);
									builder3.SetTitle ("SUCESSO!!!");
									builder3.SetIcon (Android.Resource.Drawable.IcDialogAlert);
									builder3.SetMessage ("Ola! Seu pedido foi enviado para o caixa com sucesso. Por Favor aguarde a entrega!");
									builder3.SetNegativeButton ("OK", delegate {

										Finish();
										StartActivity (typeof(CardCategoriaLoja));

									});
									builder3.Show ();
								});
							}							
						}
					} else {
						Toast.MakeText (this, "PD - Feio!!!", ToastLength.Short).Show ();
					}
				}
			} catch {

				this.RunOnUiThread (delegate {
					AlertDialog.Builder builder = new AlertDialog.Builder (this);
					builder.SetTitle ("ERRO DE CONEXÃO");
					builder.SetIcon (Android.Resource.Drawable.IcDialogAlert);
					builder.SetMessage ("Ops! Verifique a conexão da sua internet ou procure um Garçon!");
					builder.SetCancelable (false);
					builder.SetPositiveButton ("OK", delegate {

						Finish();
						StartActivity (typeof(MainActivity));

					});
					builder.Show ();
				});
			}
		}
	}
}



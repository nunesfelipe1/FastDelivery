using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Database.Sqlite;
using System.Collections.Generic;
using System.Threading;

using Java.Sql;
using Java.IO;
using Java.Lang;

namespace Menu
{
	[Activity (Label = "FastDelivery", MainLauncher = false)]
	public class CarrEmpresa : Activity
	{
		List<ListaEmpresas> empresas = new List<ListaEmpresas> ();
		List<ListaEmpresas> lsEmpresas = new List<ListaEmpresas> ();

		DBCadCategoria sqldb_categoria;
		DBCadProduto sqldb_produto;

		ListView lwEmpresas;

		IAsyncResult arCarregaProduto;
		string resultadoCarregaProduto;

		IAsyncResult arCarregaCategoria;
		string resultadoCarregaCategoria;

		IAsyncResult arCarregaEmpresa;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.frmCarrEmpresas);

			if (MainActivity.iCodEmpresa > 0) {

				//var progressDialog = ProgressDialog.Show (this, "Aguarde...", "Estamos carregando os estabelecimentos parceiros.", true);

				//	new System.Threading.Thread (new ThreadStart (delegate {		

				sqldb_categoria = new DBCadCategoria ("delivery_db");
				sqldb_produto = new DBCadProduto ("delivery_db");

				limpaTabelas ();
	
				ImageButton imgBtAnt = FindViewById<ImageButton> (Resource.Id.imgBtAnt);
				lwEmpresas = FindViewById<ListView> (Resource.Id.lwEmpresas);

				imgBtAnt.Click += new EventHandler (imgBtAnt_Click);

				carregaEmpresa ();
				//RunOnUiThread (() => progressDialog.Hide ());

				//					})).Start ();


			} else {
				Finish ();
				StartActivity (typeof(MainActivity));
			}
		}

		public override bool OnKeyDown(Keycode keycode, KeyEvent @event)
		{
			bool handled = false;

			try{

				Finish();
				StartActivity (typeof(MainActivity));

				handled = true;
			}
			catch {
				handled = false;
			}
			return handled || base.OnKeyDown (keycode, @event);
		}

		void limpaTabelas(){

			DBCadPedido sqlCadPedido = new DBCadPedido ("delivery_db");
			DBCadPedidoItem sqlCadPedidoItem  = new DBCadPedidoItem ("delivery_db");
			DBCadCarrinho sqlCadCarrinho  = new DBCadCarrinho ("delivery_db");
			DBCadCarrinhoItem sqlCadCarrinhoItem  = new DBCadCarrinhoItem ("delivery_db");

			sqlCadPedido.DeleteRecord ();
			sqlCadPedidoItem.DeleteRecord ("delete from PEDIDO_ITEM");
			sqlCadCarrinho.DeleteRecord ();
			sqlCadCarrinhoItem.DeleteRecord ("delete from CARRINHO_ITEM");
		}

		void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
		{
			var listView = sender as ListView;
			var l = empresas [e.Position];

			if (l.status == "ON") {

				var progressDialog = ProgressDialog.Show (this, "Aguarde...", "Carregando as categorias e os produtos.", true);

				new System.Threading.Thread (new ThreadStart (delegate {		

					MainActivity.iCodEmpresa = Convert.ToInt32 (l.codEmpresa);
					MainActivity.sCaminhoWS = Convert.ToString (l.caminhoWS);
					MainActivity.sCaminhoWSReserva = Convert.ToString (l.caminhoWSReserva);
					MainActivity.sNomeEmpresa = Convert.ToString (l.nome);

					sqldb_categoria.DeleteRecord ();
					carregaCategoria ();

					sqldb_produto.DeleteRecord ();
					carregaProduto ();

					RunOnUiThread (() => progressDialog.Hide ());

				})).Start ();
			} else {

				AlertDialog.Builder builder = new AlertDialog.Builder (this);
				builder.SetTitle ("OLA!");
				builder.SetMessage ("O parceiro selecionado encontra-se Offline, tente novamente mais tarde ou selecione outro parceiro!");
				builder.SetCancelable (false);
				builder.SetPositiveButton ("OK", delegate {
				});
				builder.Show ();

			}
		}

		void imgBtAnt_Click(object Sender, EventArgs e)
		{
			//Toast.MakeText (this, "Anterior...!", ToastLength.Short).Show();
			Finish();
			StartActivity(typeof(MainActivity));
		}

		private ListaEmpresas insEmpresas(int codEmpresa, string nome, string caminhoWS, string caminhoWSReserva, string endereco, 
			string telefone, string cidade,	string bairro, string cnpj, string responsavel, string status){

			ListaEmpresas lstEmpresas = new ListaEmpresas (codEmpresa, nome, caminhoWS, caminhoWSReserva, endereco, telefone, cidade,	
				bairro, cnpj, responsavel, status);
			return lstEmpresas;		
		}

		/* void carregaEmpresa()
		{
			List<ListaEmpresas> lsEmpresas = new List<ListaEmpresas> ();

			string driver = "oracle.jdbc.driver.OracleDriver";
			string bd = "orcl";
			string usuario = "DELIVERY";
			string pass = "admin";
			string porta = "1521";
			string ip = MainActivity.sCaminhoWSFast;

			string url = "jdbc:oracle:thin:@" + ip + ":" + porta + ":" + bd;

			try {
				Class.ForName (driver).NewInstance ();
				var con = DriverManager.GetConnection (url, usuario, pass);
				IStatement st = con.CreateStatement ();
				IResultSet resultado = st.ExecuteQuery ("select * from produto order by COD_PRODUTO");

				if (resultado.Row > 0) {

					string codEmpresa;
					string nome;
					string caminhoWS;
					string caminhoWSReserva;
					string endereco;
					string bairro;
					string cidade;

					while (resultado.Next ()) {

						codEmpresa = resultado.GetString ("COD_PRODUTO");
						nome = resultado.GetString ("NOME");
						caminhoWS = resultado.GetString ("CAMINHO_WS");
						caminhoWSReserva = resultado.GetString ("CAMINHO_WS_RESERVA");
						endereco = resultado.GetString ("ENDERECO");
						bairro = resultado.GetString ("BAIRRO");
						cidade = resultado.GetString ("CIDADE");

						lsEmpresas.Add (insEmpresas (Convert.ToInt32 (codEmpresa), nome, caminhoWS, caminhoWSReserva, endereco, "x", cidade, bairro, "x", "x"));

						if (lsEmpresas.Count > 0) {

							empresas = lsEmpresas;
							lwEmpresas.Adapter = new adapter_empresas (this, empresas);
							lwEmpresas.ItemClick += OnListItemClick;

						} else {
							AlertDialog.Builder builder = new AlertDialog.Builder (this);
							builder.SetTitle ("ATENÇÃO");
							builder.SetMessage ("Nossa, aconteceu um erro grave. Tchau!!!");
							builder.SetCancelable (false);
							builder.SetPositiveButton ("OK", delegate {
							});
							builder.Show ();
						}
					}
				}
			} catch (System.Exception ex) {
				AlertDialog.Builder builder = new AlertDialog.Builder (this);
				builder.SetTitle ("ERRO DE CONEXÃO");
				builder.SetIcon (Android.Resource.Drawable.IcDialogAlert);
				builder.SetMessage ("Ops! Verifique a conexão da sua internet ou procure um Garçon!");
				builder.SetPositiveButton ("OK", delegate {

					Finish ();
					StartActivity (typeof(MainActivity));
				});
				builder.Show ();
			}
		}*/
			
		private void carregaEmpresa()
		{
			try {

				List<ListaEmpresas> lsEmpresas = new List<ListaEmpresas> ();

        		var webservice = new WS.IdmServerservice (MainActivity.sCaminhoWSFast);

				arCarregaEmpresa = webservice.BeginCarregaEmpresa (null, webservice);

				string resultadoCarregaEmpresa = "";

				resultadoCarregaEmpresa = webservice.EndCarregaEmpresa (arCarregaEmpresa);

				this.RunOnUiThread (delegate {

					if (resultadoCarregaEmpresa != "") {

						//Toast.MakeText (this, MainActivity.sCaminhoWSFast, ToastLength.Short).Show();

						int i;
						int j;
						string temp;
						string letra;

						string codEmpresa;
						string nome;
						string caminhoWS;
						string caminhoWSReserva;
						string endereco;
						string bairro;
						string cidade;
						string status;

						i = resultadoCarregaEmpresa.Length;
						j = 0;
						temp = "";
						letra = "";

						codEmpresa = "";
						nome = "";
						caminhoWS = "";
						caminhoWSReserva = "";
						endereco = "";
						bairro  = "";
						cidade = "";
						status = "";

						while (j < i) {

							letra = resultadoCarregaEmpresa [j].ToString ();

							if (letra != "|" & letra != "%") {
								temp = temp + letra;
							}

							if (letra == "|") {
								if (codEmpresa == "")
									codEmpresa = temp;
								else if (nome == "")
									nome = temp;
								else if (caminhoWS == "")
									caminhoWS = temp;
								else if (endereco == "")
									endereco = temp;					
								else if (bairro == "")
									bairro = temp;
								else if (cidade == "")
									cidade = temp;
								else if (caminhoWSReserva == "")
									caminhoWSReserva = temp;
								else if (status == "")
									status = temp;

								temp = "";
							}

							if (letra == "%") {
								lsEmpresas.Add (insEmpresas (Convert.ToInt32 (codEmpresa), nome, caminhoWS, caminhoWSReserva, endereco, "x", cidade, bairro, "x", "x", status));

								temp = "";
								codEmpresa = "";
								nome = "";
								caminhoWS = "";
								caminhoWSReserva = "";
								endereco = "";
								cidade = "";
								bairro = "";
								status = "";
							}
							j++;
						}
					}

					if (lsEmpresas.Count > 0) {

																						
						empresas = lsEmpresas;
						lwEmpresas.Adapter = new adapter_empresas (this, empresas);
						lwEmpresas.ItemClick += OnListItemClick;

					} else {
						AlertDialog.Builder builder = new AlertDialog.Builder (this);
						builder.SetTitle ("ATENÇÃO");
						builder.SetMessage ("Nossa, aconteceu um erro grave. Tchau!!!");
						builder.SetCancelable (false);
						builder.SetPositiveButton ("OK", delegate {
						});
						builder.Show ();
					}
					});
			} catch  {

				//Toast.MakeText (this, ex.Message, ToastLength.Short).Show();


				//new System.Threading.Thread (new ThreadStart (delegate {	

				if (MainActivity.sCaminhoWSFast == MainActivity.sCaminhoWSFastReserva) {

					AlertDialog.Builder builder = new AlertDialog.Builder (this);
					builder.SetTitle ("ERRO DE CONEXÃO");
					builder.SetIcon (Android.Resource.Drawable.IcDialogAlert);
					builder.SetMessage ("Ops! Verifique a conexão da sua internet ou procure um Garçon!");
					builder.SetPositiveButton ("OK", delegate {


						Finish ();
						StartActivity (typeof(MainActivity));

					});
					builder.Show ();
				} else {

					MainActivity.sCaminhoWSFast = MainActivity.sCaminhoWSFastReserva;
					carregaEmpresa ();

				}
			
				//})).Start ();
			}
		}

		private void carregaCategoria()
		{		
			try {
				var webservice = new WS.IdmServerservice (MainActivity.sCaminhoWS);

				arCarregaCategoria = webservice.BeginCarregaCategoria (MainActivity.iCodEmpresa.ToString (), null, webservice);

				resultadoCarregaCategoria = webservice.EndCarregaCategoria (arCarregaCategoria);

				int i;
				int j;
				string temp;
				string letra;

				string codigo;
				string descricao;
				string ativo;

				if (resultadoCarregaCategoria != "") {				
					i = resultadoCarregaCategoria.Length;
					j = 0;
					temp = "";
					letra = "";

					codigo = "";
					descricao = "";
					ativo = "";

					while (j < i) {

						letra = resultadoCarregaCategoria [j].ToString ();

						if (letra != "|" & letra != "%") {
							temp = temp + letra;
						}

						if (letra == "|") {
							if (codigo == "")
								codigo = temp;
							else if (descricao == "")
								descricao = temp;
							else if (ativo == "")
								ativo = temp;

							temp = "";
						}

						if (letra == "%") {
							sqldb_categoria.AddRecord (Convert.ToInt32 (codigo), descricao, ativo);
							//lsEmpresas.Add (insEmpresas (Convert.ToInt32(codigo), descricao, ativo));
							temp = "";

							codigo = "";
							descricao = "";
							ativo = "";
						}

						j++;
					}
				}

			} catch {

				if (MainActivity.sCaminhoWS == MainActivity.sCaminhoWSReserva) {
					this.RunOnUiThread (delegate {

						AlertDialog.Builder builder = new AlertDialog.Builder (this);
						builder.SetTitle ("ERRO DE CONEXÃO");
						builder.SetIcon (Android.Resource.Drawable.IcDialogAlert);
						builder.SetMessage ("Ops! Verifique a conexão da sua internet ou procure um Garçon!");
						builder.SetPositiveButton ("OK", delegate {

							Finish ();
							StartActivity (typeof(MainActivity));

						});
						builder.Show ();

					});

				} else {

					MainActivity.sCaminhoWS = MainActivity.sCaminhoWSReserva;

					sqldb_categoria.DeleteRecord ();
					carregaCategoria ();

					sqldb_produto.DeleteRecord ();
					carregaProduto ();

				}
			}
		}

		private void carregaProduto()
		{
			try {
				var webservice = new WS.IdmServerservice (MainActivity.sCaminhoWS);

				arCarregaProduto = webservice.BeginCarregaProduto (MainActivity.iCodEmpresa.ToString (), null, webservice);

				resultadoCarregaProduto = webservice.EndCarregaEmpresa (arCarregaProduto);

				int i;
				int j;
				int iRecomeca;
				string temp;
				string letra;

				string idCategoria;
				string codProduto;
				string sDescricao;
				string sAtivo;
				string sDescricaoItens;
				string dPrecoUnitario;
				string sMensBoasVindas;

				if (resultadoCarregaProduto != "") {				
					i = resultadoCarregaProduto.Length;
					j = 0;
					iRecomeca = 0;
					temp = "";
					letra = "";

					idCategoria = "";
					codProduto = "";
					sDescricao = "";
					sAtivo = "";
					sDescricaoItens = "";
					dPrecoUnitario = "";
					sMensBoasVindas = "";

					while (j < i) {

						letra = resultadoCarregaProduto [j].ToString ();

						if (letra != "|" & letra != "%") {
							temp = temp + letra;
						}

						if (letra == "|") {
							if (idCategoria == "")
								idCategoria = temp;
							else if (codProduto == "")
								codProduto = temp;
							else if (sDescricao == "")
								sDescricao = temp;
							else if (sAtivo == "")
								sAtivo = temp;
							else if (sDescricaoItens == "")
								sDescricaoItens = temp;
							else if (dPrecoUnitario == "")
							{
								dPrecoUnitario = temp;
								iRecomeca = j;
							}
							else if (sMensBoasVindas == "")
							{
								sMensBoasVindas = temp;
								resultadoCarregaProduto.Replace(sMensBoasVindas,".");
								//j = iRecomeca;
							}

							temp = "";
						}

						if (letra == "%") {
							//public void AddRecord(int iIdCategoria, int iCodProduto, string sDescricao, string sAtivo, string sDescricaoItens, double dPrecoUnitario)

							sqldb_produto.AddRecord (Convert.ToInt32 (idCategoria), Convert.ToInt32 (codProduto), sDescricao, 
								sAtivo, sDescricaoItens, dPrecoUnitario);

							MainActivity.sMensagemBemVindo = sMensBoasVindas;

							temp = "";

							idCategoria = "";
							codProduto = "";
							sDescricao = "";
							sAtivo = "";
							sDescricaoItens = "";
							dPrecoUnitario = "";						
						}
						j++;
					}
				}

				Finish();
				StartActivity (typeof(CasaLoja));		
			} 
			catch {
				this.RunOnUiThread (delegate {

					AlertDialog.Builder builder = new AlertDialog.Builder (this);
					builder.SetTitle ("ERRO DE CONEXÃO");
					builder.SetIcon (Android.Resource.Drawable.IcDialogAlert);
					builder.SetMessage ("Ops! Verifique a conexão da sua internet ou procure um Garçon!");
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
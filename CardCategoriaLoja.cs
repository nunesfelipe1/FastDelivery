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
using Android.Support.V4.App;
using Java.Lang;
using Android.Media;
using Java.Util;

namespace Menu
{
	[Activity (Label = "FastDelivery", MainLauncher = false)]
	public class CardCategoriaLoja : Activity
	{
		private List<ListaCategoria> categoria = new List<ListaCategoria> ();

		private static readonly int ButtonClickNotificationId = 1000;

		System.Threading.Timer _timer;

		List<ListaMensagem> mensagens = new List<ListaMensagem> ();
		List<ListaMensagem> lsMensagens = new List<ListaMensagem> ();

		private DBCadCategoria sqldb_categoria;
		private DBCadCarrinho sqldb_carrinho;
		private DBCadMensagem sqldb_mensagem;

		Android.Database.ICursor sql_cursor = null;

		IAsyncResult arCarregaMensagem;
		string resultadoMensagem;

		string sDataPedido;

		public override bool OnKeyDown(Keycode keycode, KeyEvent @event)
		{
			bool handled = false;

			try{
				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle("CONFIRMAÇÃO");
				builder.SetIcon(Android.Resource.Drawable.IcDialogAlert);
				builder.SetMessage("Oi ! Você deseja sair do Menu?");
				builder.SetPositiveButton("Sim", delegate {

					Finish();
					StartActivity (typeof(InfLoja));

				});

				builder.SetNegativeButton ("Não", delegate { 
				
				});
				builder.SetCancelable(false);
				builder.Show();

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

			SetContentView (Resource.Layout.frmCardCategoriaLoja);

			if (MainActivity.iCodEmpresa > 0) {

				ListView lwCategoria = FindViewById<ListView> (Resource.Id.lwCategoria);
				TextView txtLoja = FindViewById<TextView> (Resource.Id.txtLoja);
				ImageButton btAnt = FindViewById<ImageButton> (Resource.Id.btAnt);
				ImageButton btCarrinho = FindViewById<ImageButton> (Resource.Id.imgBtCarrinho);

				sqldb_categoria = new DBCadCategoria ("delivery_db");
				sqldb_carrinho = new DBCadCarrinho ("delivery_db");
				sqldb_mensagem = new DBCadMensagem ("delivery_db");

				DBCadPedido sqldb_pedido = new DBCadPedido ("delivery_db");

				//Carregar a categoria pelo banco
				categoria = carregaCategoria ();

				lwCategoria.Adapter = new adapter_listview (this, categoria);
				lwCategoria.ItemClick += OnListItemClick;

				btAnt.Click += new EventHandler (btAnt_Click);
				btCarrinho.Click += new EventHandler (btCarrinho_Click);
				txtLoja.Text = MainActivity.sNomeEmpresa;

				//verifica se ja exzite
				sql_cursor = sqldb_carrinho.GetRecordCursor ("select * from CARRINHO where _codEmpresa = '" + MainActivity.iCodEmpresa +
				"' and numMesa = '" + MainActivity.iNumMesa + "' and dataFechado is null " +
				" and codigoMesa = '" + MainActivity.iCodMesa.ToString () + "'");

				if (sql_cursor.Count == 0) {
					//como o carrinho gerou 0, varifica se ja exite um pedido no banco local em aberto
					//isso porque o cliente ja poderia ter enviado um carrinho para o pedido e voltou para gerar mais carrinho, o pedido deve ser o mesmo
					sql_cursor = sqldb_pedido.GetRecordCursor ("select * from PEDIDO where _codEmpresa = '" + MainActivity.iCodEmpresa +
					"' and numMesa = '" + MainActivity.iNumMesa + "' and dataFechado is null " +
					" and codigoMesa = '" + MainActivity.iCodMesa.ToString () + "'");

					if (sql_cursor.Count > 0) {
						sql_cursor.MoveToFirst ();

						MainActivity.iCodPedido = sql_cursor.GetInt (0);
					} 
					else {
						//data do momento
						DateTime dData = DateTime.Now;

						sDataPedido = dData.ToString ("dd/MM/yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo);

						retornaProxCodigoPedido ();			
					}
				}

				verificaMensagem ();
			} 
			else 
			{
				Finish();
				StartActivity (typeof(MainActivity));
			}
		}

		private void verificaMensagem()
		{
			_timer = new System.Threading.Timer ((o) => {

				var webservice = new WS.IdmServerservice (MainActivity.sCaminhoWS.ToString ());

				arCarregaMensagem = webservice.BeginCarregaMensagem (MainActivity.iCodEmpresa.ToString (), null, webservice);

				resultadoMensagem = "";
				resultadoMensagem = webservice.EndCarregaMensagem (arCarregaMensagem);

				//ao enviar um item para a caixa, verificar se a mesa ja não foi fechada
				if (resultadoMensagem != "erro") {

					int i;
					int j;
					string temp;
					string letra;

					string codEmpresa;
					string codMensagem;
					string titulo;
					string subtitulo;
					string mensagem;
					string data;

					i = resultadoMensagem.Length;
					j = 0;
					temp = "";
					letra = "";

					codEmpresa = "";
					codMensagem = "";
					titulo = "";
					subtitulo = "";
					mensagem = "";
					data = "";

					while (j < i) {

						letra = resultadoMensagem [j].ToString ();

						if (letra != "|" & letra != "%") {
							temp = temp + letra;
						}

						if (letra == "|") {
							if (codEmpresa == "")
								codEmpresa = temp;
							else if (codMensagem == "")
								codMensagem = temp;
							else if (titulo == "")
								titulo = temp;
							else if (mensagem == "")
								mensagem = temp;
							else if (data == "")
								data = temp;
							else if (subtitulo == "")
								subtitulo = temp;	
						 
							temp = "";
						}

						if (letra == "%") {

							sql_cursor = sqldb_mensagem.GetRecordCursor (MainActivity.iCodEmpresa, Convert.ToInt32 (codMensagem));

							if (sql_cursor.Count == 0) {

								lsMensagens.Add (insMensagem (Convert.ToInt32 (codEmpresa), Convert.ToInt32 (codMensagem), titulo, mensagem, data, subtitulo));

								sqldb_mensagem.AddRecord (Convert.ToInt32 (codEmpresa), Convert.ToInt32 (codMensagem), titulo, mensagem, data, subtitulo);

								//NOTIFICACAO DE MENSAGEM

								Bundle valuesForActivity = new Bundle ();
								valuesForActivity.PutInt ("count", Convert.ToInt32 (codMensagem));

								// Create the PendingIntent with the back stack             
								// When the user clicks the notification, SecondActivity will start up.
								Intent resultIntent = new Intent (this, typeof(Mensagem));
								resultIntent.PutExtras (valuesForActivity); // Pass some values to SecondActivity.

								Android.Support.V4.App.TaskStackBuilder stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create (this);
								stackBuilder.AddParentStack (Class.FromType (typeof(Mensagem)));
								stackBuilder.AddNextIntent (resultIntent);

								PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent (0, (int)PendingIntentFlags.UpdateCurrent);

								// Build the notification
								NotificationCompat.Builder builder = new NotificationCompat.Builder (this)
								.SetAutoCancel (true) // dismiss the notification from the notification area when the user clicks on it
								.SetContentIntent (resultPendingIntent) // start up this activity when the user clicks the intent.
								.SetContentTitle (titulo) // Set the title
								.SetNumber (Convert.ToInt32 (codMensagem)) // Display the count in the Content Info
								.SetSmallIcon (Resource.Drawable.ic_stat_button_click) // This is the icon to display
								.SetSound (RingtoneManager.GetDefaultUri (RingtoneType.Notification))
								.SetContentText (subtitulo); // the message to display.
																
								//long[] pattern = {200,500};
								//builder.SetVibrate(pattern);

								// Finally publish the notification
								NotificationManager notificationManager = (NotificationManager)GetSystemService (Context.NotificationService);
								notificationManager.Notify (ButtonClickNotificationId, builder.Build ());

								MainActivity.sMensagem = mensagem;
							}

							temp = "";

							codEmpresa = "";
							codMensagem = "";
							titulo = "";
							subtitulo = "";
							mensagem = "";
							data = "";
						}
						j++;
					}

				}

			}, null, 0, 60000 * 2); //60.000 = 1 minuto
		}
	
		private ListaMensagem insMensagem(int codEmpresa, int codMensagem, string titulo, string mensagem, string data, string subtitulo){

			ListaMensagem lstMensagem = new ListaMensagem (codEmpresa, codMensagem, titulo, mensagem, data, subtitulo);
			return lstMensagem;		
		}

		void retornaProxCodigoPedido()
		{
			try {
				IAsyncResult arProxCodPedido;
				var webservice = new WS.IdmServerservice (MainActivity.sCaminhoWS.ToString ());

				arProxCodPedido = webservice.BeginRetornaProxCodPedido (MainActivity.iCodEmpresa.ToString (), null, webservice);
		
				var resultado = webservice.EndRetornaProxCodPedido (arProxCodPedido);

				if (resultado != "erro" && Convert.ToInt32 (resultado) > 0) {
					sqldb_carrinho.AddRecord (
						Convert.ToInt32 (resultado),
						MainActivity.iCodEmpresa,
						MainActivity.iNumMesa,
						sDataPedido,
						0,
						MainActivity.sIMEI,
						MainActivity.sNumCelular, 
						MainActivity.iCodMesa.ToString ());

					MainActivity.iCodPedido = Convert.ToInt32 (resultado);							
				} else if (resultado == "erro") {
					Toast.MakeText (this, "erro ao pegar o ...!", ToastLength.Short).Show ();
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

		void btCarrinho_Click(object sender, EventArgs e){
			Finish();
			StartActivity (typeof(Carrinho));
		}

		void btAnt_Click(object sender, EventArgs e){
			Finish();
			StartActivity (typeof(InfLoja));
		}

		void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
		{
			var listView = sender as ListView;
			var l = categoria [e.Position];
		
			MainActivity.id_categoria = Convert.ToInt32(l.codigo);

			if (listView != null)
			{
				Finish();
				StartActivity(typeof(CardCategoriaProdutoLoja));
			}
		}

		private List<ListaCategoria> carregaCategoria(){
			List<ListaCategoria> lsCategoria = new List<ListaCategoria> ();

			//percorrendo o retorno do select pelo getrecordcursor e retornando um objeto listacategoria
			sql_cursor = sqldb_categoria.GetRecordCursor ();

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

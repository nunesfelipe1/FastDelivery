using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Database.Sqlite;
using Android.Telephony;

namespace Menu
{
	[Activity (Label = "FastDelivery", MainLauncher = true)]
	public class MainActivity : Activity
	{
		public static string sIMEI;
		public static string sNumCelular;

		public static int iNumMesa;
		public static int iCodEmpresa;
		public static int iCodMesa;
		public static int iCodPedido;
		public static string sMensagemBemVindo;
		public static string sCaminhoWS;
		public static string sCaminhoWSReserva;
		public static string sCaminhoWSFast;
		public static string sCaminhoWSFastReserva;
		public static string sNomeEmpresa;
		public static string sMensagem;

		public static int id_categoria {set; get;} // um exemplo de string global
		public static int id_item_categorira {set; get;} // um exeplo de int global

		private TelephonyManager telephonyManager;

		public override bool OnKeyDown(Keycode keycode, KeyEvent @event)
		{
			bool handled = false;

			try{
				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle("CONFIRMAÇÃO");
				builder.SetIcon(Android.Resource.Drawable.IcDialogAlert);
				builder.SetMessage("Oi ! Você deseja sair do FastDelivery?");
				builder.SetPositiveButton("Sim", delegate {

					Finish();

				});

				builder.SetNegativeButton ("Não", delegate { 

					//StartActivity (typeof(MainActivity));

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

			SetContentView (Resource.Layout.Main);

			iCodEmpresa = 1;
			id_categoria = 0;
			id_item_categorira = 0;
			iNumMesa = 0;
			iCodPedido = 0;
			sMensagemBemVindo = "";
			sCaminhoWS = "";
			sCaminhoWSReserva = "";
			sCaminhoWSFast = "";
			sCaminhoWSFastReserva = "";
			sMensagem = "";

			ImageButton btProx = FindViewById<ImageButton> (Resource.Id.btProx);
			ImageButton btAnt = FindViewById<ImageButton> (Resource.Id.btAnt);
			ImageView imgInicio = FindViewById<ImageView> (Resource.Id.imgInicio);

			btProx.Click += new EventHandler(imgBtProx_Click);
			imgInicio.Click += new EventHandler(imgBtProx_Click);

			//Retorna informações do telefone - Numero de serie, numero do telefone
			telephonyManager = (TelephonyManager) GetSystemService(Context.TelephonyService);
				
			if (telephonyManager.DeviceId != null) {
				sIMEI = telephonyManager.DeviceId.ToString ();
			}

			if (telephonyManager.Line1Number != null) {
				sNumCelular = telephonyManager.Line1Number.ToString ();
			}

			//sCaminhoWSFast = "http://fd.webhop.me:8091/wsfastdelivery/WsFastDelivery.dll/soap/IdmServer";
			sCaminhoWSFast = "http://201.48.185.95:8091/wsfastdelivery/WsFastDelivery.dll/soap/IdmServer";
			//sCaminhoWSFast = "http://10.0.2.2:8091/wsfastdelivery/WsFastDelivery.dll/soap/IdmServer";
			//sCaminhoWSFast = "http://127.0.1.1:8091/wsfastdelivery/WsFastDelivery.dll/soap/IdmServer";
			//sCaminhoWSFast = "http://localhost:8091/wsfastdelivery/WsFastDelivery.dll/soap/IdmServer";

			sCaminhoWSFastReserva = "http://192.168.2.110:8091/wsfastdelivery/WsFastDelivery.dll/soap/IdmServer";
			//sCaminhoWSFastReserva = "http://10.0.2.2:8091/wsfastdelivery/WsFastDelivery.dll/soap/IdmServer";
		}

		void imgBtProx_Click(object Sender, EventArgs e)
		{
			Finish();
			StartActivity (typeof(CarrEmpresa));
		}
		//Toast.MakeText (this, "Anterior...!", ToastLength.Short).Show();
	}
}
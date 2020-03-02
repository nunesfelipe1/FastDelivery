using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Menu
{
	[Activity (Label = "FastDelivery", MainLauncher = false)]
	public class CasaLoja : Activity
	{

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

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.frmCasaLoja);

			if (MainActivity.iCodEmpresa > 0) {

				ImageButton imgBtAnt = FindViewById<ImageButton> (Resource.Id.imgBtAnt);
				ImageButton imgBtLoja = FindViewById<ImageButton> (Resource.Id.imgBtLoja);
				//ImageButton imgBtCasa = FindViewById<ImageButton> (Resource.Id.imgBtCasa);
				TextView txtLoja = FindViewById<TextView> (Resource.Id.txtLoja);

				imgBtAnt.Click += new EventHandler (imgBtAnt_Click);
				imgBtLoja.Click += new EventHandler (imgBtLoja_Click);
				//imgBtCasa.Click += new EventHandler (imgBtCasa_Click);
				txtLoja.Text = MainActivity.sNomeEmpresa;
			}
			else 
			{
				Finish();
				StartActivity (typeof(MainActivity));
			}
		}

		void imgBtAnt_Click(object Sender, EventArgs e)
		{
			//Toast.MakeText (this, "Anterior...!", ToastLength.Short).Show();
			Finish();
			StartActivity(typeof(MainActivity));
		}

		void imgBtLoja_Click(object Sender, EventArgs e)
		{
			//Toast.MakeText (this, "Anterior...!", ToastLength.Short).Show();
			Finish();
			StartActivity(typeof(InfLoja));
		}

		void imgBtCasa_Click(object Sender, EventArgs e)
		{
			//Toast.MakeText (this, "Anterior...!", ToastLength.Short).Show();
			Finish();
			StartActivity(typeof(infCasa));
		}
	}
}



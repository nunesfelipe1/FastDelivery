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
	public class infCasa: Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.frmInfCasa);

			ImageButton imgBtAnt = FindViewById<ImageButton> (Resource.Id.imgBtAnt);

			imgBtAnt.Click += new EventHandler (imgBtAnt_Click);
			//button.Click += delegate {button.Text = string.Format ("{0} clicks!", count++);};
		}

		void imgBtAnt_Click(object Sender, EventArgs e)
		{
			//Toast.MakeText (this, "Anterior...!", ToastLength.Short).Show();
			StartActivity(typeof(CasaLoja));
		}
	}
}



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
	public class Mensagem: Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.frmMensagem);

			TextView txtMensagem = FindViewById<TextView> (Resource.Id.textMensagem);
			ImageButton btAnt = FindViewById<ImageButton> (Resource.Id.btAntMens);

			btAnt.Click += new EventHandler (btAnt_Click);

			if (MainActivity.sMensagem.Length > 0) {
				txtMensagem.Text = MainActivity.sMensagem;
			}
		}

		void btAnt_Click(object sender, EventArgs e){
			Finish();
			StartActivity (typeof(CardCategoriaLoja));
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
	}
}



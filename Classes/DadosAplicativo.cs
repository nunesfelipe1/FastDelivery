using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database.Sqlite;
using Android.OS;

namespace Menu
{
	public class DadosAplicativo: BroadcastReceiver {

		public event EventHandler ConnectionStatusChanged;

		public override void OnReceive (Context context, Intent intent)
		{
			if (ConnectionStatusChanged != null)
				ConnectionStatusChanged (this, EventArgs.Empty);
		}
	}
}


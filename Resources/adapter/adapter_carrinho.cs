using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Menu
{
	class adapter_carrinho: BaseAdapter<ListaCarrinho>
	{
		List<ListaCarrinho> items;
		Activity context;

		public adapter_carrinho(Activity context, List<ListaCarrinho> items): base(){
			this.context = context;
			this.items = items;
		}

		#region implemented abstract members of BaseAdapter
		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = items [position];
			View view = convertView;

			if (view == null)
				view = context.LayoutInflater.Inflate (Resource.Layout.custom_carrinho, null);

			view.FindViewById<TextView> (Resource.Id.txtDescricao).Text = item.descricao;
			view.FindViewById<TextView> (Resource.Id.txtQtde).Text = "Qtde: " + item.qtde.ToString () + " - VlUn " + String.Format ("R$ : {0:0.00}  ", item.vlrUnit);
			view.FindViewById<TextView> (Resource.Id.txtvlrUnit).Text = "" + String.Format ("R$ : {0:0.00}  ", item.qtde * item.vlrUnit);

			return view;
		}

		public override int Count {
			get {return items.Count;}
		}

		#endregion
		#region implemented abstract members of BaseAdapter
		public override ListaCarrinho this [int position] {
			get { return items [position];}
		}
		#endregion
	}
}


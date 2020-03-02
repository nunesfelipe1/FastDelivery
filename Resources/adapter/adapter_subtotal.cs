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
	class adapter_subtotal: BaseAdapter<ListaSubTotal>
	{
		List<ListaSubTotal> items;
		Activity context;

		public adapter_subtotal(Activity context, List<ListaSubTotal> items): base(){
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
				view = context.LayoutInflater.Inflate (Resource.Layout.custom_sub_total, null);

			view.FindViewById<TextView> (Resource.Id.txtDescricao).Text = item.descricao;
			view.FindViewById<TextView> (Resource.Id.txtDescricaoItens).Text = "Qtde: " + item.qte.ToString () + " - VlUn " + String.Format ("R$ : {0:0.00}  ", item.preco_un);
			view.FindViewById<TextView> (Resource.Id.txtPreco).Text = "" + String.Format ("R$ : {0:0.00}  ", Convert.ToDouble(item.preco_un) *Convert.ToDouble(item.qte));

			return view;
		}

		public override int Count {
			get {return items.Count;}
		}

		#endregion
		#region implemented abstract members of BaseAdapter
		public override ListaSubTotal this [int position] {
			get { return items [position];}
		}
		#endregion
	}
}


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
	class adapter_listitem: BaseAdapter<ListaCategoriaProduto>
	{
		List<ListaCategoriaProduto> items;
		Activity context;

		public adapter_listitem(Activity context, List<ListaCategoriaProduto> items): base(){
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
				view = context.LayoutInflater.Inflate (Resource.Layout.custom_item_categoria, null);

			view.FindViewById<TextView> (Resource.Id.txtDescricaoItens).Text = item.descricaoItens.ToString ();
			view.FindViewById<TextView> (Resource.Id.txtDescricao).Text = item.descricao;
			view.FindViewById<TextView> (Resource.Id.txtPreco).Text = "" + String.Format ("R$ : {0:0.00}  ", Convert.ToDouble(item.preco_un));

			//int id_img = context.Resources.GetIdentifier ("ok1", "drawable", context.PackageName);
			//view.FindViewById<ImageView> (Resource.Id.imgItem).SetImageResource (id_img);

			return view;
		}

		public override int Count {
			get {return items.Count;}
		}

		#endregion
		#region implemented abstract members of BaseAdapter
		public override ListaCategoriaProduto this [int position] {
			get { return items [position];}
		}
		#endregion
	}
}


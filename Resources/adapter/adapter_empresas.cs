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
	class adapter_empresas: BaseAdapter<ListaEmpresas>
	{
		List<ListaEmpresas> items;
		Activity context;

		public adapter_empresas(Activity context, List<ListaEmpresas> items): base(){
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
				view = context.LayoutInflater.Inflate (Resource.Layout.custom_empresas, null);

			view.FindViewById<TextView> (Resource.Id.txtNome).Text = item.codEmpresa + " - " + item.nome;
			view.FindViewById<TextView> (Resource.Id.txtEndereco).Text = (item.endereco + " - " + item.bairro + " - " + item.cidade);

			//var imageViewById<ImageView> (Resource.Id.imgStatus);

			if (item.status == "ON")
				view.FindViewById<ImageView> (Resource.Id.imgStatus).SetImageResource (Resource.Drawable.ON);
			else if (item.status == "OFF")
				view.FindViewById<ImageView> (Resource.Id.imgStatus).SetImageResource (Resource.Drawable.off);

			return view;
		}

		public override int Count {
			get {return items.Count;}
		}

		#endregion
		#region implemented abstract members of BaseAdapter
		public override ListaEmpresas this [int position] {
			get { return items [position];}
		}
		#endregion
	}
}


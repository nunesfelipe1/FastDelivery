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
	public class MinhaListAdapter : BaseAdapter
	{
		private LayoutInflater mInflater;
		private List<ListaProduto> itens;

		public MinhaListAdapter()
		{

		}

		public MinhaListAdapter(Context context, List<ListaProduto> itens)
		{
			this.itens = itens;

			mInflater = LayoutInflater.From (context);
		}

		public override int Count
		{
			get { return itens.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return position;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View view, ViewGroup parent)
		{
			ItemSuporte itemHolder = new ItemSuporte();

			if (view == null) {

				view = mInflater.Inflate (Resource.Id.listView1, null);

				itemHolder.txtTitle = ((TextView) view.FindViewById (Resource.Id.textView14));
				itemHolder.imgIcon  = ((ImageView) view.FindViewById(Resource.Id.imageView1));

				view.SetTag (itemHolder);

			} else {

				itemHolder = (ItemSuporte) view.GetTag ();

			}

			return view;
		}

	}
}


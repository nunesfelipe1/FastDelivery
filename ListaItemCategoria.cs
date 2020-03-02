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
	class ListaItemCategoria
	{
		int _id_categoria;
		int _codigo;
		int _id_img;
		string _descricao;
		double _preco_un;

		public ListaItemCategoria(int id_categoria, int codigo, int id_img, String descricao, double preco_un){
			this._id_categoria = id_categoria;
			this._codigo = codigo;
			this._id_img = id_img;
			this._descricao = descricao;
			this._preco_un = preco_un;
		}

		public int id_categoria{
			get{
				return _id_categoria;
			}
			set{
				_id_categoria = value;
			}
		}

		public string descricao{
			get{
				return _descricao;
			}
			set{
				_descricao = value;
			}
		}

		public int id_img{
			get{
				return _id_img;
			}
			set{
				_id_img = value;
			}
		}

		public int codigo{
			get{
				return _codigo;
			}
			set{
				_codigo = value;
			}
		}
		public double preco_un{
			get{
				return _preco_un;
			}
			set{
				_preco_un = value;
			}
		}
	}
}


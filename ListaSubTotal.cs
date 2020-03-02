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
	class ListaSubTotal
	{
		string _qte;
		string _descricao;
		string _preco_un;

		public ListaSubTotal(string qte, string descricao, string preco_un){
			this._qte = qte;
			this._descricao = descricao;
			this._preco_un = preco_un;
		}

		public string qte{
			get{
				return _qte;
			}
			set{
				_qte = value;
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

		public string preco_un{
			get{
				return _preco_un;
			}
			set{
				_preco_un = value;
			}
		}
	}
}


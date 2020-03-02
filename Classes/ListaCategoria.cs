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
	class ListaCategoria
	{
		int _codigo;
		string _descricao;
		string _ativo;

		public ListaCategoria(int codigo, string descricao, string ativo){
			this._codigo = codigo;
			this._descricao = descricao;
			this._ativo = ativo;
		}

		public int codigo{
			get{
				return _codigo;
			}
			set{
				_codigo = value;
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

		public string ativo{
			get{
				return _ativo;
			}
			set{
				_ativo = value;
			}
		}

	}
}


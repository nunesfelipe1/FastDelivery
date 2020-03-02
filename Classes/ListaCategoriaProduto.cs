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
	class ListaCategoriaProduto
	{
		int _id_categoria;
		int _codigo;
		string _descricao;
		string _ativo;
		string _descricaoItens;
		double _preco_un;

		public ListaCategoriaProduto(int id_categoria, int codigo, string descricao, string ativo, string descricaoItens, double preco_un){
			this._id_categoria = id_categoria;
			this._codigo = codigo;
			this._descricao = descricao;
			this._ativo = ativo;
			this._descricaoItens = descricaoItens;
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

		public string descricaoItens{
			get{
				return _descricaoItens;
			}
			set{
				_descricaoItens = value;
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


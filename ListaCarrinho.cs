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
	class ListaCarrinho
	{
		int _codEmpresa;
		int _codPedido;
		int _itemPedido;
		int _codProduto;
		string _descricao;
		double _qtde;
		double _vlrUnit;
		string _numSerial;
		string _numCelular;
		string _lembrete;

		public ListaCarrinho(int codEmpresa, int codPedido, int itemPedido, int codProduto, 
			string descricao, double qtde,	double vlrUnit, string numSerial, string numCelular, string lembrete){

			this._codEmpresa = codEmpresa;
			this._codPedido = codPedido;
			this._itemPedido = itemPedido;
			this._codProduto = codProduto;
			this._descricao = descricao;
			this._qtde = qtde;
			this._vlrUnit = vlrUnit;
			this._numSerial = numSerial;
			this._numCelular = numCelular;
			this._lembrete = lembrete;
		}

		public int codEmpresa{
			get{
				return _codEmpresa;
			}
			set{
				_codEmpresa = value;
			}
		}

		public int codPedido{
			get{
				return _codPedido;
			}
			set{
				_codPedido = value;
			}
		}

		public int itemPedido{
			get{
				return _itemPedido;
			}
			set{
				_itemPedido = value;
			}
		}

		public int codProduto{
			get{
				return _codProduto;
			}
			set{
				_codProduto = value;
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

		public double qtde{
			get{
				return _qtde;
			}
			set{
				_qtde = value;
			}
		}

		public double vlrUnit{
			get{
				return _vlrUnit;
			}
			set{
				_vlrUnit = value;
			}
		}

		public string numSerial{
			get{
				return _numSerial;
			}
			set{
				_numSerial = value;
			}
		}

		public string numCelular{
			get{
				return _numCelular;
			}
			set{
				_numCelular = value;
			}
		}

		public string lembrete{
			get{
				return _lembrete;
			}
			set{
				_lembrete = value;
			}
		}

	}
}


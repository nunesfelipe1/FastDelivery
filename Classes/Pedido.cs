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
	class Pedido
	{
		int _codPedido;
		int _codEmpresa;
		int _numMesa;
		string _dataPedido;
		double _valorTotal;
		string _numSerial;
		string _numCelular;

		public Pedido(int codPedido, int codEmpresa, int numMesa, string dataPedido, double valorTotal, string numSerial, string numCelular){
			this._codPedido = codPedido;
			this._codEmpresa = codEmpresa;
			this._numMesa = numMesa;
			this._dataPedido = dataPedido;
			this._valorTotal = valorTotal;
			this._numSerial = numSerial;
			this._numCelular = numCelular;
		}

		public int codPedido{
			get{
				return _codPedido;
			}
			set{
				_codPedido = value;
			}
		}

		public int codEmpresa{
			get{
				return _codEmpresa;
			}
			set{
				_codEmpresa = value;
			}
		}

		public int numMesa{
			get{
				return _numMesa;
			}
			set{
				_numMesa = value;
			}
		}

		public string dataPedido{
			get{
				return _dataPedido;
			}
			set{
				_dataPedido = value;
			}
		}

		public double valorTotal{
			get{
				return _valorTotal;
			}
			set{
				_valorTotal = value;
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

	}
}


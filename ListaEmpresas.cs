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
	class ListaEmpresas
	{
		int _codEmpresa;
		string _nome;
		string _caminhoWS;
		string _caminhoWSReserva;
		string _endereco;
		string _telefone;
		string _cidade;
		string _bairro;
		string _cnpj;
		string _responsavel;
		string _status;


		public ListaEmpresas(int codEmpresa, string nome, string caminhoWS, string caminhoWSReserva, string endereco, 
			string telefone, string cidade,	string bairro, string cnpj, string responsavel, string status){

			this._codEmpresa = codEmpresa;
			this._nome = nome;
			this._caminhoWS = caminhoWS;
			this._caminhoWSReserva = caminhoWSReserva;
			this._endereco = endereco;
			this._telefone = telefone;
			this._cidade = cidade;
			this._bairro = bairro;
			this._cnpj = cnpj;
			this._responsavel = responsavel;
			this._status = status;
		}

		public int codEmpresa{
			get{
				return _codEmpresa;
			}
			set{
				_codEmpresa = value;
			}
		}

		public string nome{
			get{
				return _nome;
			}
			set{
				_nome = value;
			}
		}

		public string caminhoWS{
			get{
				return _caminhoWS;
			}
			set{
				_caminhoWS = value;
			}
		}
		public string caminhoWSReserva{
			get{
				return _caminhoWSReserva;
			}
			set{
				_caminhoWSReserva = value;
			}
		}

		public string endereco{
			get{
				return _endereco;
			}
			set{
				_endereco = value;
			}
		}

		public string telefone{
			get{
				return _telefone;
			}
			set{
				_telefone = value;
			}
		}

		public string cidade{
			get{
				return _cidade;
			}
			set{
				_cidade = value;
			}
		}

		public string bairro{
			get{
				return _bairro;
			}
			set{
				_bairro = value;
			}
		}

		public string cnpj{
			get{
				return _cnpj;
			}
			set{
				_cnpj = value;
			}
		}

		public string responsavel{
			get{
				return _responsavel;
			}
			set{
				_responsavel = value;
			}
		}
		public string status{
			get{
				return _status;
			}
			set{
				_status = value;
			}
		}
	}
}


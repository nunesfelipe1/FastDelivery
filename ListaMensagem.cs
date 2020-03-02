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
	class ListaMensagem
	{
		int _codEmpresa;
		int _codMensagem;
		string _titulo;
		string _subtitulo;
		string _mensagem;
		string _data;

		public ListaMensagem(int codEmpresa, int codMensagem, string titulo, string subtitulo, string mensagem, string data){
			this._codEmpresa = codEmpresa;
			this._codMensagem = codMensagem;
			this._titulo = titulo;
			this._subtitulo = subtitulo;
			this._mensagem = mensagem;
			this._data = data;
		}

		public int codEmpresa{
			get{
				return _codEmpresa;
			}
			set{
				_codEmpresa = value;
			}
		}

		public int codMensagem{
			get{
				return _codMensagem;
			}
			set{
				_codMensagem = value;
			}
		}

		public string titulo{
			get{
				return _titulo;
			}
			set{
				_titulo = value;
			}
		}

		public string subtitulo{
			get{
				return _subtitulo;
			}
			set{
				_subtitulo = value;
			}
		}

		public string mensagem{
			get{
				return _mensagem;
			}
			set{
				_mensagem = value;
			}
		}

		public string data{
			get{
				return _data;
			}
			set{
				_data = value;
			}
		}
	}
}


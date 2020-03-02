
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
	public class ListViewAdapter: BaseAdapter
	{
		List<Contact> _contactList;
		Activity _activity;

		public ListViewAdapter (Activity activity)
		{
			_activity = activity;
			FillContacts ();
		}

		void FillContacts ()
		{
			var uri = ContactsContract.Contacts.ContentUri;

			string[] projection = {
				ContactsContract.Contacts.InterfaceConsts.Id,
				ContactsContract.Contacts.InterfaceConsts.DisplayName,
				ContactsContract.Contacts.InterfaceConsts.PhotoId
			};

			var cursor = _activity.ManagedQuery (uri, projection, null,
				null, null);

			_contactList = new List<Contact> ();

			if (cursor.MoveToFirst ()) {
				do {
					_contactList.Add (new Contact{
						Id = cursor.GetLong (
							cursor.GetColumnIndex (projection [0])),
						DisplayName = cursor.GetString (
							cursor.GetColumnIndex (projection [1])),
						PhotoId = cursor.GetString (
							cursor.GetColumnIndex (projection [2]))
					});
				} while (cursor.MoveToNext());
			}
		}

		class Contact
		{
			public long Id { get; set; }
			public string DisplayName{ get; set; }
			public string PhotoId { get; set; }
		}
	}
}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DownloadLibrary;

namespace PodDownloader
{
	class PodListAdapter : BaseAdapter<PodAddressListPodAddress>
	{
		private List<PodAddressListPodAddress> addressList;
		private Activity activity;

		public PodListAdapter(Activity activity)
		{
			this.activity = activity;
			FillList();
		}

		public override PodAddressListPodAddress this[int position] => addressList[position];

		public override int Count => addressList.Count;

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.PodListItem, null);
			var iconUrl = view.FindViewById<ImageView>(Resource.Id.ListItemIcon);
			var title = view.FindViewById<TextView>(Resource.Id.ListItemTitle);

			iconUrl.SetImageURI(Android.Net.Uri.Parse(addressList[position].ImageUrl));
			title.Text = addressList[position].Name;

			return view;
		}

		private void FillList()
		{
			addressList = AddressBuilder.Load(activity.Assets.Open("PodAddress.xml"))
				.PodAddress
				.Select(addr => addr).ToList();
		}
	}

	[Activity(Label = "OptionSelectActivity")]
	public class OptionSelectActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Option);

			var podListView = FindViewById<ListView>(Resource.Id.podListView);
			podListView.Adapter = new PodListAdapter(this);

			var button = FindViewById<Button>(Resource.Id.okButton);
			button.Click += (object sender, EventArgs e) =>
			{
				Finish();
			};
		}

	}
}
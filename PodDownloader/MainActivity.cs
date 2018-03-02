using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Net;
using System.IO;

namespace PodDownloader
{
	[Activity(Label = "PodDownloader", MainLauncher = true)]
	public class MainActivity : Activity
	{
		private ArrayAdapter adapter;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			var messageList = FindViewById<ListView>(Resource.Id.msgList);
			adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1);
			messageList.Adapter = adapter;
			var progressBar = FindViewById<ProgressBar>(Resource.Id.downloadProgressBar);
			progressBar.Max = 100;

			var datePicker = FindViewById<DatePicker>(Resource.Id.downloadDatePicker);
			var button = FindViewById<Button>(Resource.Id.downloadButton);
			button.Click += async (object sender, EventArgs e) =>
			{
				var cm = GetSystemService(ConnectivityService) as ConnectivityManager;
				if (cm?.ActiveNetworkInfo.Type != ConnectivityType.Wifi)
				{
					var builder = new AlertDialog.Builder(this);
					builder.SetMessage("Connect without wifi?");
					builder.SetNegativeButton("Cancel", (obj, x) => { });
					builder.SetPositiveButton("Ok", async (obj, which) =>
					{
						await StartBatchDownload();
					});
					builder.Show();
				}
				else
					await StartBatchDownload();
			};

			Task StartBatchDownload()
			{
				return Task.Run(() =>
				{
					var savePath = GetSavePath();

					BatchDownloader.Date = datePicker.DateTime;
					List<string> failedList = new List<string>();
					failedList.AddRange(BatchDownloader.DownloadJungsNewsShow(savePath));
					failedList.AddRange(BatchDownloader.DownloadKimsNewsFactory(savePath));
					failedList.AddRange(BatchDownloader.DownloadKimsNewsShow(savePath));

					RunOnUiThread(() =>
					{
						failedList
							.Select(msg => "failed url: " + msg)
							.ToList()
							.ForEach(msg =>
							{
								Log.Info(typeof(MainActivity).ToString(), msg);
								adapter.Add(msg);
							});
						adapter.NotifyDataSetChanged();
						Toast.MakeText(this, "Download complete", ToastLength.Long).Show();
					});
				});
			}
		}

		private string GetSavePath(bool useInternal = false)
		{
			string internalPath = FilesDir.AbsolutePath + '/';
			if (useInternal)
				return internalPath;
			else
			{
				if (Android.OS.Environment.ExternalStorageState == Android.OS.Environment.MediaMounted)
				{
					var downloadFolderPath = Android.OS.Environment.GetExternalStoragePublicDirectory(
						Android.OS.Environment.DirectoryDownloads).AbsolutePath + '/' + "PodDownload";
					if (!Directory.Exists(downloadFolderPath))
						Directory.CreateDirectory(downloadFolderPath);
					return downloadFolderPath + "/";
				}
				else
					return internalPath;
			}
		}

		private void SetAlarms()
		{
			/*
			AlarmManager alarm = (AlarmManager)GetSystemService(Context.AlarmService);

			var futureDate = DateTime.Now.Date + new TimeSpan(10, 0, 0);
			if (DateTime.Now.Hour > 10)
				futureDate.AddHours(24);

			Intent intent = new Intent(this, typeof(BatchDownloader));

			PendingIntent sender = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);
			alarm.Set(AlarmType.RtcWakeup, futureDate.Millisecond, sender);
			*/
		}
	}
}


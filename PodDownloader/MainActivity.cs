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

namespace PodDownloader
{
	[Activity(Label = "PodDownloader", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
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

					List<string> failedList = new List<string>();
					failedList.AddRange(BatchDownloader.DownloadJungsNewsShow(savePath));
					failedList.AddRange(BatchDownloader.DownloadKimsNewsFactory(savePath));
					failedList.AddRange(BatchDownloader.DownloadKimsNewsShow(savePath));
					failedList
						.Select(msg => "failed url: " + msg)
						.ToList()
						.ForEach(msg => Log.Info(typeof(MainActivity).ToString(), msg));
				});
			}
		}

		private string GetSavePath(bool useInternal = false)
		{
			if (useInternal)
				return FilesDir.AbsolutePath + '/';
			else
			{
				return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + '/'
					+ Android.OS.Environment.DirectoryDownloads + '/';
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


using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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
				await Task.Run(() =>
				{
					List<string> failedList = new List<string>();
					failedList.AddRange(BatchDownloader.DownloadJungsNewsShow());
					failedList.AddRange(BatchDownloader.DownloadKimsNewsFactory());
					failedList.AddRange(BatchDownloader.DownloadKimsNewsShow());
					failedList
						.Select(msg => "failed url: " + msg)
						.ToList()
						.ForEach(msg => Log.Info(typeof(MainActivity).ToString(), msg));
				});
			};
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


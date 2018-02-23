using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
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
					BatchDownloader.DownloadJungsNewsShow();
					BatchDownloader.DownloadKimsNewsFactory();
					BatchDownloader.DownloadKimsNewsShow();
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


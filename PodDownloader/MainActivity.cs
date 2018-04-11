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
using System.Net;
using DownloadLibrary;
using Android.Icu.Util;

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

			var selectButton = FindViewById<Button>(Resource.Id.selectButton);
			selectButton.Click += (object sender, EventArgs e) =>
			{
				StartActivity(typeof(OptionSelectActivity));
			};

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

			SetAlarms();

			Task StartBatchDownload()
			{
				return Task.Run(() =>
				{
					List<string> failedList = Download(datePicker.DateTime, DownloadProgressHandler);

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
						adapter.Add("Download complete");
						adapter.NotifyDataSetChanged();
						Toast.MakeText(this, "Download complete", ToastLength.Long).Show();
					});
				});
			}

			void DownloadProgressHandler(object sender, DownloadProgressChangedEventArgs e)
			{
				progressBar.SetProgress(e.ProgressPercentage, true);
			}
		}

		private List<string> Download(DateTime date, DownloadProgressChangedEventHandler handler)
		{
			var savePath = GetSavePath();

			AddressBuilder.Date = date;
			var addrs = AddressBuilder.Load(Assets.Open("PodAddress.xml"));
			var urls = AddressBuilder.GetEffectiveAddresses(addrs);

			var workset = BatchDownloader.DownloadFromUrls(savePath, urls, handler);
			List<string> failedList = new List<string>();
			List<string> successList = new List<string>();
			foreach (var (url, success) in workset)
			{
				if (success)
					successList.Add(url);
				else
					failedList.Add(url);
			}

			ScanMedia(successList);

			return failedList;
		}

		private void ScanMedia(List<string> workset)
		{
			foreach (var item in workset)
			{
				var file = new Java.IO.File(item);
				var uri = Android.Net.Uri.FromFile(file);
				var scanItent = new Intent(Intent.ActionMediaScannerScanFile, uri);
				SendBroadcast(scanItent);
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
			void Callback()
			{
				Task.Run(() => Download(DateTime.Now, null));
			}

			NotificationReceiver.SetAction(Callback);
			NotificationReceiver.SetAlarms(this);
		}
	}

	[BroadcastReceiver]
	public class NotificationReceiver : BroadcastReceiver
	{
		private static Action actionTodo;

		public override void OnReceive(Context context, Intent intent)
		{
			var cm = context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;
			if (cm?.ActiveNetworkInfo.Type == ConnectivityType.Wifi)
				actionTodo();
		}

		public static void SetAction(Action action) => actionTodo = action;

		public static void SetAlarms(Context context)
		{
			AlarmManager alarm = (AlarmManager)context.GetSystemService(Context.AlarmService);

			Calendar calendar = Calendar.Instance;
			calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();
			calendar.Set(CalendarField.HourOfDay, 15);
			calendar.Set(CalendarField.Minute, 00);
			calendar.Set(CalendarField.Second, 0);

			Intent intent = new Intent(context, typeof(NotificationReceiver));
			PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
			alarm.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, AlarmManager.IntervalDay, pendingIntent);
		}
	}
}


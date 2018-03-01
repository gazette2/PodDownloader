using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PodDownloader
{
	public static class BatchDownloader
	{
		private static string year = DateTime.Now.Year.ToString();
		private static string month = DateTime.Now.Month.ToString("D2");
		private static string day = DateTime.Now.Day.ToString("D2");

		public static DateTime Date
		{
			get
			{
				return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
			}
			set
			{
				year = value.Year.ToString();
				month = value.Month.ToString("D2");
				day = value.Day.ToString("D2");
			}
		}

		public static List<string> DownloadKimsNewsFactory(string savePath)
		{
			List<string> failedFileList = new List<string>();
			using (WebClient client = new WebClient())
			{
				string podBase = "http://cdn.podbbang.com/data1/tbsadm/";
				for (int i = 1; i <= 2; i++)
				{
					string fileName = $"nf{year.Substring(2)}{month}{day}00{i}.mp3";
					string podAddress = podBase + fileName;

					try
					{
						var path = savePath + fileName;
						client.DownloadFile(podAddress, path);
					}
					catch (WebException)
					{
						failedFileList.Add(podAddress);
					}
				}
			}
			return failedFileList;
		}

		public static List<string> DownloadJungsNewsShow(string savePath)
		{
			List<string> failedFileList = new List<string>();
			using (WebClient client = new WebClient())
			{
				StringBuilder podBase = new StringBuilder("http://podcastfile2.sbs.co.kr/powerfm/");
				podBase.Append(year);
				podBase.Append('/');
				podBase.Append(month);
				podBase.Append('/');

				foreach (var i in new[] { 1, 2, 4 })
				{
					string fileName = $"love-v2000010280-{year}{month}{day}(10-0{i}).mp3";
					string podAddress = podBase + fileName;

					try
					{
						var path = savePath + fileName;
						client.DownloadFile(podAddress, path);
					}
					catch (WebException)
					{
						failedFileList.Add(podAddress);
					}
				}
			}
			return failedFileList;
		}

		public static List<string> DownloadKimsNewsShow(string savePath)
		{
			List<string> failedFileList = new List<string>();
			using (WebClient client = new WebClient())
			{
				string podBase = "http://podcast.cbs-vod.gscdn.com/cbsv/cbsaod/newshow/";
				for (int i = 1; i <= 2; i++)
				{
					string fileName = $"{year}{month}{day}newsshow{i}.mp3";
					string podAddress = podBase + fileName;

					try
					{
						var path = savePath + fileName;
						client.DownloadFile(podAddress, path);
					}
					catch (WebException)
					{
						failedFileList.Add(podAddress);
					}
				}
			}
			return failedFileList;
		}
	}
}
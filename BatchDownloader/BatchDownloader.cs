using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DownloadLibrary
{
	public static class BatchDownloader
	{
		public static List<(string url, bool success)> DownloadFromUrls(string savePath, List<string> urls, DownloadProgressChangedEventHandler eventHandler)
		{
			List<(string, bool)> workList = new List<(string, bool)>();
			using (WebClient client = new WebClient())
			{
				foreach (var url in urls)
				{
					var path = savePath + Path.GetFileName(url);

					try
					{
						client.DownloadProgressChanged += eventHandler;
						client.DownloadFileTaskAsync(url, path).Wait();
						workList.Add((url, true));
					}
					catch (Exception)
					{
						workList.Add((url, false));
					}
				}
			}
			return workList;
		}
	}
}
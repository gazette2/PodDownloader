using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DownloadLibrary
{
	public static class BatchDownloader
	{
		public static List<string> DownloadFromUrls(string savePath, List<string> urls, DownloadProgressChangedEventHandler eventHandler)
		{
			List<string> failedFileList = new List<string>();
			using (WebClient client = new WebClient())
			{
				foreach (var url in urls)
				{
					var path = savePath + Path.GetFileName(url);

					try
					{
						client.DownloadProgressChanged += eventHandler;
						client.DownloadFileTaskAsync(url, path).Wait();
					}
					catch (Exception)
					{
						failedFileList.Add(url);
					}
				}
			}
			return failedFileList;
		}
	}
}
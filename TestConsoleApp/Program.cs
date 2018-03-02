using System;
using System.Net;
using PodDownloader;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
			BatchDownloader.DownloadJungsNewsShow("./", DownloadProgressHandler);
			BatchDownloader.DownloadKimsNewsFactory("./", DownloadProgressHandler);
			BatchDownloader.DownloadKimsNewsShow("./", DownloadProgressHandler);
		}

		static void DownloadProgressHandler(object obj, DownloadProgressChangedEventArgs arg)
		{
			Console.WriteLine(arg.ProgressPercentage);
		}
	}
}

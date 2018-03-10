using System;
using System.Net;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
			DownloadLibrary.BatchDownloader.DownloadJungsNewsShow("./", DownloadProgressHandler);
			DownloadLibrary.BatchDownloader.DownloadKimsNewsFactory("./", DownloadProgressHandler);
			DownloadLibrary.BatchDownloader.DownloadKimsNewsShow("./", DownloadProgressHandler);
		}

		static void DownloadProgressHandler(object obj, DownloadProgressChangedEventArgs arg)
		{
			Console.WriteLine(arg.ProgressPercentage);
		}
	}
}

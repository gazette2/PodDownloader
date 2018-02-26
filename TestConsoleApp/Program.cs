using System;
using PodDownloader;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
			//BatchDownloader.DownloadJungsNewsShow("./");
			BatchDownloader.DownloadKimsNewsFactory("./");
			//BatchDownloader.DownloadKimsNewsShow("./");
		}
	}
}

using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using DownloadLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
	[TestClass]
	public class StringTest
	{
		[TestMethod]
		public void XmlLoadTest()
		{
			var addressList = AddressBuilder.Load("PodAddress.xml");
			var urls = AddressBuilder.GetEffectiveAddresses(addressList);
			foreach (var url in urls)
			{
				Console.WriteLine(url);
			}
		}

		[TestMethod]
		public void XmlLoadFromStreamTest()
		{
			FileStream stream = new FileStream("PodAddress.xml", FileMode.Open);
			var lists = AddressBuilder.Load(stream);
			Assert.IsNotNull(lists);
		}

		[TestMethod]
		public void PathGetFileNameTest()
		{
			string path = "http://podcastfile2.sbs.co.kr/powerfm/love-v2000010280-19740417(10-01).mp3";
			var result = Path.GetFileName(path);
			Assert.AreEqual("love-v2000010280-19740417(10-01).mp3", result);
		}
	}
}

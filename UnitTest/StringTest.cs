using System;
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
	}
}

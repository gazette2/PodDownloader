using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DownloadLibrary
{
    public static class AddressBuilder
    {
		public static PodAddressList Load(string path)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PodAddressList));
			using (var fileStream = new FileStream(path, FileMode.Open))
			{
				return xmlSerializer.Deserialize(fileStream) as PodAddressList;
			}
		}

		public static List<string> GetEffectiveAddresses(PodAddressList addressList)
		{
			List<string> result = new List<string>();
			foreach (var item in addressList.PodAddress)
			{
				foreach (var i in item.SequenceNumber)
				{
					result.Add(Replace(item.AddressTemplate, i));
				}
			}
			return result;
		}

		private static string Replace(string template, int index)
		{
			string year = DateTime.Now.Year.ToString();
			string month = DateTime.Now.Month.ToString("D2");
			string day = DateTime.Now.Day.ToString("D2");

			StringBuilder temp = new StringBuilder(template);
			temp.Replace("{Y}", year);
			temp.Replace("{y}", year.Substring(2));
			temp.Replace("{m}", month);
			temp.Replace("{d}", day);
			temp.Replace("{i}", index.ToString());

			return temp.ToString();
		}
	}
}

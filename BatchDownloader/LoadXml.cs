using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DownloadLibrary
{
    public static class AddressBuilder
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

		public static PodAddressList Load(string path)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PodAddressList));
			using (var fileStream = new FileStream(path, FileMode.Open))
			{
				return xmlSerializer.Deserialize(fileStream) as PodAddressList;
			}
		}

		public static PodAddressList Load(Stream stream)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PodAddressList));
			return xmlSerializer.Deserialize(stream) as PodAddressList;
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

using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Cbms.Kms.Domain.Helpers
{
    public static class ApiHelper
	{
		public static async Task<string> RestfulPostAsync(string apiUrl, object dataRequest, string token = null, string cookie = null)
		{
			HttpClient client = new HttpClient()
			{
				BaseAddress = new Uri(apiUrl)
			};

			if (token != null ) {
				client.DefaultRequestHeaders.Add("Token", token);
			}		
			if (cookie != null) {
				client.DefaultRequestHeaders.Add("Cookie", cookie);
			}

			string data = JsonConvert.SerializeObject(dataRequest);
			HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

			var rp = await client.PostAsync("", content);
			return await rp.Content.ReadAsStringAsync();
		}

		public static async Task<string> CrmSOAPPostAsync(string apiUrl, string username, string password, string xMLData)
		{
			HttpClient client = new HttpClient()
			{
				BaseAddress = new Uri(apiUrl)
			};

			//XML Data Process
			string xmlPostData = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
				"xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">\r\n<soap12:Body>\r\n<GetOrderStatusUpdate " +
				"xmlns=\"http://tempuri.org/\">\r\n" +
				"<username>" + username + "</username>\r\n" +
				"<password>" + password + "</password>\r\n" +
				"<XMLData>{0}</XMLData>\r\n</GetOrderStatusUpdate>\r\n</soap12:Body>\r\n</soap12:Envelope>";

			xmlPostData = string.Format(xmlPostData, xMLData);

			var workItem = new XmlDocument();
			workItem.PreserveWhitespace = true;
			workItem.LoadXml(xmlPostData);
		
			var httpContent = new StringContent(workItem.OuterXml.ToString(), Encoding.UTF8, "application/soap+xml");
			var respone = await client.PostAsync(apiUrl, httpContent);			

			return await respone.Content.ReadAsStringAsync();
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using System.Collections.Specialized;
using Bot_App1.FormFlow;
using System;
using System.IO;
using System.Web;

namespace Bot_App1.Service
{

    public class Parser
    {
        string url;
        WebClient client;

        public Parser(string url)
        {
            this.url = url;
            client = new WebClient();
        }

        public string Load(FlatParameters parameters)   
        {
                var formData = new NameValueCollection();
                formData["tx_uedbflat_pi2[DATA][town_id][e]"] = parameters.Town;
                formData["tx_uedbflat_pi2[DATA][x_count_pictures][ge]"] = "1"; //with foto
                formData["tx_uedbflat_pi2[sort_by][0]"] = "date_revision"; //sort by date
                formData["tx_uedbflat_pi2[DATA][rooms][e][1]"] = parameters.Quantity;
                formData["tx_uedbflat_pi2[DATA][building_year][ge]"] = parameters.StartYear;
                formData["tx_uedbflat_pi2[DATA][price_m2][le]"] = parameters.Price;
                var responseBytes = client.UploadValues(url, "POST", formData);
                return Encoding.UTF8.GetString(responseBytes);           
        }

         
        public IEnumerable<FlatData> Parse(string text)  
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(text);  

                var nodes = doc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "bd-item ").Take(10);
                return nodes.Select(node => new FlatData
                {
                    ImageSrc = node.Descendants("img").FirstOrDefault().Attributes["data-original"].Value,
                    Title = node.ChildNodes.Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "title").FirstOrDefault().InnerText,
                    Link = node.Descendants("a").FirstOrDefault().Attributes["href"].Value
                });
            }
            catch (Exception e)
            {
                return new[] { new FlatData { Title = e.Message, ImageSrc = "http://lorempixel.com/200/200/food", Link = "https://stackoverflow.com/questions/2159361/error-502-bad-gateway-when-sending-a-request-with-httpwebrequest-over-ssl" } };
            }
        }



           
        

    }
}

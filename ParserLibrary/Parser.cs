using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.ComponentModel;
using System.Collections.Specialized;

namespace ParserLibrary
{
    public class Parser
    {
        public string Url { get; set; }

        public Parser(string region)
        {
            Url = $"https://realt.by/{region}-region/sale/flats/search/"; 
            //delete it and move to property
        }

        public  IEnumerable<FlatData>  GetInfo(string town)  
        {
            // var doc = web.Load(Url);

            /* var client = new WebClient();
             var data = client.DownloadData(Url);
             var page=System.Text.Encoding.UTF8.GetString(data);
             HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
             doc.LoadHtml(page);*/


            var formData = new NameValueCollection();
            formData["tx_uedbflat_pi2[DATA][town_id][e]"] = town;
            formData["tx_uedbflat_pi2[DATA][x_count_pictures][ge]"] = "1";
    

            var client = new WebClient();
            var responseBytes = client.UploadValues(Url, "POST", formData);
            var page = Encoding.UTF8.GetString(responseBytes);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            var nodes = doc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value=="bd-item ").Take(10);
             return nodes.Select(node => new FlatData
             {
                 ImageSrc = node.Descendants("img").FirstOrDefault().Attributes["data-original"].Value,
                 Title = node.ChildNodes.Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "title").FirstOrDefault().InnerText,
                 Link = node.Descendants("a").FirstOrDefault().Attributes["href"].Value
             });
        }
 
    }
}


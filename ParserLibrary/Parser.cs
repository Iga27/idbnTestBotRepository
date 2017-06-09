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

        HtmlWeb web;

        public string Url { get; set; }

        public Parser()
        {
            web = new HtmlWeb();
        }

        public Parser(string region,int quantity)
        {
            web = new HtmlWeb();
            //Url = $"https://realt.by/{region}-region/sale/flats/{quantity}k/";

        }

        public  IEnumerable<FlatData>  GetInfo()  
        {
            // var doc = web.Load(Url);

            /* var client = new WebClient();
             var data = client.DownloadData(Url);
             var page=System.Text.Encoding.UTF8.GetString(data);
             HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
             doc.LoadHtml(page);*/


            string url = "https://realt.by/brest-region/sale/flats/search/";
           /* var formData = new NameValueCollection();
            formData["tx_uedbflat_pi2[DATA][state_district_id][e]"] = "85";
            formData["tx_uedbflat_pi2[DATA][town_id][e]"] = "6262";
            formData["tx_uedbflat_pi2[DATA][x_count_pictures][ge]"] = "1";

            var client = new WebClient();
            var responseBytes = client.UploadValues(url, "POST", formData);
            var page = Encoding.UTF8.GetString(responseBytes);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);*/


            var doc=web.Load(url);
            var dictionary = new Dictionary<string, string>();

            //var selectNode = doc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "some-search-left-right").FirstOrDefault().FirstChild;
            //dictionary.Add(x.InnerText,x.Attributes["value"].Value));

            var selectNode = doc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "some-search")
                .FirstOrDefault().Descendants().Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "some-search-left-right").Skip(1)
                .FirstOrDefault().ChildNodes.Skip(1).FirstOrDefault();
            var result=selectNode.ChildNodes.Where(x=>x.Name=="option").Skip(1).Select(x => new { Name = x.NextSibling.InnerText, Value = x.Attributes["value"].Value });

            foreach (var r in result)
                dictionary.Add(r.Name,r.Value);



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


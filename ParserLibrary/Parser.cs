using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.ComponentModel;

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
            Url = $"https://realt.by/{region}-region/sale/flats/{quantity}k/";
        }

        public  IEnumerable<FlatData>  GetInfo()  
        {
            var doc = web.Load(Url);

           /* var client = new WebClient();
            var data = client.DownloadData(Url);
            var page=System.Text.Encoding.UTF8.GetString(data);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);*/

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

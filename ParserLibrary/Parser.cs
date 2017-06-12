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
        ISettings settings;
        WebClient client;

        public Parser(ISettings settings)
        {
            this.settings = settings;
            client = new WebClient();
        }

        public string Load(string town,string quantity,string start,string end)  //,string quantity  Parameters parameters
        {
            var formData = new NameValueCollection();
            formData["tx_uedbflat_pi2[DATA][town_id][e]"] = town;
            formData["tx_uedbflat_pi2[DATA][x_count_pictures][ge]"] = "1"; //with foto
            formData["tx_uedbflat_pi2[DATA][rooms][e][1]"] = quantity;
            formData["tx_uedbflat_pi2[DATA][building_year][ge]"] = start;
            formData["tx_uedbflat_pi2[DATA][building_year][le]"] = end;
            var responseBytes = client.UploadValues(settings.Url, "POST", formData);
            return Encoding.UTF8.GetString(responseBytes);
        }

        public IEnumerable<FlatData> Parse(string text)  
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);

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


using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public class TownCodeLoader
    {
        HtmlWeb web;

        public Dictionary<string, string> Dictionary { get; set; }

        public string Url { get; set; }


        public TownCodeLoader(string region)
        {
            web = new HtmlWeb();
            Url = $"https://realt.by/{region}-region/sale/flats/search/";
            Dictionary = new Dictionary<string,string>();
        }

        public bool LoadTowns()
        {
            try
            {
                var doc = web.Load(Url);

                var selectNode = doc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "some-search")
                    .FirstOrDefault().Descendants().Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "some-search-left-right").Skip(1)
                    .FirstOrDefault().ChildNodes.Skip(1).FirstOrDefault();
                var townAndCodes = selectNode.ChildNodes.Where(x => x.Name == "option").Skip(1).Select(x => new { Name = x.NextSibling.InnerText, Value = x.Attributes["value"].Value });

                foreach (var x in townAndCodes)
                    Dictionary.Add(x.Name, x.Value);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}

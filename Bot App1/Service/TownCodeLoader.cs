using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_App1.Service
{
    public class TownCodeLoader
    {
        HtmlWeb web;
        string url;

        public Dictionary<string, string> CodeDictionary { get; set; }

        public TownCodeLoader(string url)
        {
            this.url = url;
            web = new HtmlWeb();
            CodeDictionary = new Dictionary<string, string>();
            
        }

        public Dictionary<string, string> LoadTowns() //void
        {
            var doc = web.Load(url);

            var selectNode = doc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "some-search")
                .FirstOrDefault().Descendants().Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "some-search-left-right").Skip(1)
                .FirstOrDefault().ChildNodes.Skip(1).FirstOrDefault();
            var townAndCodes = selectNode.ChildNodes.Where(x => x.Name == "option").Skip(1).Select(x => new { Name = x.NextSibling.InnerText, Value = x.Attributes["value"].Value });

            foreach (var x in townAndCodes)
                CodeDictionary.Add(x.Name, x.Value);

            return CodeDictionary; //
        }

    }
}
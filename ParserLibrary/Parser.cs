using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ParserLibrary
{
    public class Parser
    {
        HtmlWeb web;

        public string BaseUrl { get;  }= "https://realt.by/sale/flats/1k/"; //{0}  

        public IEnumerable<string> Titles { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }

        public Parser()
        {
            web = new HtmlWeb();
        }


        public async Task<bool> GetInfo()
        {
            string url = String.Format(BaseUrl);
            var doc = await Task.Factory.StartNew(() => web.Load(url));

            var titleNodes = doc.DocumentNode.SelectNodes("//*[@id=\"c1030\"]/div/div//div[1]/a[1]");           //.Skip(3).Take(100);      
            Titles = titleNodes.Select(node => node.GetAttributeValue("title", null)).Skip(5).Take(10).Distinct();
            return true;
        }

      /*  public async Task<IEnumerable<string>> GetSrc()
        { 
            string url = String.Format(BaseUrl);
            var doc = await Task.Factory.StartNew(() => web.Load(url));

            //*[@id="c1030"]/div/div//div[2]/div[1]/a/img    
            var imagesNodes = doc.DocumentNode.Descendants("//*[@id=\"c1030\"]/div/div[3]/div[2]/div[1]/a/img");   //*[@id="c1030"]/div/div[6]/div[2]/div[1]/a/img
            return imagesNodes.Select(node => node.GetAttributeValue("src", null)).Where(s => !String.IsNullOrEmpty(s)); ;            //*[@id="c1030"]/div/div[3]/div[2]/div[1]/a/img
            //catch
             
        }*/

        public async Task<IEnumerable<string>> GetTitles()
        {
            string url = String.Format(BaseUrl);
            var doc = await Task.Factory.StartNew(() => web.Load(url));

            var nodes = doc.DocumentNode.Descendants().Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value=="bd-item ");

            var titleNodes = nodes.Select(node=>node.ChildNodes.Where(x => x.Name == "div" && x.Attributes["class"] != null && x.Attributes["class"].Value == "title").FirstOrDefault());

             var imageNodes = nodes.Select(node => node.Descendants("img").FirstOrDefault());


             var images = imageNodes.Select(node => node.Attributes["data-original"].Value);

        //    var descriptionNodes = nodes.Select(node => node.Descendants("bd-item-right-center").FirstOrDefault());

        //    var description = descriptionNodes.Select(node => node.ChildNodes.Where(x => x.Name == "p").LastOrDefault().InnerText);
            
                

            var titles = titleNodes.Select(node => node.InnerText);
     
            return titles;
        }
 

    }
}

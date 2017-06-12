using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public class Settings:ISettings
    {
        public Settings(string url)
        {
            Url = url;
        }
        public string Url { get; set; }
    }
}

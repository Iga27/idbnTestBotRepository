using Bot_App1.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace BotApp1.Tests
{

    [TestClass]
    public class TownCodeLoaderTestClass
    {
        TownCodeLoader townLoader;

        [TestInitialize]
        public void Initialize()
        {
            townLoader = new TownCodeLoader("https://realt.by/sale/flats/search/");
        }

        [TestMethod]
        public void LoadTowns_Should_Return_Right_Results()
        {
            var actualDictionary = townLoader.LoadTowns();
            var expectedDictionary = new Dictionary<string, string>(); 
            var lines=File.ReadAllLines("D:/BotApp1/BotApp1.Tests/resources/codes.txt", Encoding.Default);
            foreach(var line in lines)
            {
                var pair = line.Split(',');
                expectedDictionary.Add(pair[0], pair[1]);
            }

            Assert.IsNotNull(actualDictionary);
            Assert.IsTrue(expectedDictionary.Take(10).OrderBy(x=>x.Key).Select(x => x.Value)
                .SequenceEqual(actualDictionary.Take(10).OrderBy(y=>y.Key).Select(y => y.Value)));            
        }
    }
}

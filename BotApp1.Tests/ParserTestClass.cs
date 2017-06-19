using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bot_App1.Service;
using System.IO;
using System.Text;

namespace BotApp1.Tests
{
    [TestClass]
    public class ParserTestClass
    {
        #region initialization
        Parser parser;
        FlatParameters parameters;

        [TestInitialize]
        public void Initialize()
        {
            parameters = new FlatParameters() { Town = "186", Price = "1500", StartYear = "2010", Quantity = "3" };
            parser = new Parser("https://realt.by/sale/flats/search/");
        }
        #endregion


        #region ParseTests
        [TestMethod]
        public void Parse_Should_Return_Right_Result()
        {
            string text = File.ReadAllText("D:/BotApp1/BotApp1.Tests/resources/file.txt",Encoding.Default);
            var actual = parser.Parse(text);
            var expected = new[] {
                new FlatData {
                    ImageSrc ="https://realt.by/thumb/c/180x120/e40a326f19a3da9dc03e6300514f5aec/sm/t/site1q1h8tsm/6a4e1ba4b2.jpg",
                    Link="https://realt.by/brest-region/sale/flats/object/1086648/",
                    Title=" Продается 3 комнатная квартира, Барановичи, Скорины ул., 6   "} };
            Assert.IsTrue(expected.SequenceEqual(actual, new MyClassComparer()));
        }
        #endregion
    }
}

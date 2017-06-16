using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bot_App1.Dialogs;
using Bot_App1.Service;
using System.Collections.Generic;

namespace BotApp1.Tests
{
    [TestClass]
    public class UnitTest1
    {

        #region  GetAllParametersTests
        private void GetAllParametersTest(string text,FlatParameters expected)
        {
            var actual = RootDialog.GetAllParameters(text);
            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public void GetAllParameters_Should_Return_FlatParameters()
        {
            GetAllParametersTest("город Брест, 3-комнатная квартира, год постройки не позднее 1996 г и не дороже 900$ за кв.м.",
                new FlatParameters() { Price = "900", Quantity = "3", StartYear = "1996", Town = "791" });


            GetAllParametersTest("город Молодечно, 4-комнатная, не дороже 4500$ за кв.м. и не позднее 2010 года",
                new FlatParameters() { Price = "4500", Quantity = "4", StartYear = "2010", Town = "5196" });                    
        }
        #endregion

        #region GetBeetweenTests

        private void GetBetweenTest(string source, string startString, string endString,string expected)
        {
            var actual = RootDialog.GetBetween(source, startString, endString);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetBetween_Should_Return_Right_Results()
        {
            GetBetweenTest("простой текст для проверки", "текст ", " проверки", "для");

            GetBetweenTest("random text, 900$ hello",", ","$","900");
        }

        [TestMethod]
        public void GetBetween_Should_Return_EmptyString_If_Not_Found()
        {
            var actual = RootDialog.GetBetween("my random text","word","text");
            Assert.AreEqual(String.Empty, actual);
        }

        #endregion

        #region ParseTests

       


        [TestMethod]

       

        public void Parse_Should_Return_Right_Result()
        {
            var parser = new Parser("https://realt.by/sale/flats/search/");
            var stringForParsing = parser.Load(new FlatParameters() { Town = "186", Price = "1500", StartYear = "2010", Quantity = "3" });
            var actual=parser.Parse(stringForParsing);
            var expected = new[] {
                new FlatData {
                    ImageSrc ="https://realt.by/thumb/c/180x120/e40a326f19a3da9dc03e6300514f5aec/sm/t/site1q1h8tsm/6a4e1ba4b2.jpg",
                Link="https://realt.by/brest-region/sale/flats/object/1086648/",
                Title=" Продается 3 комнатная квартира, Барановичи, Скорины ул., 6   "} };
            Assert.AreEqual(expected, actual);
        }


        #endregion
    }
}

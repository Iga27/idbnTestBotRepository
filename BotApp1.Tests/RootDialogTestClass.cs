using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bot_App1.Dialogs;
using Bot_App1.Service;
 
 


namespace BotApp1.Tests
{
    [TestClass]
    public class RootDialogTestClass
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
       
    }    
}

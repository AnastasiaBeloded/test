using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AF.AutomationTest.MatchingEngine.Tests
{
    [TestClass]
    public class Tests
    {
        private static MatchingApi _matchingApi;

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            _matchingApi = new MatchingApi();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _matchingApi.ClearData();
        }

        // example test
        [TestMethod]
        public void FindMatchTest()
        {
            var date = DateTime.UtcNow;

            var record1 = _matchingApi.CreateRecord("Test", 100, 10, date, Side.Buy);
            var record2 = _matchingApi.CreateRecord("test", 100, 10, date, Side.Sell);

            var isMatched = _matchingApi.CheckIfRecordsMatched(record1, record2);

            Assert.IsTrue(isMatched);
        }
        [TestMethod]
        public void TestTryMatch_Success()
        {
            
            var record1 = _matchingApi.CreateRecord("Google", 100, 1250.00, new DateTime(2023, 3, 5), Side.Buy);
            var record2 = _matchingApi.CreateRecord("Google", 105, 1250.00, new DateTime(2023, 3, 5), Side.Sell);

            
            var matches = _matchingApi.FindMatch("Google");

            
            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(record1, matches[0].trade);
            Assert.AreEqual(record2, matches[0].counterTrade);
        }

        [TestMethod]
        public void TestTryMatch_Failure_DifferentSymbol()
        {
            
            var record1 = _matchingApi.CreateRecord("Google", 100, 1250.00, new DateTime(2023, 3, 5), Side.Buy);
            var record2 = _matchingApi.CreateRecord("Amazon", 100, 1250.00, new DateTime(2023, 3, 5), Side.Sell);

           
            var matches = _matchingApi.FindMatch("Google");

            
            Assert.AreEqual(0, matches.Count);
        }

        [TestMethod]
        public void TestCheckIfRecordsMatched_True()
        {
            
            var record1 = _matchingApi.CreateRecord("Google", 100, 1250.00, new DateTime(2023, 3, 5), Side.Buy);
            var record2 = _matchingApi.CreateRecord("Google", 105, 1250.00, new DateTime(2023, 3, 5), Side.Sell);

            
            var isMatched = _matchingApi.CheckIfRecordsMatched(record1, record2);

            
            Assert.IsTrue(isMatched);
        }

        [TestMethod]
        public void TestCheckIfRecordsMatched_False()
        {
           
            var record1 = _matchingApi.CreateRecord("Google", 100, 1250.00, new DateTime(2023, 3, 5), Side.Buy);
            var record2 = _matchingApi.CreateRecord("Amazon", 100, 1250.00, new DateTime(2023, 3, 5), Side.Sell);

            
            var isMatched = _matchingApi.CheckIfRecordsMatched(record1, record2);

            
            Assert.IsFalse(isMatched);
        }

        [TestMethod]
        public void TestClearData()
        {
            
            _matchingApi.CreateRecord("Google", 100, 1250.00, new DateTime(2023, 3, 5), Side.Buy);
            _matchingApi.CreateRecord("Google", 105, 1250.00, new DateTime(2023, 3, 5), Side.Sell);

            
            _matchingApi.ClearData();
            var allRecords = _matchingApi.GetAllRecords();
            var allMatches = _matchingApi.FindMatch("Google");

            
            Assert.AreEqual(0, allRecords.Count);
            Assert.AreEqual(0, allMatches.Count);
        }
    }
}

    

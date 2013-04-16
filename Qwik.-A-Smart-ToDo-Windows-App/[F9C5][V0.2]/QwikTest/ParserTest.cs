using QWIK.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using QWIK;

namespace QwikTest
{
    
    
    /// <summary>
    ///This is a test class for ParserTest and is intended
    ///to contain all ParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ParserTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /*
        /// <summary>
        ///A test for Parser Constructor
        ///</summary>
        [TestMethod()]
        public void ParserConstructorTest()
        {
            Parser target = new Parser();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
        */
        /// <summary>
        ///A test for DateOnly
        ///</summary>
        [TestMethod()]
        public void DateOnlyTest()
        {
            DateTime temp = new DateTime(2,2,2,12,0,0); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(2,2,2,0,0,0); // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = Parser.DateOnly(temp);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DayToDate
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void DayToDateTest()
        {
            string day1 = "tmr"; // TODO: Initialize to an appropriate value
            
            DateTime expected1 = new DateTime(); // TODO: Initialize to an appropriate value
            expected1 = DateTime.Now;
            expected1 = expected1.AddDays(1);

            DateTime actual1;
            
            actual1 = Parser_Accessor.DayToDate(day1);        
            Assert.AreEqual(expected1.ToString(), actual1.ToString());
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for EditEvent
        ///</summary>
        [TestMethod()]
        public void EditEventTest()
        {
            string commandline = "edit 0 lala project meeting 13/12 9pm"; // TODO: Initialize to an appropriate value
            string commandline1 = "project meeting 12/12 6pm";
            NewUi ui = new NewUi();
            ui.ProcessCommandline(commandline1);

            int index = 0; // TODO: Initialize to an appropriate value
            int indexExpected = 0; // TODO: Initialize to an appropriate value
            NewUi.EventInfo info = new NewUi.EventInfo(); // TODO: Initialize to an appropriate value
            NewUi.EventInfo infoExpected = new NewUi.EventInfo(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser.EditEvent(commandline, ref index, ref info);
            Assert.AreEqual(indexExpected, index);
            Assert.AreEqual(infoExpected, info);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /*
        /// <summary>
        ///A test for EndOfDay
        ///</summary>
        [TestMethod()]
        public void EndOfDayTest()
        {
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = Parser.EndOfDay();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ExtractDate
        ///</summary>
        [TestMethod()]
        public void ExtractDateTest()
        {
            string substring = string.Empty; // TODO: Initialize to an appropriate value
            DateTime date = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime dateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser.ExtractDate(substring, ref date);
            Assert.AreEqual(dateExpected, date);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ExtractTime
        ///</summary>
        [TestMethod()]
        public void ExtractTimeTest()
        {
            string substring = string.Empty; // TODO: Initialize to an appropriate value
            DateTime time = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime timeExpected = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser.ExtractTime(substring, ref time);
            Assert.AreEqual(timeExpected, time);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FindDates
        ///</summary>
        [TestMethod()]
        public void FindDatesTest()
        {
            string[] words = null; // TODO: Initialize to an appropriate value
            int index = 0; // TODO: Initialize to an appropriate value
            DateTime date = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime dateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = Parser.FindDates(words, index, ref date);
            Assert.AreEqual(dateExpected, date);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FindTime
        ///</summary>
        [TestMethod()]
        public void FindTimeTest()
        {
            string[] words = null; // TODO: Initialize to an appropriate value
            int index = 0; // TODO: Initialize to an appropriate value
            DateTime time = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime timeExpected = new DateTime(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = Parser.FindTime(words, index, ref time);
            Assert.AreEqual(timeExpected, time);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FoundDateKey
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void FoundDateKeyTest()
        {
            string[] words = null; // TODO: Initialize to an appropriate value
            int index = 0; // TODO: Initialize to an appropriate value
            DateTime startDate = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime startDateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endDate = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endDateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = Parser_Accessor.FoundDateKey(words, index, ref startDate, ref endDate);
            Assert.AreEqual(startDateExpected, startDate);
            Assert.AreEqual(endDateExpected, endDate);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FoundOtherKey
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void FoundOtherKeyTest()
        {
            string[] words = null; // TODO: Initialize to an appropriate value
            int i = 0; // TODO: Initialize to an appropriate value
            bool urgent = false; // TODO: Initialize to an appropriate value
            bool urgentExpected = false; // TODO: Initialize to an appropriate value
            bool alarm = false; // TODO: Initialize to an appropriate value
            bool alarmExpected = false; // TODO: Initialize to an appropriate value
            DateTime alarmTime = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime alarmTimeExpected = new DateTime(); // TODO: Initialize to an appropriate value
            bool freeDay = false; // TODO: Initialize to an appropriate value
            bool freeDayExpected = false; // TODO: Initialize to an appropriate value
            DateTime startDate = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime startDateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endDate = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endDateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = Parser_Accessor.FoundOtherKey(words, i, ref urgent, ref alarm, ref alarmTime, ref freeDay, ref startDate, ref endDate);
            Assert.AreEqual(urgentExpected, urgent);
            Assert.AreEqual(alarmExpected, alarm);
            Assert.AreEqual(alarmTimeExpected, alarmTime);
            Assert.AreEqual(freeDayExpected, freeDay);
            Assert.AreEqual(startDateExpected, startDate);
            Assert.AreEqual(endDateExpected, endDate);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FoundTimeKey
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void FoundTimeKeyTest()
        {
            string[] words = null; // TODO: Initialize to an appropriate value
            int index = 0; // TODO: Initialize to an appropriate value
            DateTime startTime = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime startTimeExpected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endTime = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endTimeExpected = new DateTime(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = Parser_Accessor.FoundTimeKey(words, index, ref startTime, ref endTime);
            Assert.AreEqual(startTimeExpected, startTime);
            Assert.AreEqual(endTimeExpected, endTime);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPeriod
        ///</summary>
        [TestMethod()]
        public void GetPeriodTest()
        {
            string period = string.Empty; // TODO: Initialize to an appropriate value
            DateTime startDate = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime startDateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endDate = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endDateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser.GetPeriod(period, ref startDate, ref endDate);
            Assert.AreEqual(startDateExpected, startDate);
            Assert.AreEqual(endDateExpected, endDate);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetTestDatePhrase
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void GetTestDatePhraseTest()
        {
            string[] words = null; // TODO: Initialize to an appropriate value
            int index = 0; // TODO: Initialize to an appropriate value
            int temp = 0; // TODO: Initialize to an appropriate value
            int tempExpected = 0; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Parser_Accessor.GetTestDatePhrase(words, index, ref temp);
            Assert.AreEqual(tempExpected, temp);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetTestTimePhrase
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void GetTestTimePhraseTest()
        {
            string[] words = null; // TODO: Initialize to an appropriate value
            int index = 0; // TODO: Initialize to an appropriate value
            int temp = 0; // TODO: Initialize to an appropriate value
            int tempExpected = 0; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Parser_Accessor.GetTestTimePhrase(words, index, ref temp);
            Assert.AreEqual(tempExpected, temp);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetThisOrNextDay
        ///</summary>
        [TestMethod()]
        public void GetThisOrNextDayTest()
        {
            string phrase = string.Empty; // TODO: Initialize to an appropriate value
            DateTime date = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime dateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser.GetThisOrNextDay(phrase, ref date);
            Assert.AreEqual(dateExpected, date);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetWeek
        ///</summary>
        [TestMethod()]
        public void GetWeekTest()
        {
            string period = string.Empty; // TODO: Initialize to an appropriate value
            DateTime startDate = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime startDateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endDate = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime endDateExpected = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser.GetWeek(period, ref startDate, ref endDate);
            Assert.AreEqual(startDateExpected, startDate);
            Assert.AreEqual(endDateExpected, endDate);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for INotEqualJ
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void INotEqualJTest()
        {
            int i = 0; // TODO: Initialize to an appropriate value
            int j = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser_Accessor.INotEqualJ(i, j);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsNonDateKey
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void IsNonDateKeyTest()
        {
            string[] words = null; // TODO: Initialize to an appropriate value
            int i = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser_Accessor.IsNonDateKey(words, i);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsNonTimeKey
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void IsNonTimeKeyTest()
        {
            string[] words = null; // TODO: Initialize to an appropriate value
            int i = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser_Accessor.IsNonTimeKey(words, i);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsTimeRelated
        ///</summary>
        [TestMethod()]
        public void IsTimeRelatedTest()
        {
            string[] words = null; // TODO: Initialize to an appropriate value
            int i = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser.IsTimeRelated(words, i);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SplitWords
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void SplitWordsTest()
        {
            string commandLine = string.Empty; // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = Parser_Accessor.SplitWords(commandLine);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TimeOnly
        ///</summary>
        [TestMethod()]
        public void TimeOnlyTest()
        {
            DateTime temp = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = Parser.TimeOnly(temp);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ValidEventInfo
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void ValidEventInfoTest()
        {
            NewUi.EventInfo info = new NewUi.EventInfo(); // TODO: Initialize to an appropriate value
            NewUi.EventInfo infoExpected = new NewUi.EventInfo(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser_Accessor.ValidEventInfo(ref info);
            Assert.AreEqual(infoExpected, info);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }*/
    }
}

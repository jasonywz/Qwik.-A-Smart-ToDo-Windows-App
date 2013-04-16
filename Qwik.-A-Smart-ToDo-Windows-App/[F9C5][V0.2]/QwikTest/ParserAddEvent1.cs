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
    public class ParserAddEvent1
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


        /// <summary>
        ///A test for AddEvent
        ///</summary>
        [TestMethod()]
        public void AddEventTest()
        {
            string commandline = "talk by Dr. Lim at 4pm thur alarm 3pm"; // TODO: Initialize to an appropriate value
            NewUi.EventInfo info = new NewUi.EventInfo(); // TODO: Initialize to an appropriate value
            NewUi.EventInfo infoExpected = new NewUi.EventInfo(); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Parser.AddEvent(commandline, ref info);

            Assert.AreEqual(info.eventName, "talk by Dr. Lim");
            Assert.AreEqual(info.startDate.DayOfWeek, DayOfWeek.Thursday);
            Assert.AreEqual(info.endDate.DayOfWeek, DayOfWeek.Thursday);
            Assert.AreEqual(info.startTime, new DateTime(1, 1, 1, 16, 0, 0));
            Assert.AreEqual(info.endTime, new DateTime(1, 1, 1, 16, 0, 0));
            Assert.AreEqual(info.urgent, false);
            Assert.AreEqual(info.alarm, true);
            Assert.AreEqual(info.alarmTime, new DateTime(info.endDate.Year,info.endDate.Month,info.endDate.Day, 15, 0, 0));
            Assert.AreEqual(info.freeDay, false);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}


using QWIK;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace QwikTest
{
    
    
    /// <summary>
    ///This is a test class for NewUiTest and is intended
    ///to contain all NewUiTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NewUiTest
    {
        /*Main Author: Yee Yee Htut*/

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
        [DeploymentItem("ShortcutUIv1.exe")]
        public void AddEventTest()
        {
            NewUi_Accessor target = new NewUi_Accessor(); // TODO: Initialize to an appropriate value
            string[] commandline = { "meeting on 12 jan 2012 at 5pm", "3:11pm meeting 12-01-12", "urgent meeting 12/1/12", "alarm meeting 12 jan 15:00" }; // TODO: Initialize to an appropriate value
            string[] expected = { "meeting 12/1/2012 5:00 PM is added.", "meeting 12/1/2012 3:11 PM is added.", "meeting 12/1/2012 is added.", "meeting 12/1/2012 3:00 PM is added." }; // TODO: Initialize to an appropriate value
            string[] actual = new string[commandline.Length];
            for (int i = 0; i < commandline.Length; i++)
                actual[i] = target.ProcessAddEvent(commandline[i]);
            for (int i = 0; i < commandline.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }
        /// <summary>
        ///A test for ClearEvent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void ClearEventTest()
        {
            NewUi_Accessor target = new NewUi_Accessor(); // TODO: Initialize to an appropriate value
            string[] commandline = {"","12/11/2011","all"}; // TODO: Initialize to an appropriate value
            string[] expected = {"All events cleared.\n","Events on 12/11/2011 cleared.\n","All events cleared.\n"}; // TODO: Initialize to an appropriate value
            string[] actual = new string[commandline.Length];
            for(int i=0; i<commandline.Length; i++)
                actual[i] = target.ClearEvent(commandline[i]);
            for (int i = 0; i < commandline.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
           // Assert.Inconclusive("Verify the correctness of this test method.");
        }

       

        /// <summary>
        ///A test for DeleteEvent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void DeleteEventTest()
        {
            NewUi_Accessor target = new NewUi_Accessor(); // TODO: Initialize to an appropriate value
            string[] index ={null,"all","-1","0","0,1,3"} ; // TODO: Initialize to an appropriate value
            string[] expected = { "Enter index of the event.\n", "All the above tasks are deleted.\n", 
                  "Invalid index. Please try again.\n", "Invalid index. Please try again.\n" ,"Index beyond range. Please enter a valid index!\n"}; // TODO: Initialize to an appropriate value
            string[] actual = new string[index.Length];
            for(int i=0;i < index.Length;i++)
                actual[i] = target.ProcessDeleteEvent(index[i]);
            for(int i=0;i<index.Length;i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        ///A test for DoneEvent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void DoneEventTest()
        {
            NewUi_Accessor target = new NewUi_Accessor(); // TODO: Initialize to an appropriate value
            string[] index = { null, "all", "-1", "0", "0,1,3" };  // TODO: Initialize to an appropriate value
            string[] expected = { "Enter index of the event.\n", "All the above tasks are marked as done.\n", 
                  "Invalid index. Please try again.\n", "Invalid index. Please try again.\n" ,"Please enter a valid index!\n"}; // TODO: Initialize to an appropriate value
            string[] actual = new string[index.Length];
            for (int i = 0; i < index.Length; i++)
                actual[i] = target.ProcessDoneEvent(index[i]);
            for (int i = 0; i < index.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

       

        /// <summary>
        ///A test for SearchEvent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void SearchEventTest()
        {
            NewUi_Accessor target = new NewUi_Accessor(); // TODO: Initialize to an appropriate value
            string[] searchWord = {null,"hello","   k a"}; // TODO: Initialize to an appropriate value
            string[] expected = { "Enter search word.\n", "No results found. Please try another search word.\n", "No results found. Please try another search word.\n" };// TODO: Initialize to an appropriate value
            string[] actual = new string[searchWord.Length];
            for(int i=0; i<searchWord.Length;i++)
                actual[i] = target.SearchEvent(searchWord[i]);
            for (int i = 0; i < searchWord.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        ///A test for ViewEvent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void ViewEventTest()
        {
            NewUi_Accessor target = new NewUi_Accessor(); // TODO: Initialize to an appropriate value
            string[] date = {null,"Urgent","Done","4/2/12","1 jan 12 to 3jan 12","from 11 dec 11 to 13 dec 11"}; // TODO: Initialize to an appropriate value
            string[] expected = { "No events.\n", "No events urgent.\n", "No events done.\n", "No events on 4/2/2012.\n", "No events from 1/1/2012 to 3/1/2012.\n", "No events from 11/12/2011 to 13/12/2011.\n" }; // TODO: Initialize to an appropriate value
            string[] actual = new string[date.Length];
            Controller_Accessor.ClearAll();            
            for(int i=0; i< date.Length;i++)
                actual[i] = target.ProcessViewEvent(date[i]);
            for (int i = 0; i < date.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

       

        /// <summary>
        ///A test for GetCommand
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ShortcutUIv1.exe")]
        public void GetCommandTest()
        {
            NewUi_Accessor target = new NewUi_Accessor(); // TODO: Initialize to an appropriate value
            string[] commandline = {"QuiT",null,"delEte  4","meeting","fiNd","vieW",}; // TODO: Initialize to an appropriate value
            string[] commandlineExpected = {null,null," 4","meeting",null,null}; // TODO: Initialize to an appropriate value
            NewUi.Command[] expected = { NewUi.Command.Exit, NewUi.Command.Add, NewUi.Command.Delete, NewUi.Command.Add, NewUi.Command.Search, NewUi.Command.View}; // TODO: Initialize to an appropriate value
            NewUi.Command[] actual = new NewUi.Command[commandline.Length];
            for(int i=0;i< commandline.Length;i++)
                actual[i] = target.GetCommand(ref commandline[i]);

            for (int i = 0; i < commandline.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
                Assert.AreEqual(commandlineExpected[i], commandline[i]);
            }
        }
    }
}

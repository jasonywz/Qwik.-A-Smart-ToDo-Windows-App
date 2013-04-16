//this class is for the automated testing of the software. It runs and checks the test results
//authors: Yu Wei Zhong and Yee Yee Htut
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace QWIK
{
    class TestDrive : NewUi
    {
        private NewUi TestUi;
        StreamReader readText;
        StreamWriter writeResults;
        List<string> commandlines;
        List<string> expectedOutputs;
        string resultFilename;
        public TestDrive()
        {
            TestUi = new NewUi();
            resultFilename = "results.txt";
            Controller.ClearAll();
            if (File.Exists("input.txt"))
            {
                commandlines = new List<string>();
                readText = new StreamReader("input.txt");
                while (!readText.EndOfStream)
                    commandlines.Add(readText.ReadLine());
                readText.Close();

            }
            if (File.Exists("output.txt"))
            {
                expectedOutputs = new List<string>();
                StreamReader readExpected = new StreamReader("output.txt");
                while (!readExpected.EndOfStream)
                    expectedOutputs.Add(readExpected.ReadLine());
                readExpected.Close();
            }
            writeResults = new StreamWriter(resultFilename);
            RunTest();
            CompareOutputs();
        }
        public void RunTest()
        {

            for (int i = 0; i < commandlines.Count; i++)
                writeResults.Write(TestUi.ProcessCommandline(commandlines[i]));

            writeResults.Close();

        }

        private void CompareOutputs()
        {
            readText = new StreamReader(resultFilename);
            List<string> results = new List<string>();
            while (!readText.EndOfStream)
                commandlines.Add(readText.ReadLine());
            readText.Close();

            bool correct = true;
            for (int i = 0; i < results.Count; i++)
            {
                if (i < expectedOutputs.Count)
                {
                    if (!results.ElementAt(i).Equals(expectedOutputs.ElementAt(i)))
                        correct = false;
                }
                else correct = false;
            }
            if (correct)
                MessageBox.Show("Test passed!");
            else MessageBox.Show("Test failed!");

        }
    }
}

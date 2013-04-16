//This class is has code for the actual form of our user interface. It gets the information from the interface
//and passes it to other classes to process
//authors: Yu Wei Zhong, G Swetha, Yee Yee Htut
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Media;
using System.Timers;

namespace QWIK
{
    public partial class NewUi : Form
    {
        float Step;
        System.Windows.Forms.Timer time;
        System.Timers.Timer timer = new System.Timers.Timer();
        System.Timers.Timer alarmCheck = new System.Timers.Timer(10000000);
        Event[] AlarmArr = new Event[30];
        private int AlarmIndex;
        public NewUi()
        {
            InitializeComponent();
            //icon = new NotifyIcon();
            //icon.Visible = true;
            //icon.Icon = new Icon("icon.ico");
            AlarmIndex = 0;
            WindowFadeIn(50, 0.1F);
            DLabel.Text = null;
            ShowDefaultList();
            QwikText.Select();
            timer.Interval = 1000000;
            timer.Enabled = false;
            alarmCheck.Enabled = true;
            alarmCheck.AutoReset = false;
            alarmCheck.Elapsed += new ElapsedEventHandler(CheckAlarmDone);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            ProcessAlarm();
        }
        

            const int WS_CLIPCHILDREN = 0x2000000;
            const int WS_MINIMIZEBOX = 0x20000;
            const int WS_MAXIMIZEBOX = 0x10000;
            const int WS_SYSMENU = 0x80000;
            const int CS_DBLCLKS = 0x8;
       

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style = WS_CLIPCHILDREN | WS_MINIMIZEBOX | WS_SYSMENU;
                cp.ClassStyle = CS_DBLCLKS;
                return cp;
            }
        }

        Point formloc, cursloc = new Point(0, 0);

        private void setpositions()
        {
            formloc = this.Location;
            cursloc = Cursor.Position;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            int exe = formloc.X - cursloc.X + Cursor.Position.X;
            int eye = formloc.Y - cursloc.Y + Cursor.Position.Y;
            this.Location = new Point(exe, eye);
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            timer1.Start();
            setpositions();
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Stop();
            setpositions();
        }
        public void WindowFadeIn(int interval, float steps)
        {
            Step = steps;
            time = new System.Windows.Forms.Timer();
            time.Interval = interval;
            time.Tick += new EventHandler(Timer_Tick);
            time.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity != 1.0)
            {
                this.Opacity += Step;
            }
            else
            {
                time.Stop();
            }
        }
        private void QwikText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                //Application.Exit();
                //Process.GetCurrentProcess().Kill();
                this.MinimizeBox = true;
                this.Visible = false;
                notifyIcon1.Visible = true;
            }

            if (e.KeyCode == Keys.Enter)//testing the entries in listview
            {
                string commandline;
                string message;
                commandline = QwikText.Text;
                if (!string.IsNullOrWhiteSpace(commandline))
                {
                    message = ProcessCommandline(commandline); 
                    if (message != null)
                        DLabel.Text = message;
                    QwikText.Clear();
                }
            }
            if (e.Control && e.KeyCode == Keys.Z)
                DLabel.Text = Undo();
        }
        private void QwikText_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(QwikText.Text))
            {
                string commandline = QwikText.Text;
                if (GetCommand(ref commandline) == Command.Search)
                    SearchEvent(commandline);
            }
        }

        private const string ADD = "add";
        private const string DELETE = "delete";
        private const string EDIT = "edit";
        private const string SEARCH = "search";
        private const string FIND = "find";
        private const string VIEW = "view";
        private const string UNDO = "undo";
        private const string DONE = "done";
        private const string CLEAR = "clear";
        private const string EXIT = "exit";
        private const string QUIT = "quit";
        public enum Command { Add, Delete, Edit, Search, View, Undo, Done, Exit, Clear };
        private StreamWriter writeLog;
        public struct EventInfo
        {
            public DateTime startDate, endDate, startTime, endTime, alarmTime;
            public string eventName;
            public bool urgent, alarm, freeDay;
            public EventInfo(int x)
            {
                startDate = new DateTime();
                endDate = new DateTime();
                startTime = new DateTime();
                endTime = new DateTime();
                alarmTime = new DateTime();
                eventName = null;
                urgent = alarm = freeDay = false;
            }
        };
        public static void Log(string msg)
        {
            Debug.Assert(msg!=null, "Log(msg) parameter is null");

            StreamWriter writeLog = new StreamWriter("Qwik_log.txt",true);
            writeLog.WriteLine(msg);
            writeLog.Close();
        }
        public string ProcessCommandline(string commandline)
        {
            Debug.Assert(commandline != null, "processcommandline(commandline) is null)");
            string message = null;
            if (!string.IsNullOrWhiteSpace(commandline))
            {
                Command command = GetCommand(ref commandline);
                message = ExecuteCommand(command, commandline);
            }
            return message;
        }
        private Command GetCommand(ref string commandline)
        {
            Command command;
            string[] commandArray = { ADD, DELETE, EDIT, SEARCH, FIND, UNDO, VIEW, DONE, CLEAR, EXIT, QUIT };
            if (commandline != null)
            {

                string firstWord;
                int index1 = 0;
                while (char.IsWhiteSpace(commandline[index1]))
                    index1++;
                int index2 = index1;

                Debug.Assert(index2 >= 0, "index2 in GetCommand(string commandline is <0");

                while (index2 < commandline.Length)
                    if (commandline[index2] != ' ')
                        index2++;
                    else break;
                firstWord = commandline.Substring(index1, index2 - index1);

                Debug.Assert(index2 - index1 > -1);


                if (commandArray.Contains(firstWord.ToLower()))
                {
                    command = StringToCommand(firstWord.ToLower());
                    try
                    {
                        commandline = commandline.Substring(index2 + 1);
                    }
                    catch
                    {
                        commandline = null;
                    }
                }
                else
                {
                    command = Command.Add;
                    commandline = commandline.Substring(index1);
                }
                if (string.IsNullOrWhiteSpace(commandline))
                    commandline = null;
            }
            else command = Command.Add;
            return command;
        }
        private Command StringToCommand(string word)
        {
            Debug.Assert(word != null);
            Command command;
            switch (word)
            {
                case ADD: command = Command.Add;
                    break;
                case DELETE: command = Command.Delete;
                    break;
                case EDIT: command = Command.Edit;
                    break;
                case DONE: command = Command.Done;
                    break;
                case SEARCH:
                case FIND: command = Command.Search;
                    break;
                case UNDO: command = Command.Undo;
                    break;
                case VIEW: command = Command.View;
                    break;
                case CLEAR: command = Command.Clear;
                    break;
                case EXIT:
                case QUIT: command = Command.Exit;
                    break;
                default: command = Command.Add;
                    break;
            }
            return command;
        }
        private string ExecuteCommand(Command command, string commandline)
        {
            string message = null;
            switch (command)
            {
                case Command.Add: message = ProcessAddEvent(commandline);
                    break;
                case Command.Delete: message = ProcessDeleteEvent(commandline);
                    break;
                case Command.Search: message = SearchEvent(commandline);
                    break;
                case Command.View: message = ProcessViewEvent(commandline);
                    break;
                case Command.Undo: message = Undo();
                    break;
                case Command.Done: message = ProcessDoneEvent(commandline);
                    break;
                case Command.Clear: message = ClearEvent(commandline);
                    break;
                case Command.Edit: message = ProcessEditEvent(commandline);
                    break;
                case Command.Exit: 
                    Application.Exit();
                    break;
            }
            Debug.Assert(message != null);
            return message;
        }      
        private string ClearEvent(string commandline)
        {
             string message = null;
            DateTime date = new DateTime();
            try
            {
                if (string.IsNullOrWhiteSpace(commandline) || commandline.ToLower() == "all")
                {
                    if (MessageBox.Show("Are you sure?", "Clear All Events", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        Controller.ClearAll();
                        ClearAlarmList(AlarmArr);
                        message = "All events cleared.\n";
                    }
                }
                else if (Classes.Parser.ExtractDate(commandline, ref date))
                {
                    Controller.Clear(date);
                    message = "Events on " + date.ToShortDateString() + " cleared.\n";
                    if (date == DateTime.Today)
                        ClearAlarmList(AlarmArr);

                }
                else
                    message = "Invalid date.\n";
                ShowDefaultList();
                return message;
            }
            catch (Exception ex)
            {
                Log("Failed in ClearEvent in NewUI " + ex.ToString());
                message = "Invalid date.\n";
            }
            return message;
        }
        private string ProcessAddEvent(string commandline)
        {
            string message;
            EventInfo info = new EventInfo();
            if (commandline != null)
                try
                {
                    Log("Parsing AddEvent info");
                    if (Classes.Parser.AddEvent(commandline, ref info))
                        message = AddEvent(ref info);
                    else message = "Enter valid date and time.\n";
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    Log("Index of AlarmArr out of range: " + ex.ToString());
                    message = "Unable to add event.\n";
                }
                catch (Exception ex)
                {
                    Log("Failed to add: " + ex.ToString());
                    message = "Unable to add event.\n";
                }
            else message = "Enter event information.\n";
            return message;
        }
        private string AddEvent(ref EventInfo info)
        {
            string message;
            Debug.Assert(info.eventName!= null);
            if (info.freeDay)
            {
                Log("Calling UI to Add Free Event");
                info.startDate = info.endDate = Controller.AddFreeEvent(info.eventName, info.startDate, info.endDate, info.startTime, info.endTime, info.alarm, info.urgent, info.alarmTime);
            }
            else
            {
                Log("Calling UI to Add Event");
                Controller.Add(info.eventName, info.startDate, info.endDate, info.startTime, info.endTime, info.alarm, info.urgent, info.alarmTime);
            }
            if (info.endTime == new DateTime(1, 1, 1, 23, 59, 59))
                message = info.eventName + ' ' + info.endDate.ToShortDateString() + " is added.";
            else message = info.eventName + ' ' + info.endDate.ToShortDateString() + ' ' + info.startTime.ToShortTimeString() + " is added.";
            
            ShowDefaultList();
            if (info.alarm == true && info.alarmTime.Date == DateTime.Today)
            {
                ProcessAlarm();
            }
            return message;
        }
        private string ProcessEditEvent(string commandline)
        {
            string message = null;
            EventInfo info = new EventInfo();
            int index = -1;
            if (commandline != null)
            {
                try
                {
                    Log("Parsing EditEvent info");
                    if (Classes.Parser.EditEvent(commandline, ref index, ref info))
                    {
                        if (index < 0)
                            message = "Enter a valid index\n";
                        else
                            message = EditEvent(index, info);
                    }
                    else message = "Enter index of event.\n";

                }
                catch (Exception ex)
                {
                    Log("Failed to edit in NewUI class: " + ex.ToString());
                    message = "Invalid index.\n";
                }
            }
            else message = "Enter index of event and information you want to change.\n";
            return message;
        }
        private string EditEvent(int index, EventInfo info)
        {
            string message= null, editedEvent=null;
            Debug.Assert(index >= 0);
            try
            {
                Log("Calling Controller to edit event");
                editedEvent = Controller.Edit(index, info.eventName, info.startDate, info.endDate, info.startTime, info.endTime, info.alarm, info.urgent, info.alarmTime);
                if (editedEvent != null)
                {
                    message = listView1.Items[index - 1].SubItems[1].Text + ' ' + listView1.Items[index - 1].SubItems[2].Text + " is edited ";
                    message += "to " + editedEvent + ".\n";
                    if (info.alarm == true && info.endDate == DateTime.Today)
                        ProcessAlarm();
                }
                else message = "Invalid index.\n";
                ShowDefaultList();
                return message;
            }
            catch (Exception ex)
            {
                Log("Failed to edit in NewUI: " + ex.ToString());
                message = "Invalid command\n";
            }
            return message;
        }
        private string ProcessDeleteEvent(string index)
        {
            string message = null;
            if (index != null)
            {
                int indexNo;
                bool isInt;
                bool deleted = false;
                if (index.ToLower() == "all")
                    deleted = DeleteAll(ref message);
                else
                {
                    isInt = Int32.TryParse(index, out indexNo);
                    if (isInt)
                        deleted = DeleteOneEvent(indexNo, ref message);
                    else
                    {
                        try
                        {
                            deleted = TryDeleteMultiple(index, ref message);
                        }
                        catch (Exception ex)
                        {
                            Log("Failed to delete multiple event: " + ex.ToString());
                            message = "Invalid index. Please try again.\n";
                        }
                    }
                }
                if (deleted)
                    ShowDefaultList();
            }
            else message = "Enter index of the event.\n";
            return message;
        }
        private bool DeleteAll(ref string message)
        {
            bool deleted = false;
            Log("Calling UI to delete all");
            try
            {
                if (Controller.DeleteAll())
                {
                    message = "All the above tasks are deleted.\n";
                    deleted = true;
                    ClearAlarmList(AlarmArr);
                }
            }
            catch (Exception ex)
            {
                Log("Failed to delete all: " + ex.ToString());
                message = "Unable to delete event.\n";
            }
            return deleted;
        }
        private bool DeleteOneEvent(int indexNo, ref string message)
        {
            bool deleted = false;
            Log("Calling UI to delete event");
            try
            {
                if (Controller.Delete(indexNo))
                {
                    message = listView1.Items[indexNo - 1].SubItems[1].Text + ' ' + listView1.Items[indexNo - 1].SubItems[2].Text + " is deleted.\r\n";
                    deleted = true;
                    ProcessAlarm();
                }
                else message = "Invalid index. Please try again.\n";
            }
            catch (System.IndexOutOfRangeException exception)
            {
                Log("Failed to delete event: " + exception.ToString());
                message = "Invalid index. Please try again.\n";
            }
            return deleted;
        }
        private bool TryDeleteMultiple(string index, ref string message)
        {
            Log("Preparing to delete multiple events");
            string[] arr = index.Split('\r', ' ', ',');
            bool isInt;
            List<int> deleted = new List<int>();
            int indexNo;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                isInt = Int32.TryParse(arr[i], out indexNo);
                
                if (isInt)
                {
                    Log("Calling UI to delete event");
                    if (Controller.Delete(indexNo))
                    {
                        deleted.Add(indexNo);
                    }
                }
                else if (arr[i] == "")
                    continue;                
            }
            if (deleted.Count != 0)
            {
                message = listView1.Items[deleted.ElementAt(0)-1].SubItems[1].Text;
                if (deleted.Count > 1)
                {
                    for (int i = 1; i < deleted.Count - 1; i++)
                        message += ", " + listView1.Items[deleted.ElementAt(i) - 1].SubItems[1].Text;
                    message += " and " + listView1.Items[deleted.ElementAt(deleted.Count-1)-1].SubItems[1].Text + " are deleted.\n";
                }
                else message += " is deleted.\n";
                ProcessAlarm();

                return true;
            }
            else
            {
                message = "Index beyond range. Please enter a valid index!\n";
                return false;
            }
        }
        private string ProcessViewEvent(string date)
        {
            string message = null;
            bool valid = true;
            List<string[]> viewEventsForDay = new List<string[]>();
           
            if (!ViewOthers(ref message, date, ref viewEventsForDay))
                if (!ViewDates(ref message, date, ref viewEventsForDay))
                    valid = false;
            
            if (valid)
            {
                if (viewEventsForDay.Count == 0)
                    message = "No events" + message + ".\n";
                else
                {
                    UpdateList(viewEventsForDay);
                    if (viewEventsForDay.Count == 1)
                        message = viewEventsForDay.Count + " event" + message + ".\n";
                    else
                        message = viewEventsForDay.Count + " events" + message + ".\n";
                }
            }
            return message;
        }
        private static bool ViewOthers(ref string message, string word, ref List<string[]> viewedEvents)
        {
            if (string.IsNullOrWhiteSpace(word) || word.ToLower() == "all")
                Controller.ViewAll(ref viewedEvents);
            else if (word.ToLower() == "urgent")
            {
                Controller.ViewUrgent(ref viewedEvents);
                message = " urgent";
            }
            else if (word.ToLower() == "done")
            {
                Controller.ViewDone(ref viewedEvents);
                message = " done";
            }
            else return false;
            return true;
        }
        private static bool ViewDates(ref string message, string date, ref List<string[]> viewEventsForDay)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            if (Classes.Parser.GetWeek(date, ref startDate, ref endDate))
            {
                Controller.View(startDate, endDate, ref viewEventsForDay);
                message = " from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString();
            }
            else if (Classes.Parser.ExtractDate(date, ref startDate))
            {
                Controller.View(startDate, startDate, ref viewEventsForDay);
                message = " on " + startDate.ToShortDateString();
            }
            else if (Classes.Parser.GetPeriod(date, ref startDate, ref endDate))
            {
                Controller.View(startDate, endDate, ref viewEventsForDay);
                message = " from " + startDate.ToShortDateString() + " to " + endDate.ToShortDateString();
            }
            
            else
            {
                message = "Invalid view! Please enter valid date/period/week command\n";
                return false;
            }
            return true;
        }
        private string SearchEvent(string searchWord) 
        {
            string message = null;
            try
            {
                if (searchWord != null)
                {
                    List<string[]> SearchResult = new List<string[]>();
                    if (Controller.Search(searchWord, ref SearchResult))
                    {
                        UpdateList(SearchResult);
                        if (SearchResult.Count == 1)
                            message = SearchResult.Count + " result found.";
                        else
                            message = SearchResult.Count + " results found.";
                    }
                    else
                        message = "No results found. Please try another search word.\n";
                }
                else
                {
                    message = "Enter search word.\n";
                    ShowDefaultList();
                }
                return message;
            }
            catch (Exception ex)
            {
                Log("Failed in SearchEvent in NewUI: " + ex.ToString());
                message = "Invalid search. Please enter a search word";
            }
            return message;
        }
        private string ProcessDoneEvent(string index)
        {
            string message = null;
            if (index != null)
            {
                int indexNo;
                bool isInt;
                bool marked = false;
                if (index.ToLower() == "all")
                {
                    marked = DoneAll(ref message); //refers to displayed tasks only
                }
                else
                {
                    isInt = Int32.TryParse(index, out indexNo);
                    if (isInt)
                    {
                        if (indexNo > 0)
                            marked = DoneOneEvent(indexNo, ref message);
                        else
                            message = "Invalid index.\n";
                    }
                    else
                    {
                        try
                        { marked = TryDoneMultiple(index, ref message); }

                        catch (Exception ex)
                        {
                            Log("Failed to mark multiple events as done: " + ex.ToString());
                            message = "Invalid index. Please try again.\n";
                        }
                    }
                }
                if (marked)
                    ShowDefaultList();
            }
            else message = "Enter index of the event.\n";
            return message;
        }
        private bool DoneAll(ref string message)
        {
            bool marked = false;
            Log("Calling UI to mark all as done");
            try
            {
                if (Controller.DoneAll())
                {
                    Log("Calling UI to DoneAll");
                    message = "All the above tasks are marked as done.\n";
                    marked = true;
                    ProcessAlarm();
                }
            }
            catch (Exception ex)
            {
                Log("Failed to mark all as done: " + ex.ToString());
                message = "Unable to mark event as done.\n";
            }
            return marked;
        }
        private bool DoneOneEvent(int indexNo, ref string message)
        {
            bool marked = false;
            Log("Calling UI to mark done");
            try
            {
                if (Controller.Done(indexNo))
                {
                    message = listView1.Items[indexNo - 1].SubItems[1].Text + ' ' + listView1.Items[indexNo - 1].SubItems[2].Text + " is marked as done.\r\n";
                    marked = true;
                    //ProcessAlarm();
                }
                else message = "Invalid index. Please try again.\n";
            }
            catch (System.IndexOutOfRangeException exception)
            {
                Log("Failed to mark event as done: " + exception.ToString());
                message = "Invalid index. Please try again.\n";
            }
            return marked;
        }
        private bool TryDoneMultiple(string index, ref string message)
        {
            Log("Preparing to mark multiple events as done");
            string[] arr = index.Split('\r', ' ', ',');
            bool isInt;
            List<int> marked = new List<int>();
            int indexNo;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                isInt = Int32.TryParse(arr[i], out indexNo);

                if (isInt)
                {
                    Log("Calling UI to delete event");
                    if (Controller.Done(indexNo))
                    {
                        marked.Add(indexNo);
                    }
                }
                else if (arr[i] == "")
                    continue;
            }
            if (marked.Count != 0)
            {
                message = listView1.Items[marked.ElementAt(0) - 1].SubItems[1].Text;
                if (marked.Count > 1)
                {
                    for (int i = 1; i < marked.Count - 1; i++)
                        message += ", " + listView1.Items[marked.ElementAt(i) - 1].SubItems[1].Text;
                    message += " and " + listView1.Items[marked.ElementAt(marked.Count - 1) - 1].SubItems[1].Text + " are marked as done.\n";
                }
                else message += " is marked as done.\n";
                //ProcessAlarm();
                return true;
            }
            else
            {
                message = "Please enter a valid index!\n";
                return false;
            }
        }
        private string Undo()
        {
            Log("Calling UI to Undo");
            if (Controller.Undo())
            {
               ShowDefaultList();
               ProcessAlarm();
               return "Undo.";
            }
            return "Undo not available."; 
        }
        public void ShowDefaultList()
        {
            List<string[]> results = new List<string[]>();
            Controller.View(Classes.Parser.DateOnly(DateTime.Now), Classes.Parser.DateOnly(DateTime.Now.AddDays(6)), ref results);
            UpdateList(results);
        }
        private void UpdateList(List<string[]> listToPrint)
        {
            Debug.Assert(listToPrint != null);
            listView1.Items.Clear();
            
            for (int i = 0; i < listToPrint.Count; i++)
            {
                if (listToPrint[i][4] == "Urgent")
                {
                    ListViewItem item = new ListViewItem(listToPrint[i],null,System.Drawing.Color.Red, Color.WhiteSmoke,null);
                    listView1.Items.Add(item);
                }
                else
                {
                    ListViewItem item = new ListViewItem(listToPrint[i]);
                    listView1.Items.Add(item);
                }
            }
        }
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
        }
        private void ProcessAlarm() //WHAT ABT CASE WHR THR'S >1 ALARM FOR SAME TIME?
        {
            try
            {
                //ClearAlarmList(AlarmArr);
                int CountAlarms = Controller.GetAlarmList(AlarmArr);
                if (CountAlarms != 0)
                {
                    SortAlarmList(ref AlarmArr);
                    int compareTime, i =0;
                    TimeSpan Dur, NextTimeInterval;
                    DateTime currDateTime = DateTime.Now;
                    DateTime alarmTime = DateTime.Today;
                    currDateTime = DateTime.Now;
                    for (i = 0; i < CountAlarms; i++)
                    {
                        if (AlarmArr[i]!=null && AlarmArr[i].AlarmTime != DateTime.MinValue)
                        {
                            if ((i==0 && AlarmArr[i].AlarmTime > DateTime.Now) || (i!=0 && AlarmArr[i].AlarmTime < AlarmArr[i-1].AlarmTime))
                            {
                                alarmTime = alarmTime.AddHours(AlarmArr[i].AlarmTime.Hour);
                                alarmTime = alarmTime.AddMinutes(AlarmArr[i].AlarmTime.Minute);
                                alarmTime = alarmTime.AddSeconds(AlarmArr[i].AlarmTime.Second);
                                compareTime = currDateTime.CompareTo(alarmTime);

                                if (compareTime == 0 || compareTime < 0)
                                {
                                    Dur = alarmTime.Subtract(currDateTime);
                                    timer.Enabled = true;
                                    timer.Interval = Dur.TotalMilliseconds;
                                    timer.AutoReset = true;
                                    timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

                                    AlarmIndex = i;
                                }
                                else
                                    continue;
                            }
                            if (AlarmArr[i + 1] != null && AlarmArr[i + 1].AlarmTime != DateTime.MinValue)
                            {
                                compareTime = DateTime.Now.CompareTo(AlarmArr[i + 1].AlarmTime);
                                if (compareTime <= 0)
                                {
                                    NextTimeInterval = AlarmArr[i + 1].AlarmTime.Subtract(DateTime.Now);
                                    alarmCheck.Interval = NextTimeInterval.TotalMilliseconds - 1000;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Failed to get process alarm: " + ex.ToString());
            }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                MessageBox.Show(AlarmArr[AlarmIndex].EventName, "Reminder!!!");
                SystemSounds.Beep.Play();
                timer.Interval = 1000000000;
            }
            catch (Exception ex)
            { Log("Failed at OnTimedEvent: " + ex.ToString()); }
        }
        private void CheckAlarmDone(object source, ElapsedEventArgs e)
        {
            OnTimedEvent(timer, e);
            alarmCheck.Interval = 10000000;    
            ProcessAlarm();
        }
        private void SortAlarmList(ref Event[] arr)
        {
            int i, j = 0;
            Event next = new Event();
            for (i = 0; i < AlarmArr.Length; ++i)
            {
                if (arr[i] != null)
                {
                    next = arr[i];
                    for (j = i - 1; j >= 0 && arr[j].AlarmTime > next.AlarmTime; --j)
                    {
                        arr[j + 1] = arr[j];
                    }
                    arr[j + 1] = next;
                }
            }
        }
        private void ClearAlarmList(Event[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if(arr[i]!= null)
                    arr[i].AlarmTime = DateTime.MinValue;
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}


//need to call alarm in edit











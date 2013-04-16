//this class is the application layer which does the manipulation of the tasks and processes the information from NewUI
//to perform the appropriate actions
//author: Foo Fang Hau
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing.Text;
using System.Diagnostics;
namespace QWIK
{
    class Logic
    {
        private static Logic LogicInstance;
        //private string Username;
        private Storage QStorage;
        private List<Storage> UndoList;
        private const int MAX_UNDO = 10;
        private DateTime empty = new DateTime();
        public List<Event> ForTempUse = new List<Event>();//list of events for temporary use
        public List<string[]> ToUI = new List<string[]>();//this string is to be passed to UI as UI has no access to Event contents
        public List<DateTime> ToUIFreeday = new List<DateTime>();//list of FREE DATES to be passed to UI

        //contructor
        public Logic()
        {
            UndoList = new List<Storage>();
            if (!OpenQWIKFile())
                QStorage = new Storage();
            LogicInstance = this;
        }
        public static Logic GetInstance()
        {
            if (LogicInstance == null)
                LogicInstance = new Logic();
            return LogicInstance;
        }
        public void AddEvent(string eventName, DateTime startDate, DateTime endDate, DateTime startTime, DateTime endTime, bool alarmStatus, bool urgent, DateTime alarmTime)
        {
            AddToUndoList();
            Event newEvent = new Event(eventName, startDate, endDate, startTime, endTime, alarmStatus, urgent, alarmTime, QStorage.geteventcode());
            QStorage.AddEvent(newEvent);
            QStorage.incrementeventcode();

            WriteQWIKFile();
        }
        public string EditEvent(int index, string newEventName, DateTime newStartDate, DateTime newEndDate, DateTime newStartTime, DateTime newEndTime, bool newalarmStatus, bool newUrgent, DateTime newAlarmTime)
        {
            Debug.Assert(index >= 0 && index < ForTempUse.Count);
            if (index >= 0 && index < ForTempUse.Count)
            {
                AddToUndoList();
                int flag = 0;
                if (BinSearchEventcode(ForTempUse.ElementAt(index).EventCode, ref flag))
                {
                    Event e = QStorage.RetrieveEvent(flag);
                    if (newEventName != null)
                        e.EventName = newEventName;
                    if (newStartDate != empty)
                        e.StartDate = newStartDate;
                    if (newEndDate != empty)
                        e.EndDate = newEndDate;
                    if (newStartTime != empty)
                        e.StartTime = newStartTime;
                    if (newEndTime != empty)
                        e.EndTime = newEndTime;
                    e.AlarmStatus = newalarmStatus;
                    if (newAlarmTime != empty)
                        e.AlarmTime = newAlarmTime;
                    else e.AlarmTime = e.StartTime;
                    e.Importance = newUrgent;
                    if (e.AlarmStatus)
                    {
                        e.AlarmTime = e.AlarmTime.AddDays(e.EndDate.Day - 1);
                        e.AlarmTime = e.AlarmTime.AddMonths(e.EndDate.Month - 1);
                        e.AlarmTime = e.AlarmTime.AddYears(e.EndDate.Year - 1);
                    }
                    WriteQWIKFile();
                    return e.EventName + ' ' + e.GetDeadline().ToShortDateString() + ' ' + e.GetDeadline().ToShortTimeString();
                }
                else return null;
                    
            }
            else throw new System.IndexOutOfRangeException();
        }
        public bool DeleteEvent(int index)
        {
            Debug.Assert(index >= 0 && index < ForTempUse.Count);
            if (index >= 0 && index < ForTempUse.Count)
            {
                AddToUndoList();
                int flag = 0;//temporary flag to store which item to be deleted
                if (BinSearchEventcode(ForTempUse.ElementAt(index).EventCode, ref flag))
                {
                    QStorage.DeleteEvent(flag);

                    WriteQWIKFile();
                    return true;
                }
                else
                    return false;//items cannot be found
            }
            
             else throw new System.IndexOutOfRangeException();
        }
        public bool SearchName(string searchkeyword)
        {
            if (searchkeyword != null)
            {
                ForTempUse.Clear();
                for (int i = 0; i < QStorage.RetrieveSize(); i++)
                {
                    if (!QStorage.RetrieveEvent(i).EventStatus) //undone
                        if (StringContain(searchkeyword, QStorage.RetrieveEvent(i).EventName))
                            ForTempUse.Add(QStorage.RetrieveEvent(i));


                }
                if (ForTempUse.Count() != 0)
                { return true; }
                else
                { return false; }
            }
            else return false;
        }
        public bool ViewEvent(DateTime startDate, DateTime endDate)
        {
            //Debug.Assert(endDate.CompareTo(startDate) <= 0);
            ForTempUse.Clear();
            Event temp;
            for (int i = 0; i < QStorage.RetrieveSize(); i++)
            {
                temp = QStorage.RetrieveEvent(i);
                if(!temp.EventStatus)
                    if ((((temp.StartDate.CompareTo(startDate)) >= 0) && ((temp.StartDate.CompareTo(endDate)) <= 0) || ((temp.EndDate.CompareTo(startDate) >= 0) && (temp.EndDate.CompareTo(endDate) <= 0))) || ((temp.StartDate.CompareTo(startDate) < 0) && (temp.EndDate.CompareTo(endDate) > 0)))
                    ForTempUse.Add(temp);
                
            }
            if (ForTempUse.Count() != 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool ViewAllEvent()
        {
            ForTempUse.Clear();
            for (int i = 0; i < QStorage.RetrieveSize(); i++)
                if (!QStorage.RetrieveEvent(i).EventStatus)
                    ForTempUse.Add(QStorage.RetrieveEvent(i));
            if (ForTempUse.Count() != 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool ViewDoneEvent()
        {
            ForTempUse.Clear();
            for (int i = 0; i < QStorage.RetrieveSize(); i++)
                if (QStorage.RetrieveEvent(i).EventStatus)
                    ForTempUse.Add(QStorage.RetrieveEvent(i));
            if (ForTempUse.Count() != 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool ViewUrgentEvent()
        {
            ForTempUse.Clear();
            for (int i = 0; i < QStorage.RetrieveSize(); i++)
            {
                if (!QStorage.RetrieveEvent(i).EventStatus)
                    if (QStorage.RetrieveEvent(i).Importance)
                        ForTempUse.Add(QStorage.RetrieveEvent(i));
            }
            if (ForTempUse.Count() != 0)
            {
                return true;
            }
            else
                return false;
        }
        public bool BinSearchEventcode(int eventCode, ref int returnflag)
        {
            //fortempuse.Clear();
            int size = QStorage.RetrieveSize();
            int first = 0, mid, last = size;
            while (first <= last)
            {
                mid = (first + last) / 2;  // compute mid point.
                if (eventCode > QStorage.RetrieveEvent(mid).EventCode)
                {
                    first = mid + 1;  // repeat search in top half.
                }
                else if (eventCode < QStorage.RetrieveEvent(mid).EventCode)
                {
                    last = mid - 1; // repeat search in bottom half.
                }
                else
                {
                    returnflag = mid;
                    return true;     // found it. return position /////
                }

            }
            return false;
        }
        public bool Clear(DateTime date)
        {

            if (ViewEvent(date, date))
            {
                AddToUndoList();
                for (int i = 0; i < ForTempUse.Count(); i++)
                {
                    DeleteEvent(i);
                }
                WriteQWIKFile();
                return true;
            }
            else
            {
                return false;
            }

        }
        public void ClearAll()
        {
            AddToUndoList();
            QStorage.ClearAllEvents();
            WriteQWIKFile();
        }
        public bool setEventStatus(int index, bool status)
        {
            Debug.Assert(index >= 0 && index < ForTempUse.Count);
            if (index >= 0 && index < ForTempUse.Count)
            {
                AddToUndoList();
                int flag = 0;
                if (BinSearchEventcode(ForTempUse.ElementAt(index).EventCode, ref flag))
                {
                    QStorage.RetrieveEvent(flag).EventStatus = status;

                    WriteQWIKFile();
                    return true;
                }
                else return false;
            }
            else throw new System.IndexOutOfRangeException();
        }
        public DateTime SearchFreeDay(DateTime startDate, DateTime endDate)
        {
            Debug.Assert(startDate != empty && endDate != empty);
            TimeSpan ts = endDate - startDate;
            for (int i = 0; i <= ts.Days; i++)
            {
                if (!ViewEvent(startDate, startDate))
                {
                    //toUIfreeday.Add(startDate);
                    break;

                }
                startDate = startDate.AddDays(1);
            }
            return startDate;
        }
        public bool StringContain(string searchword, string tobecompared)
        {
            string s1, s2;
            s1 = searchword.ToLower();
            s2 = tobecompared.ToLower();

            if (s2.Contains(s1))
            { return true; }

            return false;

        }
        public string[] ConvertTypetoString(Event eventObject)
        {
            Debug.Assert(eventObject != null);
            string eventname, enddate, starttime = null, endtime, remark, urgent;
            eventname = eventObject.EventName;
            enddate = eventObject.EndDate.ToShortDateString();
            endtime = eventObject.EndTime.ToShortTimeString();
            if (endtime != "11:59 PM")
                starttime = eventObject.StartTime.ToShortTimeString();

            if (eventObject.Importance)
                urgent = "Urgent";
            else urgent = null;
            if (eventObject.AlarmStatus)
                remark = "Alarm " + eventObject.AlarmTime.ToShortTimeString();
            else remark = null;
            if (eventObject.GetDeadline().CompareTo(DateTime.Now) < 0 && !eventObject.EventStatus)
                remark = "Missed Deadline";
            if (eventObject.EventStatus)
                remark = "Done";
            string[] finalstring = { eventname, enddate + ' ' + starttime, remark, urgent };
            return finalstring;
        }
        public bool PassToUI()
        {
            ToUI.Clear();
            if (ForTempUse.Count() != 0)
            {
                
                ForTempUse.Sort((x, y) => x.GetDeadline().CompareTo(y.GetDeadline()));
                for (int i = 0; i < ForTempUse.Count(); i++)
                {
                    string[] final = new string[5];
                    final[0] = string.Format("{0}.", i + 1);
                    ConvertTypetoString(ForTempUse.ElementAt(i)).CopyTo(final, 1);
                    ToUI.Add(final);
                    //toUI.Add(i+1+". "+ ConvertTypetoString(fortempuse.ElementAt(i)));
                }
                
                return true;
            }
            else
                return false;
        }
        private void AddToUndoList()
        {
            if (UndoList.Count >= MAX_UNDO)
            {
                UndoList.RemoveAt(9);
            }
            Storage temp = new Storage(QStorage);
            UndoList.Insert(0, temp);

        }
        public bool Undo()
        {
            Debug.Assert(UndoList.Count > -1 , "Undolist does not exist");
            if (UndoList.Count != 0)
            {
                QStorage = UndoList.ElementAt(0);
                UndoList.RemoveAt(0);
                return true;
            }
            else return false;
        }
        public void WriteQWIKFile()
        {
            
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Storage));
                TextWriter write = new StreamWriter("Qwik.xml");
                serializer.Serialize(write, QStorage);
                write.Close();
            }
            catch (System.Exception exception)
            {
                NewUi.Log(exception.ToString());
            }
        }
        public bool OpenQWIKFile()
        {
            try
            {
                StreamReader read = new StreamReader("Qwik.xml");
                XmlSerializer deserializer = new XmlSerializer(typeof(Storage));
                QStorage = (Storage)deserializer.Deserialize(read);
                read.Close();
                QStorage.ResetEventCode();
                return true;
            }
            catch (Exception exception)
            {
                NewUi.Log(exception.ToString());
                return false;
            }
        }
        public int GetAlarmsForDay(Event[] arr)
        {
            int j = 0, AlarmCount = 0;
            Event tmp = new Event();
            for (int i = 0; i < QStorage.RetrieveSize(); i++)
            {
                tmp = QStorage.RetrieveEvent(i);
                if (tmp.AlarmStatus == true && tmp.EndDate.Date == DateTime.Today && tmp.AlarmTime >= DateTime.Now && tmp.EventStatus == false)
                {
                    arr[j] = QStorage.RetrieveEvent(i);
                    j++;
                    AlarmCount++;
                }
            }
            return AlarmCount;
        }
    }
}

//This class stores the information of the basic unit of the to-do list: Event objects
//author: Foo Fang Hau
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace QWIK
{
    [Serializable()]
    public class Event : ISerializable
    {
        protected string event_name;
        public string EventName
        {
            get { return event_name; }
            set { event_name = value;
            if (value == "NULL")
                throw new SystemException("An evnet cannot be without a name!");
            }
        }
        protected DateTime start_date;
        public DateTime StartDate
        {
            get { return start_date; }
            set { start_date = value; }
        }
        protected DateTime end_date;
        public DateTime EndDate
        {
            get { return end_date; }
            set { end_date = value; }
        }
        protected DateTime start_time;
        public DateTime StartTime
        {
            get { return start_time; }
            set { start_time = value; }
        }
        protected DateTime end_time;
        public DateTime EndTime
        {
            get { return end_time; }
            set { end_time = value; }
        }
        protected bool event_status;
        public bool EventStatus //false means undone
        {
            get { return event_status; }
            set { event_status = value; }
        }
        protected bool alarm_status;
        public bool AlarmStatus
        {
            get { return alarm_status; }
            set { alarm_status = value; }
        }
        protected bool importance;
        public bool Importance
        {
            get { return importance; }
            set { importance = value; }
        }
        
        protected DateTime alarm_time;
        public DateTime AlarmTime
        {
            get { return alarm_time; }
            set { alarm_time = value; }
        }

        protected int event_code;
        public int EventCode
        {
            get { return event_code; }
            set
            {
                event_code = value;
            }
                //if (value < 0)
                //{ throw new SystemException("All events must be labelled with a positive nonzero code"); }
            
        }

        public Event()
        {
            EventCode = -1;
            EventName = null;
            StartDate = new DateTime();
            EndDate = new DateTime();
            StartTime = new DateTime();
            EndTime = new DateTime();
            Importance = false;
            AlarmStatus = false;
            AlarmTime = new DateTime();
            EventStatus = false;
        }
        public Event(Event e)
        {
            EventCode = e.EventCode;
            EventName = e.EventName;
            StartDate = e.StartDate;
            EndDate = e.EndDate;
            StartTime = e.start_time;
            EndTime = e.EndTime;
            Importance = e.Importance;
            AlarmStatus = e.AlarmStatus;
            AlarmTime = e.AlarmTime;
            EventStatus = e.EventStatus;
        }
        public Event(string name, DateTime startDate, DateTime endDate, DateTime startTime, DateTime endTime, bool alarmStatus, bool urgent,  DateTime alarmTime, int eventCode)
        {
            EventCode = eventCode;
            EventName = name;
            StartDate = startDate;
            EndDate = endDate;
            StartTime = startTime;
            EndTime = endTime;
            Importance = urgent;
            AlarmStatus = alarmStatus;
            AlarmTime = alarmTime;
            EventStatus = false;
        }

        //public string GetName()
        //{
        //    return EventName;
        //}

        //public void SetName(string newName)
        //{
        //    if (newName == "XD")
        //    { throw new SystemException("Event must have an event name!");}
        //    EventName = newName;
        //}

        //public DateTime GetStartDate()
        //{
        //    return StartDate;
        //}

        //public void SetStartDate(DateTime newDate)
        //{
        //    StartDate = newDate;
        //}

        //public DateTime GetEndDate()
        //{
        //    return EndDate;
        //}

        //public void SetEndDate(DateTime newDate)
        //{
        //    EndDate = newDate;
        //}

        //public DateTime GetStartTime()
        //{
        //    return StartTime;
        //}

        //public void SetStartTime(DateTime newTime)
        //{
        //    StartTime = newTime;
        //}

        //public DateTime GetEndTime()
        //{
        //    return EndTime;
        //}

        //public void SetEndTime(DateTime newTime)
        //{
        //    EndTime = newTime;
        //}
        public DateTime GetDeadline()
        {
            DateTime deadline = EndDate;
            if (StartTime == new DateTime(1, 1, 1, 0, 0, 0) && EndTime == new DateTime(1, 1, 1, 23, 59, 59))
            {
                deadline = deadline.AddHours(EndTime.Hour);
                deadline = deadline.AddMinutes(EndTime.Minute);
                deadline = deadline.AddSeconds(EndTime.Second);
            }
            else
            {
                deadline = deadline.AddHours(StartTime.Hour);
                deadline = deadline.AddMinutes(StartTime.Minute);
                deadline = deadline.AddSeconds(StartTime.Second);
            }
            return deadline;
        }
        //public bool GetEventStatus()
        //{
        //    return EventStatus;
        //}

        //public void SetEventStatus(bool status)
        //{
        //    EventStatus = status;
        //}

        //public bool CheckAlarmStatus()
        //{
        //    return AlarmStatus;
        //}

        //public void SetAlarm(bool alarmSet)
        //{
        //    AlarmStatus = alarmSet;
        //}

        //public DateTime GetAlarmTime()
        //{
        //    return AlarmTime;
        //}
      
        //public bool CheckImportance()
        //{
        //    return Importance;
        //}

        //public void SetImportance(bool urgent)
        //{
        //    Importance = urgent;
        //}
        //public void SetAlarmTime(DateTime alarmTime)
        //{
        //    AlarmTime = alarmTime;
        //}

        //public void SetEventCode(int code)
        //{
        //    EventCode = code;
        //}
        //public int RetrieveEventCode()
        //{
        //    return EventCode;
        //}
        
        //Deserialization constructor
        public Event(SerializationInfo info, StreamingContext ctxt)
        {
            EventName = (string)info.GetValue("EventName", typeof(string));
            StartDate = (DateTime)info.GetValue("StartDate", typeof(DateTime));
            EndDate = (DateTime)info.GetValue("EndDate", typeof(DateTime));
            StartTime = (DateTime)info.GetValue("StartTime", typeof(DateTime));
            EndTime = (DateTime)info.GetValue("EndTime", typeof(DateTime));
            EventStatus = (bool)info.GetValue("EventStatus", typeof(bool));
            AlarmStatus = (bool)info.GetValue("AlarmStatus", typeof(bool));
            Importance = (bool)info.GetValue("Importance", typeof(bool));
            AlarmTime = (DateTime)info.GetValue("AlarmTime", typeof(DateTime));
            EventCode = (int)info.GetValue("EventCode", typeof(int));
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("EventName", EventName);
            info.AddValue("StartDate", StartDate);
            info.AddValue("EndDate", EndDate);
            info.AddValue("StartTime", StartTime);
            info.AddValue("EndTime", EndTime);
            info.AddValue("EventStatus", EventStatus);
            info.AddValue("AlarmStatus", AlarmStatus);
            info.AddValue("Importance", Importance);
            info.AddValue("AlarmTime", AlarmTime);
            info.AddValue("EventCode", EventCode);
        }
    }
}

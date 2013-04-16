//This class manages the list of events and does the actual editing of the list itself.
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
    public class Storage : ISerializable
    {
        private List<Event> ListOfEvent;
        public List<Event> list
        {
            get { return ListOfEvent; }
            set { ListOfEvent = value; }
        }

        protected int eventcode;
        public Storage()
        {
            ListOfEvent = new List<Event>();
            eventcode = 0;
        }
        public Storage(Storage temp)
        {
            ListOfEvent = new List<Event>();
            foreach (Event e in temp.ListOfEvent)
                ListOfEvent.Add(new Event(e));
            eventcode = temp.eventcode;
        }
        public void AddEvent(Event newEvent)
        {
            ListOfEvent.Add(newEvent);
        }
        public bool DeleteEvent(int index)
        {
            if (index < ListOfEvent.Count)
            {
                ListOfEvent.RemoveAt(index);
                return true;
            }
            else return false;
        }
        public Event RetrieveEvent(int index)
        {
            return ListOfEvent.ElementAt(index);
        }
        public int RetrieveSize()
        {
            return ListOfEvent.Count;
        }
        public void incrementeventcode()
        {
            eventcode += 1;
        }
        public int geteventcode()
        {
            return eventcode;
        }
        public void ClearAllEvents()
        {
            ListOfEvent.Clear();
        }
        public void ResetEventCode()
        {
            for (int i = 0; i < ListOfEvent.Count(); i++)
            {
                ListOfEvent.ElementAt(i).EventCode = i;
            }
            eventcode = ListOfEvent.Count;
        }
        //Deserialization constructor.
        public Storage(SerializationInfo info, StreamingContext ctxt)
        {
            ListOfEvent = (List<Event>)info.GetValue("ListOfEvent", typeof(List<Event>));
            eventcode = (int)info.GetValue("checklistsize", typeof(int));
        }
        //Serialization function.

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ListOfEvent", ListOfEvent);
            info.AddValue("checklistsize", eventcode);
        }
    }
}

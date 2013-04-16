//This class is the mode of interaction between the user interface and application layer. It calls the Logic class to perform actions
// and does the final checking of the input by the user 
//authors: Yee Yee Htut and G Swetha
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
namespace QWIK
{
      
    public class Controller
    {  
        private static Logic QLogic;
        public static NewUi UI;
        public Controller() 
        {
            //username = name;
            QLogic = new Logic();
            UI = new NewUi();
            Application.Run(UI);
            
        }
        
        public static bool Add (string eventName, DateTime startDate, DateTime endDate, DateTime startTime, DateTime endTime,bool alarmStatus,bool urgent, DateTime alarmTime)
        {
            Logic.GetInstance().AddEvent(eventName, startDate, endDate, startTime, endTime, alarmStatus, urgent, alarmTime);
            return true;
        }
        public static string Edit(int index,string eventName, DateTime startDate, DateTime endDate, DateTime startTime, DateTime endTime, bool alarmStatus, bool urgent, DateTime alarmTime)
        {
            index--;
            try
            {
                if (index >= Logic.GetInstance().ForTempUse.Count || index < 0)
                    return null;
                // throw new System.IndexOutOfRangeException();
                else
                    return Logic.GetInstance().EditEvent(index, eventName, startDate, endDate, startTime, endTime, alarmStatus, urgent, alarmTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DateTime AddFreeEvent(string eventName, DateTime startDate, DateTime endDate, DateTime startTime, DateTime endTime, bool alarmStatus,bool urgent, DateTime alarmTime)
        {
            startDate = endDate = Logic.GetInstance().SearchFreeDay(startDate, endDate);
            Logic.GetInstance().AddEvent(eventName, startDate, endDate, startTime, endTime, alarmStatus, urgent, alarmTime);
            return endDate;
        }
        public static bool Delete(int indexNum)
        {
            indexNum--;
            try
            {
                if (indexNum >= Logic.GetInstance().ForTempUse.Count || indexNum < 0)
                {
                    //throw new System.IndexOutOfRangeException();
                    return false;
                }
                else
                {
                    if (Logic.GetInstance().DeleteEvent(indexNum))
                        return true;
                    else return false;
                }
            }
            catch (Exception ex)
            { throw ex; }
           
        }
        public static bool Search(string searchWord, ref List<string[]> searchResult)
        {
            if (Logic.GetInstance().SearchName(searchWord))
            {
                Logic.GetInstance().PassToUI();
                searchResult = Logic.GetInstance().ToUI;
                return true;
            }
            else
                return false;
        }
        public static bool View(DateTime startDate, DateTime endDate, ref List<string[]> viewResult)
        {
            if (Logic.GetInstance().ViewEvent(startDate, endDate))
            {
                Logic.GetInstance().PassToUI();
                viewResult = Logic.GetInstance().ToUI;
                return true;
            }
            else
                return false;
        }
        public static bool ViewAll(ref List<string[]> viewResult)
        {
            if (Logic.GetInstance().ViewAllEvent())
            {
                Logic.GetInstance().PassToUI();
                viewResult = Logic.GetInstance().ToUI;
                return true;
            }
            else
                return false;
        }
        public static bool ViewDone(ref List<string[]> viewResult)
        {
            if (Logic.GetInstance().ViewDoneEvent())
            {
                Logic.GetInstance().PassToUI();
                viewResult = Logic.GetInstance().ToUI;
                return true;
            }
            else
                return false;
        }
        public static bool ViewUrgent(ref List<string[]> viewResult)
        {
            if (Logic.GetInstance().ViewUrgentEvent())
            {
                Logic.GetInstance().PassToUI();
                viewResult = Logic.GetInstance().ToUI;
                return true;
            }
            else return false;
        }
        public static bool Undo()
        {
            return Logic.GetInstance().Undo();
            
        }
        public static void Clear(DateTime date)
        {
            Logic.GetInstance().Clear(date);
            
        }
        public static void ClearAll()
        {
            Logic.GetInstance().ClearAll();
        }
        public static bool Done(int index) //consider using event name
        {
            index--;
            try
            {
                if (index >= Logic.GetInstance().ForTempUse.Count || index < 0)
                    return false;
                //throw new System.IndexOutOfRangeException();
                else
                {
                    Logic.GetInstance().setEventStatus(index, true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool DoneAll()
        {
            for (int i = 0; i < Logic.GetInstance().ForTempUse.Count; i++)
            {
                Logic.GetInstance().setEventStatus(i, true);
            }
            return true;
        }
        public static bool DeleteAll()
        {
            for (int i = 0; i < Logic.GetInstance().ForTempUse.Count; i++)
            {
                Logic.GetInstance().DeleteEvent(i);
            }
            return true;
        }

        public static int GetAlarmList(Event[] arr)
        {
            int NoOfAlarms = 0;
            NoOfAlarms = Logic.GetInstance().GetAlarmsForDay(arr);
            return NoOfAlarms;
        }
    
    }
}
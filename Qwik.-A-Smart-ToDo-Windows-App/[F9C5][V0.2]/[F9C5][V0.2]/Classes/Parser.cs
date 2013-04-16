//This class interprets the format of the input and extracts the information appropriately so that it can be processed by other classes
//author: Yee Yee Htut
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace QWIK.Classes
{
    class Parser
    {
        private static string[] dateKey = { "by", "before", "due", "on", "to", "from", "this", "last", "next" }; 
        private static string[] timeKey = { "at", "from", "to", "by", "am", "pm" };
        private static string[] otherKey = { "urgent", "alarm", "this", "next", "week" };
        private static string[] days = { "mon", "tue", "tues", "wed", "thur", "thurs", "fri", "sat", "sun", "tmr", "monday", "tuesday", "wednesday", 
                                           "thursday", "friday","saturday", "sunday", "today", "tomorrow", "yesterday" };
        public static DateTime empty = new DateTime();

        public static bool AddEvent(string commandline, ref NewUi.EventInfo info)
        {
            string[] words = SplitWords(commandline);
            int length = words.Length;
            int j = 0;
            bool found = false;
            for (int i = 0; i < length; i = j)
            {
                found = false;
                if (dateKey.Contains(words[i].ToLower()) || days.Contains(words[i].ToLower()))
                {
                    j = FoundDateKey(words, i, ref info.startDate, ref info.endDate);
                    found = INotEqualJ(i, j);
                }
                if (timeKey.Contains(words[i].ToLower()) && !found)
                {
                    j = FoundTimeKey(words, i, ref info.startTime, ref info.endTime);
                    found = INotEqualJ(i, j);
                }
                if (otherKey.Contains(words[i].ToLower()) && !found)
                {
                    j = FoundOtherKey(words, i, ref info.urgent, ref info.alarm, ref info.alarmTime, ref info.freeDay, ref info.startDate, ref info.endDate);
                    found = INotEqualJ(i, j);
                }
                else
                {
                    if (char.IsDigit(words[i][0]))
                        if (IsTimeRelated(words, i))
                            j = FoundTimeKey(words, i, ref info.startTime, ref info.endTime);
                        else
                            j = j = FoundDateKey(words, i, ref info.startDate, ref info.endDate);
                }

                if (i == j)
                    if (info.eventName == null)
                        info.eventName = words[j++];
                    else info.eventName += ' ' + words[j++];
            }

            if (ValidEventInfo(ref info))
                return true;
            else return false;
        }
        public static bool EditEvent(string commandline, ref int index, ref NewUi.EventInfo info)
        {
            Debug.Assert(commandline != null, "Commandline was null in EditEvent function in Parser");
            string[] words = SplitWords(commandline);
            int j=0;
            bool found = false;
            if (Int32.TryParse(words[0], out index))
            {
                j = 1;
                for (int i = 1; i < words.Length; i = j)
                {
                    found = false;
                    if (dateKey.Contains(words[i].ToLower()) || days.Contains(words[i].ToLower()))
                    {
                        j = FoundDateKey(words, i, ref info.startDate, ref info.endDate);
                        found = INotEqualJ(i, j);
                    }
                    if (timeKey.Contains(words[i].ToLower()) && !found)
                    {
                        j = FoundTimeKey(words, i, ref info.startTime, ref info.endTime);
                        found = INotEqualJ(i, j);
                    }
                    if (otherKey.Contains(words[i].ToLower()) && !found)
                    {
                        j = FoundOtherKey(words, i, ref info.urgent, ref info.alarm, ref info.alarmTime, ref info.freeDay, ref info.startDate, ref info.endDate);
                        found = INotEqualJ(i, j);
                    }
                    else
                    {
                        if (char.IsDigit(words[i][0]))
                            if (IsTimeRelated(words, i))
                                j = FoundTimeKey(words, i, ref info.startTime, ref info.endTime);
                            else
                                j = j = FoundDateKey(words, i, ref info.startDate, ref info.endDate);
                    }

                    if (i == j)
                        if (info.eventName == null)
                            info.eventName = words[j++];
                        else info.eventName += ' ' + words[j++];
                }

                
                return true;
            }
            else return false;
        }
        private static bool INotEqualJ(int i, int j)
        {
            if (i == j)
                return false;
            else return true;
        }
        private static bool ValidEventInfo(ref NewUi.EventInfo info)
        {
            bool isValid = true;
            if (info.startDate == empty || info.endDate.CompareTo(DateOnly(DateTime.Now)) >= 0)
            {
                if (string.IsNullOrWhiteSpace(info.eventName))
                    info.eventName = "<unknown>";
                if (info.startTime == empty)
                {
                    if (info.endTime != empty)
                        info.startTime = info.endTime;
                    else if (info.alarmTime != empty)
                        info.startTime = info.alarmTime;
                    else info.endTime = EndOfDay();
                }
                if (info.alarm && info.alarmTime == empty)
                    info.alarmTime = info.startTime;
                
                if (info.endTime == empty && info.startTime != empty)
                    info.endTime = info.startTime;
                if (info.endDate == empty)
                    info.startDate = info.endDate = DateOnly(DateTime.Now);
                else
                {
                    if (info.startDate == empty)
                        info.startDate = DateOnly(DateTime.Now);
                }
                if (info.alarm)
                {
                    info.alarmTime = info.alarmTime.AddDays(info.endDate.Day-1);
                    info.alarmTime = info.alarmTime.AddMonths(info.endDate.Month-1);
                    info.alarmTime = info.alarmTime.AddYears(info.endDate.Year-1);
                }
            }
            else isValid = false;
            if (info.endDate.CompareTo(info.startDate) < 0)
                isValid = false;
            if (info.endTime.CompareTo(info.startTime) < 0)
                isValid = false;
            return isValid;
        }
        private static int FoundDateKey(string[] words, int index, ref DateTime startDate, ref DateTime endDate)
        {
            int j = index;
            string key = words[index].ToLower();
            DateTime tempDate = new DateTime();
            j = FindDates(words, j, ref tempDate);
            if (j != index)
            {
                if (key == "from")
                    startDate = tempDate;
                else if (key == "to")
                    endDate = tempDate;
                else if (key == "by" || key == "before" || key == "due")
                {
                    startDate = DateOnly(DateTime.Now);
                    endDate = tempDate;
                }
                else startDate = endDate = tempDate;
            }
            return j;
        }
        private static int FoundTimeKey(string[] words, int index, ref DateTime startTime, ref DateTime endTime)
        {
            int j = index;
            string key = words[index].ToLower();
            DateTime tempTime = new DateTime();
            j = FindTime(words, index, ref tempTime);
            if (j != index)
            {
                if (key == "from")
                    startTime = tempTime;
                else if (key == "to")
                    endTime = tempTime;
                else
                    startTime = endTime = tempTime;
            }
            return j;
        }
        private static int FoundOtherKey(string[] words, int i, ref bool urgent, ref bool alarm, ref DateTime alarmTime, ref bool freeDay, ref DateTime startDate, ref DateTime endDate)
        {
            int nextIndex = i;
            if (words[i].ToLower() == "urgent")
            {
                urgent = true;
                nextIndex = i + 1;
            }
            else if (words[i].ToLower() == "alarm")
            {
                alarm = true;
                nextIndex = i + 1;
                if (nextIndex < words.Length)
                    nextIndex = FindTime(words, i + 1, ref alarmTime);
            }
            else if (words[i].ToLower() == "this" || words[i].ToLower() == "next")
            {
                if (i + 1 < words.Length)
                {
                    if (GetWeek(words[i] + " " + words[i + 1], ref startDate, ref endDate))
                    {
                        freeDay = true;
                        nextIndex = i + 2;
                    }
                }
                else nextIndex = i;
            }
            return nextIndex;
        }

        public static int FindTime(string[] words, int index, ref DateTime time)
        {
            int nextIndex = index, temp = index;
            string test = GetTestTimePhrase(words, index, ref temp);
            if (test != null)
            {
                if (!ExtractTime(test, ref time))
                {
                    for (int t = test.Length - 1; t > 0; t--)
                    {
                        if (test[t] == ' ')
                        {
                            test = test.Substring(0, t);
                            temp--;
                            if (ExtractTime(test, ref time))
                            {
                                nextIndex = temp;
                                break;
                            }
                        }
                    }
                }
                else nextIndex = temp;
            }
            return nextIndex;
        }
        public static int FindDates(string[] words, int index, ref DateTime date)
        {
            int nextIndex = index, temp = index;
            string test = GetTestDatePhrase(words, index, ref temp);
            if (test != null)
            {
                if (!ExtractDate(test, ref date))
                {
                    for (int t = test.Length - 1; t > 0; t--)
                    {
                        if (test[t] == ' ')
                        {
                            test = test.Substring(0, t);
                            temp--;
                            if (ExtractDate(test, ref date))
                            {
                                nextIndex = temp;
                                break;
                            }
                        }
                    }
                }
                else nextIndex = temp;
            }
            return nextIndex;
        }
        private static string GetTestDatePhrase(string[] words, int index, ref int temp)
        {
            int start = index;
            string test = null;
            string[] keys = { "by", "before", "due", "on", "to", "from" };
            if (keys.Contains(words[start].ToLower()))
                start++;
            temp = start;
            if (!IsNonDateKey(words, temp))
            {

                if (start < words.Length)
                {
                    while (temp < words.Length)
                        if (!IsNonDateKey(words, temp))
                            temp++;
                        else break;
                    test = words[start];
                    for (int i = start + 1; i < temp; i++)
                        test += " " + words[i];
                }
            }
            return test;
        }
        private static string GetTestTimePhrase(string[] words, int index, ref int temp)
        {
            int start = index;
            if (timeKey.Contains(words[start].ToLower()))
                start++;
            temp = start;
            string test = null;
            if (start < words.Length)
            {
                while (temp < words.Length)
                    if (!IsNonTimeKey(words, temp))
                        temp++;
                    else break;
                test = words[start];
                for (int i = start + 1; i < temp; i++)
                    test += " " + words[i];
            }
            return test;
        }
        private static bool IsNonTimeKey(string[] words, int i)
        {
            string temp = words[i].ToLower();
            bool isOtherKey = false;
            if (timeKey.Contains(temp))
                isOtherKey = false;
            else if (dateKey.Contains(temp) || days.Contains(temp) || otherKey.Contains(temp))
                isOtherKey = true;
            else if (i > 0 && !timeKey.Contains(temp))
                if (words[i - 1].ToLower() == "am" || words[i - 1].ToLower() == "pm" || IsTimeRelated(words, i - 1))
                    isOtherKey = true;
            return isOtherKey;
        }
        private static bool IsNonDateKey(string[] words, int i)
        {
            string temp = words[i].ToLower();
            bool isOtherKey = false;
            if (dateKey.Contains(temp) || days.Contains(temp))
                isOtherKey = false;
            else if (timeKey.Contains(temp) || otherKey.Contains(temp))
                isOtherKey = true;
            else if (IsTimeRelated(words, i))
                isOtherKey = true;
            return isOtherKey;
        }

        public static bool GetThisOrNextDay(string phrase, ref DateTime date)
        {
            bool isDay = false;
            string[] words = SplitWords(phrase);
            if (words.Length == 2 && (words[0].ToLower() == "this" || words[0].ToLower() == "next" || words[0].ToLower() == "last"))
            {
                DateTime temp = new DateTime();
                if (ExtractDate(words[1], ref temp))
                {
                    if (temp.DayOfWeek < DateTime.Now.DayOfWeek)
                        if (words[0].ToLower() == "this")
                            date = temp.AddDays(-7);
                        else if (words[0].ToLower() == "last")
                            date = temp.AddDays(-14);
                        else date = temp;
                    else
                    {
                        if (words[0].ToLower() == "next")
                            date = temp.AddDays(7);
                        else if (words[0].ToLower() == "last")
                            date = temp.AddDays(-7);
                        else date = temp;
                    }
                    isDay = true;
                }
            }
            else isDay = false;
            return isDay;
        }
        public static bool GetWeek(string period, ref DateTime startDate, ref DateTime endDate)
        {
            bool isWeek = false;
            if (period.ToLower() == "this week")
            {
                startDate = endDate = DateOnly(DateTime.Today);
                while (endDate.DayOfWeek != DayOfWeek.Saturday)
                    endDate = endDate.AddDays(1);
                isWeek = true;
            }
            else if (period.ToLower() == "next week")
            {
                startDate = DateOnly(DateTime.Today);
                while (startDate.DayOfWeek != DayOfWeek.Sunday)
                    startDate = startDate.AddDays(1);
                endDate = startDate.AddDays(6);
                isWeek = true;
            }
            else isWeek = false;
            return isWeek;
        }
        public static bool GetPeriod(string period, ref DateTime startDate, ref DateTime endDate)
        {
            string[] words = SplitWords(period);
            int length = words.Length;
            int j = 0;
            bool valid = true;
            for (int i = 0; i < length; i = j)
            {
                if (dateKey.Contains(words[i]) || days.Contains(words[i]))
                {
                    j = FoundDateKey(words, i, ref startDate, ref endDate);
                }

                else
                {
                    if (char.IsDigit(words[i][0]))
                        if (!IsTimeRelated(words, i))
                            j = FoundDateKey(words, i, ref startDate, ref endDate);
                }

                if (i == j)
                {
                    valid = false;
                    break;
                }
            }
            return valid;
        }
        public static bool IsTimeRelated(string[] words, int i)
        {
            DateTime temp = new DateTime();
            int number;
            bool isTime = false;
            if (char.IsDigit(words[i][0]))
            {
                string test = words[i];
                test = test.Replace('.', ':');
                if (DateTime.TryParse(test, out temp))
                    if (test.ToLower().Contains("am") || test.ToLower().Contains("pm") || test.Contains(':'))
                        isTime = true;
                    else isTime = false;
                else if (int.TryParse(test, out number) && i < words.Length - 1)
                    if (words[i + 1].ToLower() == "am" || words[i + 1].ToLower() == "pm")
                        isTime = true;
            }
            else isTime = false;
            return isTime;
        }
        public static bool ExtractDate(string substring, ref DateTime date)
        {
            if (substring != null)
            {
                bool isDate = false;
                DateTime temp = new DateTime();
                if (GetThisOrNextDay(substring, ref temp))
                    isDate = true;
                else if (DateTime.TryParse(substring, out temp))
                    isDate = true;
                else
                {
                    string day = substring.ToLower();
                    if (days.Contains(day))
                    {
                        temp = DayToDate(substring);
                        isDate = true;
                    }
                    else isDate = false;
                }
                if (isDate)
                {
                    if (temp.CompareTo(DateTime.Now) < 0)
                        temp = temp.AddYears(1);
                    date = DateOnly(temp);
                }
                return isDate;
            }
            else return false;
        }
        public static bool ExtractTime(string substring, ref DateTime time)
        {
            if (substring != null)
            {
                bool isTime = false;
                string test = substring;
                test = test.Replace('.', ':');
                DateTime temp = new DateTime();
                if (DateTime.TryParse(test, out temp))
                    if (test.ToLower().Contains("am") || test.ToLower().Contains("pm") || test.Contains(':'))
                    {
                        time = TimeOnly(temp);
                        isTime = true;
                    }
                    else isTime = false;
                else isTime = false;
                return isTime;
            }
            else return false;
        }
        public static DateTime DateOnly(DateTime temp)
        {
            DateTime date = new DateTime();
            date = date.AddDays(temp.Day - 1);
            date = date.AddMonths(temp.Month - 1);
            date = date.AddYears(temp.Year - 1);
            return date;
        }
        public static DateTime TimeOnly(DateTime temp)
        {
            DateTime time = new DateTime();
            time = time.AddHours(temp.Hour);
            time = time.AddMinutes(temp.Minute);
            time = time.AddSeconds(temp.Second);
            return time;
        }
        public static DateTime EndOfDay()
        {
            DateTime time = new DateTime();
            time = time.AddHours(23);
            time = time.AddMinutes(59);
            time = time.AddSeconds(59);
            return time;
        }
        private static DateTime DayToDate(string day)
        {
            DateTime date;
            DayOfWeek find = DayOfWeek.Sunday;
            day = day.ToLower();
            switch (day)
            {
                case "mon":
                case "monday":
                    find = DayOfWeek.Monday;
                    break;
                case "tue":
                case "tues":
                case "tuesday":
                    find = DayOfWeek.Tuesday;
                    break;
                case "wed":
                case "wednesday":
                    find = DayOfWeek.Wednesday;
                    break;
                case "thur":
                case "thurs":
                case "thrusday":
                    find = DayOfWeek.Thursday;
                    break;
                case "fri":
                case "friday":
                    find = DayOfWeek.Friday;
                    break;
                case "sat":
                case "saturday":
                    find = DayOfWeek.Saturday;
                    break;
                case "sun":
                case "sunday":
                    find = DayOfWeek.Sunday;
                    break;
            }
            date = DateTime.Now;
            if (day != "today")
                if (day == "tomorrow" || day == "tmr")
                    date = date.AddDays(1);
                else if (day == "yesterday")
                    date = date.AddDays(-1);
                else
                    while (date.DayOfWeek != find)
                        date = date.AddDays(1);
            return date;
        }
        private static string[] SplitWords(string commandLine)
        {
            string[] words = commandLine.Split(' ', '\r', '\n');
            int count = 0, length = 0;
            foreach (string s in words)
                if (!string.IsNullOrWhiteSpace(s))
                    length++;
            string[] compress = new string[length];
            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(words[i]))
                    compress[count++] = words[i];
            }
            return compress;
        }
    }

}

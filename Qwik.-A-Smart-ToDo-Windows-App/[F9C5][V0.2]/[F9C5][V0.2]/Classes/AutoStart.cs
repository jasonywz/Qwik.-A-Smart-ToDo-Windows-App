//this class is used for the auto-run of the software on startup of the computer 
//author: Yu Wei Zhong
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace QWIK
{
        public class AutoStart
        {
            private const string RUN_LOCATION = @"Software\Microsoft\Windows\CurrentVersion\Run";
            public static void SetAutoStart(string keyName, string assemblyLocation)
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
                key.SetValue(keyName, assemblyLocation);
            }
            public static bool IsAutoStartEnabled(string keyName, string assemblyLocation)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_LOCATION);
                if (key == null)
                    return false;
                string value = (string)key.GetValue(keyName);
                if (value == null)
                    return false;
                return (value == assemblyLocation);
            }
            public static void UnSetAutoStart(string keyName)
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(RUN_LOCATION);
                key.DeleteValue(keyName);
            }
        }
}

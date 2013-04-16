using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace QWIK
{
    public class LoginCtrl
    {
        static LoginStorage _Data;
        static LoginCtrl LgCtrl;
        private static string filename = "Restricted_Data.qwik";
        public LoginCtrl()
        {
            if(!OpenFile())
                _Data = new LoginStorage();
        }
        public static LoginCtrl thisCtrl()
        {
            if (LgCtrl == null)
                LgCtrl = new LoginCtrl();
            return LgCtrl;
        }
        public LoginStorage GetStorage()
        {
            return _Data;
        }
        public bool Valid_Un(string username)
        {
            int i;
            for (i = 0; i < (_Data.GetUserList().Count()); i++)
            {
                if (username == SplitString(_Data.GetUserList()[i])[0])
                    return true;
            }
            return false;
        }

        public bool Valid_Pw(string password)
        {
            int i;
            for (i = 0; i < (_Data.GetUserList().Count()); i++)
            {
                if (password == SplitString(_Data.GetUserList()[i])[1])
                    return true;
            }
            return false;
        }

        private static string[] SplitString(string input)
        {
            string[] sstring = input.Split(' ');
            return sstring;
        }
        
        //Read from file and deserialize
        //load file
        private bool OpenFile()
        {
            LoginStorage temp;
            if (File.Exists(filename))
            {
                Stream stream = File.Open(filename, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                temp = (LoginStorage)bformatter.Deserialize(stream);
                stream.Close();
                _Data = temp;
                return true;
            }
            else return false;

        }
        //Serialze and write to file
        public void WriteFile()
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, _Data);
            stream.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
namespace QWIK
{
    [Serializable()]
    public class LoginStorage : ISerializable
    {
        static LoginStorage LStore;
        private static List<string> UserList;

        public LoginStorage()
        {
            UserList = new List<string> ();
        }
     
        public List<string> GetUserList()
        {
            return UserList;
        }
         //Deserialization constructor.
        public LoginStorage(SerializationInfo info, StreamingContext ctxt)
        {
            UserList = (List<string>)info.GetValue("UserList", typeof(List<string>));
        }
        //Serialization function.

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("UserList", UserList);
        }
        
    }
}
/*
namespace QWIK
{
    public class LoginStorage
    {
        static LoginStorage LStore;
        private static string filename= "Restricted_Data.txt";
        private LoginStorage()
        {
            if(!(File.Exists(filename))){
                File.Create(filename).Dispose(); 
            }
        }

        public static LoginStorage LgStore()//create an instance of the Storage
        {
            if (LStore == null)
            {
                LStore = new LoginStorage();
            }
            return LStore;
        }

        
        public List<String> ReadFile()
        {
            
            List<String> NewList = new List<string>();
            string line = "";
            using (StreamReader _sr = new StreamReader(filename))//reads the text from file
            {
                while ((line = _sr.ReadLine()) != null)
                {
                    NewList.Add(line);
                }
                _sr.Close();
            }
            return NewList;
        }

        public void WriteFile(List<String> NewList)
        {
            using (StreamWriter _sw = new StreamWriter(filename))
            {
                foreach (string s in NewList)
                {
                    _sw.WriteLine(s);//writes the line back into file.
                }
                _sw.Close();
            }
        }
    }
}
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShortcutUIv1
{
    public class Serializer
    {
        public Serializer()
        {
        }
        //public void SerializeObject(string filename, Storage objectToSerialize)
        //{
        //    Stream stream = File.Open("data_donotdelete", FileMode.Create);
        //    BinaryFormatter bFormatter = new BinaryFormatter();
        //    bFormatter.Serialize(stream, objectToSerialize);
        //    stream.Close();
        //}
        //public Storage DeSerializeObject(string filename)
        //{
        //    Storage objectToSerialize;
        //    Stream stream = File.Open("data_donotdelete", FileMode.Open);
        //    BinaryFormatter bFormatter = new BinaryFormatter();
        //    objectToSerialize = (Storage)bFormatter.Deserialize(stream);
        //    stream.Close();
        //    return objectToSerialize;
        //}
    }
}

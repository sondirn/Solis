using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Solis.Utils
{
    public class SolisXML
    {
        public static void ToXML<T>(T instance, string path) where T : class
        {
            XmlSerializer writer = new XmlSerializer(typeof(T));

            FileStream file = File.Create(path);
            try
            {
            writer.Serialize(file, instance);
            file.Close();
            }
            catch
            {
                throw new Exception("Could not save file");
            }
        }

        public static T FromXML<T>(string path) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (Stream reader = new FileStream(path, FileMode.Open))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}

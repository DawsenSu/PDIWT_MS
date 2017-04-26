using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace PDIWT_MS_CZ.Models
{
    public static class XmlSerializerHelper
    {
        public static void SaveToXml(string filePath, object sourceObj, Type type, string xmlRootName)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
            {
                type = type ?? sourceObj.GetType();
                using ( StreamWriter writer = new StreamWriter(filePath))
                {
                    XmlSerializer xmlSerializer = string.IsNullOrWhiteSpace(xmlRootName) ? new XmlSerializer(type) : new XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                    xmlSerializer.Serialize(writer, sourceObj);
                }
            }
        }

        public static T LoadFromXml<T>(string filePath) where T :class
        {
            T result = null;
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    result =(T)xmlSerializer.Deserialize(reader);
                }
            }
            return result;
        }
    }
}

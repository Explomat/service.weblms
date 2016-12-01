using System;
using System.Xml.Serialization;
using System.IO;

namespace Service.Model.Utils
{
    class XmlSettings
    {
        public static XmlModel LoadXml(string path)
        {
            XmlSerializer reader = new XmlSerializer(typeof(XmlModel));
            StreamReader file = new StreamReader(path);
            XmlModel overview = (XmlModel)reader.Deserialize(file);
            file.Close();
            return overview;
        }
    }
}

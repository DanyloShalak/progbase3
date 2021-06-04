using System.Xml.Serialization;
using System.Text;

namespace XMLModule
{
    public static class Serialiser
    {
        public static string SerialiseObj<T>(T item)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = false;
            settings.NewLineHandling = System.Xml.NewLineHandling.None;
            StringBuilder sb = new StringBuilder();

            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sb, settings);
            serializer.Serialize(writer, item);
            
            return sb.ToString();
        }
    }
}
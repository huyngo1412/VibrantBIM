using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using VibrantBIM.ViewModels;

namespace VibrantBIM.Extensions
{
    public class CXVCruid
    {
        private static XmlDocument _xmlDocument;
        public static string FilePathCXV = "";
        public static void CreateFile(DataContainer dataContainer, string FilePath)
        {
            var xmlSerializer = new XmlSerializer(typeof(DataContainer));
            using (var write = new StreamWriter(FilePath))
            {
                xmlSerializer.Serialize(write, dataContainer);
            }
            MessageBox.Show("Export Successful");
        }
        public static void UpdateFile(string filename,string XPath, string check,string NodeCheck,string NodeEdit,string value)
        {
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(filename);
            XmlNodeList NodeList = _xmlDocument.SelectNodes(XPath);
            foreach (XmlNode Node in NodeList)
            {
                XmlNode GetNodeEdit = Node.SelectSingleNode(NodeEdit);
                XmlNode GetNodeCheck = Node.SelectSingleNode(NodeCheck);
                if (GetNodeCheck.InnerText == check)
                {
                    GetNodeEdit.InnerText = value;
                }
            }
            _xmlDocument.Save(filename);

        }
        public static DataContainer ReadFile(string FilePath)
        {
            FilePathCXV = FilePath;
            var xmlSerializer = new XmlSerializer(typeof(DataContainer));
            try
            {
                using (TextReader reader = new StreamReader(FilePathCXV))
                {
                    return (DataContainer)xmlSerializer.Deserialize(reader);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Deserialization error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    MessageBox.Show($"Inner Exception: {ex.InnerException.Message}");
                }
            }
            return null;
        }
    }
}

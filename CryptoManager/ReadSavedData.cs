using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CryptoManager
{
    public static class ReadSavedData
    {
        public static string[] Exchanges;
        public static void LoadSelectedExchange()
        {
            /*Fileba menti az éppen kiválasztott váltó api linkjét.*/
            
            XmlSerializer xml = new XmlSerializer(typeof(Properties_));
            FileStream fileStream = File.Open("properties.xml", FileMode.Open);
            Properties_ prop = xml.Deserialize(fileStream) as Properties_;
            Properties_.StSelectedEchange = prop.SelectedEchange;
            fileStream.Close();
            

        }
        public static void WriteSelectedExchange()
        {
            /*Fileba menti az éppen kiválasztott váltó api linkjét.*/
            XmlSerializer xml = new XmlSerializer(typeof(Properties_));
            Properties_ prop = new Properties_();
            prop.SelectedEchange = Properties_.StSelectedEchange;
            FileStream fileStream = File.Open("properties.xml", FileMode.Open);
            xml.Serialize(fileStream, prop);
            fileStream.Close();
        }
        

        
    }
}

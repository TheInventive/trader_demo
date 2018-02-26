using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace CryptoManager
{
    public static class APIClient
    {
        private static HttpClient client = new HttpClient();
        private static readonly string Backup =  "https://api.coinmarketcap.com/v1/ticker/";
        public static string Path = "https://api.coinmarketcap.com/v1/ticker/";
        
        public static void ResetPath()
        {
            Path = Backup;
        }

        public static async Task<List<Crypto>> GetProductAsync(string path)
        {
            List<Crypto> product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<List<Crypto>>();
            }
            return product;
        }

        public static async Task<JArray> GetJsonAsync(string path)
        {
            JArray product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<JArray>();
            }
            return product;
        }

        public static async Task<List<dynamic>> ConvertToDynamicList()
        {
            List<string> array = await ConvertToStringList();
            List<dynamic> list = new List<dynamic>();
            for (int i = 0; i < array.Count; i++)
            {
                list.Add(JsonConvert.DeserializeObject<dynamic>(array[i]));
            }

            return list;
        }
        public static async Task<List<string>> ConvertToStringList()
        {
            JArray array = await GetJsonAsync(Path);
            List<string> json = new List<string>();
            for (int i = 0; i < array.Count; i++)
            {
                json.Add(array[i].ToString());
            }
            return json;
        }


        public static async Task WriteProductToFile()
        {
            var product = await GetProductAsync(Path);
            XmlSerializer xml = new XmlSerializer(typeof(List<Crypto>));
            TextWriter fileStream = new StreamWriter(@"docs5.xml");
            try
            {
                if (product != null)
                        xml.Serialize(fileStream, product);
                        
                else
                    MessageBox.Show("Product is null.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }
            finally
            {

                fileStream.Close();
            }
        

        }
        public static List<Crypto> ReadProductToObject()
        { 
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<Crypto>));
                
                using (TextReader sr = new StreamReader(@"docs5.xml"))
                {
                    List<Crypto> c = (List<Crypto>)xml.Deserialize(sr);
                    
                    return c;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                

            }
            finally {  }
            return null;
        }
        public static void WriteRecordToFile(List<Record> list)
        {
            var product = list;
            XmlSerializer xml = new XmlSerializer(typeof(List<Record>));
            TextWriter fileStream = new StreamWriter(@"op.xml");
            try
            {
                if (product != null)
                     xml.Serialize(fileStream, product);



                //Make the function write the xlm document in a ... instead
                else
                    MessageBox.Show("Product is null.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }
            finally
            {

                fileStream.Close();
            }

        }
        public static List<Record> ReadRecordToObject()
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<Record>));

                using (TextReader sr = new StreamReader(@"op.xml"))
                {
                    List<Record> c = (List<Record>)xml.Deserialize(sr);

                    return c;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }
            finally { }
            return null;
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoManager
{
    public class Crypto
    {
        public Crypto( ) 
        {
           
        }
        public string name { get; set; }
        public string price_usd { get; set; }
        public string symbol { get; set; }
        public string price_eur { get; set; }
        public string price_huf { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoManager
{
    public  class Properties_
    {
        public static string StSelectedEchange { get; set; }
        public  string SelectedEchange { get; set; }

        public enum Exchange
        {
            Coinmarketcap = 5
        }
        public enum Currency
        {
            USD, EUR, HUF
        }
        public enum Style
        {
            Default,
            White,
            Black
        }
    }
}

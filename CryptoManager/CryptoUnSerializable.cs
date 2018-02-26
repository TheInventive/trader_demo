using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoManager
{
    class CryptoUnSerializable :Crypto
    {
        public CryptoUnSerializable(string name = "sample", string price_usd = "sample", string symbol = "sample")
        {
            this.name = name;
            this.price_usd = price_usd;
            this.symbol = symbol;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace CryptoManager
{
    public class Record :Crypto
    {
        public DateTime Creation { get; set; }
        public DateTime Close { get; set; }
        public double? OpenPrice { get; set; }
        public double? ClosePrice { get; set; }
        public double? Profit { get; set; }
        public bool IsClosed { get; set; }

        public Record()
        {
            this.Creation = DateTime.Now;
            this.IsClosed = false;
            
        }
        public void CloseThis( double closePrice) 
        {
            if (!this.IsClosed)
            {
                this.Close = DateTime.Now;
                var base1 = APIClient.ReadProductToObject();
                Crypto base2 = base1.Find(x => x.name == name);
                this.ClosePrice = double.Parse(base2.price_usd.Replace('.',',')); 
                this.Profit =  this.ClosePrice * (double.Parse(price_usd) / OpenPrice) - double.Parse(this.price_usd); //Get the individual pieces  instead.
                this.IsClosed = true;
            }
        }

        public double? ProfitPercentage()
        {
             return this.Profit / this.OpenPrice * 100;
        }
       


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoManager
{
    public partial class ExForm : Form
    {
        public string RegisterPrice { get; set; }
        public Crypto Crypto { get; set; }
        public ExForm(Crypto crypto)
        {
            InitializeComponent();
            this.Crypto = crypto;
        }

   
        private void button2_Click(object sender, EventArgs e)
        {

        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
                {
                try
                {
                    textBox2.TextChanged -= textBox2_TextChanged; //Eltávolítja az EventHandler-t
                    string base1 = Crypto.price_usd.Replace('.', ','); //Alap: ár
                    decimal base2 = Convert.ToDecimal(base1); // Konverálás
                    string base3 = textBox1.Text.Replace('.', ','); //Alap2: felhasznáó által megadott mennyiség
                    decimal base4 = 0;
                    if (decimal.TryParse(base3, out base4))
                    {
                        textBox2.Text = (base2 * base4).ToString(); //Végeredmény

                    }
                    textBox2.TextChanged += textBox2_TextChanged;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }

        }

        public void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox2.Text))
            {
                try
                {
                    textBox1.TextChanged -= textBox1_TextChanged; //Eltávolítha az EventHandler-t
                    string base1 = Crypto.price_usd.Replace('.', ',');
                    decimal base2 = Convert.ToDecimal(base1);
                    string base3 = textBox2.Text.Replace('.', ',');
                    decimal base4 = 0;
                    if (decimal.TryParse(base3, out base4))
                    {
                        if (base2 != 0)
                            textBox1.Text = (base4 / base2).ToString();
                    }
                    textBox1.TextChanged += textBox1_TextChanged;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.RegisterPrice = textBox2.Text;
        }
    }
}

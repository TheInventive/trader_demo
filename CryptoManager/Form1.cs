using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CryptoManager
{
    public partial class Form1 : Form
    {
        public static List<Crypto> CryptoList;
        public static List<Record> RecordList;
        public static string SelectedCurrency = "USD";

        public Form1()
        {
            InitializeComponent();
            
            
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;

            comboBox1.DataSource = Enum.GetValues(typeof(Properties_.Exchange));
            timer1.Interval = 60000;
            timer1.Start();

            UpdateMarkets();
        
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.Bisque;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;

            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.CadetBlue;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Azure;

            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            dataGridView2.RowsDefaultCellStyle.BackColor = Color.Bisque;
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.None;

            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.CadetBlue;
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.Azure;

            dataGridView2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;


            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
       
       
        private static async void  RetrieveData()
        {
            await APIClient.WriteProductToFile();
        }
       

        private void Button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage1");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage2");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage3");
            UpdateTable();
            
        }
        public void UpdateMarkets()
        {
                CryptoList = APIClient.ReadProductToObject();
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
            switch (SelectedCurrency)
            {
                case "USD": dt.Columns.Add("Price in USD", typeof(string)); break;
                case "EUR": dt.Columns.Add("Price in EUR", typeof(string)); break;
                case "HUF": dt.Columns.Add("Price in HUF", typeof(string)); break;
                default: dt.Columns.Add("Price in USD", typeof(string)); break;
            }
                
                dt.Columns.Add("Symbol", typeof(string));

                if (CryptoList != null)
                    foreach (var record in CryptoList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Name"] = record.name;
                    switch (SelectedCurrency)
                    {
                        case "USD": dr["Price in USD"] = record.price_usd.ToString(); break;
                        case "EUR":  dr["Price in EUR"] = record.price_eur.ToString(); break;
                        case "HUF": dr["Price in HUF"] = record.price_huf.ToString(); break;
                        default: dr["Price in USD"] = record.price_usd.ToString(); break;
                    }
                        dr["Symbol"] = record.symbol;

                        dt.Rows.Add(dr);
                    }

                dataGridView1.DataSource = dt;
        }
        public  void UpdateTable()
        {
            if (File.Exists(@"op.xml"))
            {
                RecordList = APIClient.ReadRecordToObject();
                DataTable dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Price", typeof(string));
                dt.Columns.Add("Symbol", typeof(string));
                dt.Columns.Add("Created", typeof(string));
                dt.Columns.Add("Closed", typeof(string));
                dt.Columns.Add("OpenPrice", typeof(string));
                dt.Columns.Add("ClosePrice", typeof(string));
                dt.Columns.Add("Profit", typeof(string));
                dt.Columns.Add("IsClosed", typeof(string));

                if (RecordList != null)
                    foreach (var record in RecordList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Name"] = record.name;
                        dr["Price"] = record.price_usd.ToString(); //Variable here;
                        dr["Symbol"] = record.symbol;
                        dr["Created"] = record.Creation.ToString();
                        dr["Closed"] = record.Close.ToString();
                        dr["OpenPrice"] = record.OpenPrice.ToString();
                        dr["ClosePrice"] = record.ClosePrice.ToString();
                        dr["Profit"] = record.Profit.ToString();
                        dr["IsClosed"] = record.IsClosed.ToString();
                        dt.Rows.Add(dr);
                    }

                dataGridView2.DataSource = dt;
            }
        }
        

        private void Button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage4");
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button5_Click(object sender, EventArgs e)
        {
          Properties_.StSelectedEchange = comboBox1.SelectedValue.ToString();
            ReadSavedData.WriteSelectedExchange();
            
        }


        private  void Timer1_Tick(object sender, EventArgs e)
        {
            //Az árak 1 percenként kerülnek frissítésre.
            RetrieveData();
            UpdateMarkets();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string searchValue = textBox1.Text;

            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Value.ToString().Equals(searchValue))
                        {
                            row.Selected = true;
                            break;
                        }
                    }
                }
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.SelectedRows[0].Index;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        private void RecordButton_Click(object sender, EventArgs e)
        {
            /***/
            
            //A DataGridViewCollection nem serializálható, ezért kell neki egy új osztály
            string _name = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string _price = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string _symbol = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            CryptoUnSerializable crypto = new CryptoUnSerializable(_name,_price,_symbol);
            
            //Record button
            ExForm ex = new ExForm(crypto);
            ex.label2.Text = crypto.symbol;/*Ez állítja be a szimbólumot*/
            ex.textBox1.Text = "1";
            ex.textBox1_TextChanged(null, null);
            List<Record> list = new List<Record>();
            if (File.Exists("op.xml"))
            {
                list.AddRange(APIClient.ReadRecordToObject());
            }
            if (ex.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Record rc = new Record();
                    rc.name = crypto.name;
                    rc.price_usd = ex.RegisterPrice;
                    rc.symbol = crypto.symbol;
                    rc.OpenPrice = /*double.Parse(rc.price_usd) /2;*/ double.Parse(crypto.price_usd.Replace('.', ','));
                    list.Add(rc);
                    APIClient.WriteRecordToFile(list);
                }
                catch (Exception)
                {

                    MessageBox.Show("You didn't enter a value.");
                }
                
            }
        }

        private void CloseOrderButton_Click(object sender, EventArgs e)
        {
            var selectedRow = dataGridView2.SelectedRows[0];
            CryptoList = APIClient.ReadProductToObject();
            var matches = CryptoList.Where(p => p.name == selectedRow.Cells[0].Value.ToString()).ToList();
            List<Record> list = new List<Record>();
            if (File.Exists("op.xml"))
            {
                list.AddRange(APIClient.ReadRecordToObject());
            }
            list[selectedRow.Index].CloseThis(double.Parse(matches[0].price_usd.Replace('.',',')));
            MessageBox.Show(String.Format("Order closed. Your profit is {0}%",list[selectedRow.Index].ProfitPercentage().ToString()));
            APIClient.WriteRecordToFile(list);
            UpdateTable();
              
        }

        private  async void button6_Click(object sender, EventArgs e)
        {
            ChangeAttribute ch = new ChangeAttribute();
            ch.comboBox1.DataSource = Enum.GetValues(typeof(Properties_.Currency));
            if (ch.ShowDialog() == DialogResult.OK)
            {
                label4.Text = ch.comboBox1.SelectedValue.ToString();
                if (ch.comboBox1.SelectedValue.ToString() == "USD")
                {
                    SelectedCurrency = "USD";
                    APIClient.Path = "https://api.coinmarketcap.com/v1/ticker/";
                    await APIClient.WriteProductToFile();
                    UpdateMarkets();
                }
                else
                {
                    SelectedCurrency = ch.comboBox1.SelectedValue.ToString();
                    APIClient.Path = "https://api.coinmarketcap.com/v1/ticker/?convert=" + ch.comboBox1.SelectedValue.ToString();
                    await APIClient.WriteProductToFile();
                    UpdateMarkets();
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ChangeAttribute ch = new ChangeAttribute();
            ch.comboBox1.DataSource = Enum.GetValues(typeof(Properties_.Style));
            if (ch.ShowDialog() == DialogResult.OK)
            {
                //Write to file the default style then restyle all windows, also make the program
                //load the selected style at start
                Properties_.Style selection = (Properties_.Style)Enum.Parse(typeof(Properties_.Style), ch.comboBox1.Text);
                SetStyle(selection);
                label5.Text = ch.comboBox1.Text;
                
            }
            
        }
        private Color[] SetColors(Color TabsColor, Color PanelColor ,Color ButtonBackColor, Color ButtonForeColor, Color LabelsForeColor)
        {
            Color[] array = { TabsColor, PanelColor, ButtonBackColor, ButtonForeColor, LabelsForeColor };
            return array;
        }
        private void SetStyle(Properties_.Style style)
        {
            Color[] ColorSettings = null;
            switch (style)
            {
                case Properties_.Style.White : ColorSettings = 
                    SetColors(Color.FromArgb(219, 223, 234),
                    Color.FromArgb(177, 197, 196),
                    Color.FromArgb(123, 171, 237),
                    Color.White,
                    Color.FromArgb(14, 67, 117)); break;
                case Properties_.Style.Black: ColorSettings = 
                    SetColors(Color.FromArgb(219, 223, 234),
                    Color.FromArgb(177, 197, 196),
                    Color.FromArgb(123, 171, 237),
                    Color.White,
                    Color.FromArgb(14, 67, 117)); break;
                case Properties_.Style.Default: ColorSettings = 
                    SetColors(Color.FromArgb(26, 32, 40),
                    Color.FromArgb(26, 32, 40),
                    Color.FromArgb(26, 32, 40),
                    Color.FromArgb(153, 180, 209),
                    Color.FromArgb(153, 180, 209)); break;
            }

            tabPage1.BackColor = ColorSettings[0];
            tabPage2.BackColor = ColorSettings[0];
            tabPage3.BackColor = ColorSettings[0];
            tabPage4.BackColor = ColorSettings[0];
            panel1.BackColor = ColorSettings[1];
            button1.BackColor = ColorSettings[2];
            button2.BackColor = ColorSettings[2];
            button3.BackColor = ColorSettings[2];
            button4.BackColor = ColorSettings[2];
            button1.ForeColor = ColorSettings[3];
            button2.ForeColor = ColorSettings[3];
            button3.ForeColor = ColorSettings[3];
            button4.ForeColor = ColorSettings[3];
            label1.ForeColor = ColorSettings[4];
            label2.ForeColor = ColorSettings[4];
            label3.ForeColor = ColorSettings[4];
            label4.ForeColor = ColorSettings[4];
            label5.ForeColor = ColorSettings[4];
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string searchValue = textBox2.Text;

            try
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Value.ToString().Equals(searchValue))
                        {
                            row.Selected = true;
                            break;
                        }
                    }
                }
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.SelectedRows[0].Index;
            }
            catch (Exception) { }
        }
    }
}

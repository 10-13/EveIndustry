using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;
using EveEchoesIndustry.Items;

namespace EveIndustry.Legacy.Tests
{
    public partial class ParsersTest : Form
    {
        public ParsersTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = JsonConvert.SerializeObject(InventoryListConverter.FromIngameList(Clipboard.GetText()));
            richTextBox1.Text = InventoryListConverter.ToIngameList(JsonConvert.DeserializeObject<InventoryList>(richTextBox2.Text));
        }
    }
}

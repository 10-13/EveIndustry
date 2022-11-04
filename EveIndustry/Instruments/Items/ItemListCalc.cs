using EveEchoesIndustry.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveIndustry.Instruments.Items
{
    public partial class ItemListCalc : Form
    {
        private InventoryList A;
        private InventoryList B;

        public ItemListCalc()
        {
            InitializeComponent();
            A = new InventoryList();
            B = new InventoryList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            A = InventoryListConverter.FromIngameList(Clipboard.GetText());
            if (A == null)
                A = new InventoryList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ItemListVeiw f = new ItemListVeiw(A);
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            B = InventoryListConverter.FromIngameList(Clipboard.GetText());
            if (B == null)
                B = new InventoryList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ItemListVeiw f = new ItemListVeiw(B);
            f.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            InventoryList C = A.Clone() as InventoryList;
            C.AddList(B.Clone() as InventoryList);
            ItemListVeiw f = new ItemListVeiw(C);
            f.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            InventoryList C = A.Clone() as InventoryList;
            C.RemoveList(B.Clone() as InventoryList, out InventoryList empty);
            ItemListVeiw f = new ItemListVeiw(C);
            f.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            InventoryList C = A.Clone() as InventoryList;
            C.RemoveList(B.Clone() as InventoryList, out InventoryList empty);
            ItemListVeiw f = new ItemListVeiw(empty);
            f.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(int.TryParse(textBox1.Text,out int r))
            {
                InventoryList C = A.Clone() as InventoryList;
                C.Multiply(r);
                ItemListVeiw f = new ItemListVeiw(C);
                f.Show();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int r))
            {
                InventoryList C = A.Clone() as InventoryList;
                C.Multiply(null,r);
                ItemListVeiw f = new ItemListVeiw(C);
                f.Show();
            }
        }
    }
}

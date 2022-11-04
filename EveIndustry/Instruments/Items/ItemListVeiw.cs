using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EveEchoesIndustry.Items;
using EveEchoesIndustry.Services;
using Newtonsoft.Json;

namespace EveIndustry.Instruments.Items
{
    public partial class ItemListVeiw : Form
    {
        private InventoryList List;

        public ItemListVeiw(InventoryList list = null)
        {
            InitializeComponent();
            List = list;
            if (List == null)
                List = new InventoryList();
            UpdateListData();
            UpdateTextData();
        }

        private void UpdateListData()
        {
            listBox1.Items.Clear();
            foreach (var item in List.Items)
            {
                listBox1.Items.Add(InventoryListConverter.ToIngameRow(item));
            }
        }

        private void UpdateTextData()
        {
            richTextBox1.Text = InventoryListConverter.ToIngameList(in List);
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex > -1)
            {
                toolStripTextBox1.Text = List.Items[listBox1.SelectedIndex].Cost.ToString();
                toolStripTextBox2.Text = List.Items[listBox1.SelectedIndex].Name;
                toolStripTextBox3.Text = List.Items[listBox1.SelectedIndex].Count.ToString();
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex > -1)
            {
                if(double.TryParse(toolStripTextBox1.Text,out double res))
                {
                    this.List.Items[listBox1.SelectedIndex].Cost = res;
                    listBox1.Items[listBox1.SelectedIndex] = InventoryListConverter.ToIngameRow(this.List.Items[listBox1.SelectedIndex]);
                }
            }
        }
        
        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            UpdateTextData();
        }

        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                this.List.Items[listBox1.SelectedIndex].Name = toolStripTextBox2.Text;
                listBox1.Items[listBox1.SelectedIndex] = InventoryListConverter.ToIngameRow(this.List.Items[listBox1.SelectedIndex]);
            }
        }

        private void toolStripTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                if (long.TryParse(toolStripTextBox3.Text, out long res))
                {
                    this.List.Items[listBox1.SelectedIndex].Count = res;
                    listBox1.Items[listBox1.SelectedIndex] = InventoryListConverter.ToIngameRow(this.List.Items[listBox1.SelectedIndex]);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List.Items.Add(new InventoryItem());
            List.Items[List.Items.Count - 1].Id = List.Items.Count;
            listBox1.Items.Add(InventoryListConverter.ToIngameRow(List.Items[List.Items.Count - 1]));
            UpdateTextData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex > -1)
            {
                List.Items.RemoveAt(listBox1.SelectedIndex);
                for(int i = listBox1.SelectedIndex; i < List.Items.Count;i++)
                    List.Items[i].Id = i + 1;
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                UpdateTextData();
                UpdateListData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List = InventoryListConverter.FromIngameList(Clipboard.GetText());
            UpdateListData();
            UpdateTextData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.List.Validate();
            Clipboard.SetText(InventoryListConverter.ToIngameList(in this.List));
        }

        private void ItemListVeiw_Load(object sender, EventArgs e)
        {
            
        }

        private void ItemListVeiw_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.List.Validate();
            Clipboard.SetText("```\n" + InventoryListConverter.ToGoodList(in this.List) + "\n```");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EveEchoesIndustry.Services;
using EveEchoesIndustry.Skills;

namespace EveIndustry.Legacy.Tests
{
    public partial class MainServiceTest : Form
    {
        private MainService service;

        public MainServiceTest()
        {
            InitializeComponent();
            service = new MainService();
            comboBox1.Items.AddRange(service.GetRecipeTypes().ToArray());
        }

        private void MainServiceTest_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(service.GetRecipesByType((comboBox1.SelectedItem.ToString())).ToArray());
            Skill s = service.GetSkillByName(comboBox1.SelectedItem.ToString());
            if (s != null)
            {
                label1.Text = s.Name;
                label2.Text = s.ToString();
            }
            else
            {
                label1.Text = "null";
                label2.Text = "null";
            }
        }
    }
}

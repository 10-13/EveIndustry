using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EveEchoesIndustry.Items;
using EveEchoesIndustry.Recipes;
using EveEchoesIndustry.Services;
using EveEchoesIndustry.Skills;
using Newtonsoft.Json;

namespace EveIndustry.Legacy.Tests
{
    public partial class MainServiceTest : Form
    {
        private MainService service;

        private Skill ActualSkill = new Skill();
        private float Efficiency = 0;

        delegate void MA();

        private event Action SkillChanged;

        public MainServiceTest()
        {
            InitializeComponent();
            service = new MainService();
            comboBox1.Items.AddRange(service.GetRecipeTypes().ToArray());
            Instruments.Items.ItemListVeiw f = new Instruments.Items.ItemListVeiw();
            f.Show();
            SkillChanged += AccountSkill;
        }

        private void MainServiceTest_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(service.GetRecipesByType((comboBox1.SelectedItem.ToString())).ToArray());
            Skill s = service.GetSkillByName(comboBox1.SelectedItem.ToString());
            ActualSkill = s;
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
            SkillChanged.Invoke();
        }

        private void AccountSkill()
        {
            float res = ActualSkill.GetProcentageMatirialEffeciency();
            if (checkBox1.Checked)
                res -= 1;
            res = MathF.Round(res);
            Efficiency = res;
            label3.Text = res.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex > -1)
            {
                Recipe rec = service.GetRecipesByType(comboBox1.Text)[comboBox2.SelectedIndex];
                service.RecipeService.ExtendRecipe(ref rec);
                InventoryList rq = rec.Requirements;
                rq.Multiply((int)numericUpDown1.Value);
                rq.Multiply(null,Efficiency / 100);
                Clipboard.SetText(InventoryListConverter.ToIngameList(rq));
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SkillChanged.Invoke();
        }
    }
}

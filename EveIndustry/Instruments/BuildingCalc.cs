using EveEchoesIndustry.Services;
using EveEchoesIndustry.Skills;
using EveEchoesIndustry.Recipes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EveEchoesIndustry.Items;
using System.Security.Cryptography.Xml;

namespace EveIndustry.Instruments
{
    public partial class BuildingCalc : Form
    {
        private MainService service;

        private Skill skill;
        private float Efficiency;

        private Recipe recipe = new Recipe();

        private event Action CountSkillRes;

        public BuildingCalc()
        {
            InitializeComponent();
            CountSkillRes += CountSkillModifiers;
            comboBox3.Items.AddRange(typeof(Basic).GetEnumNames());
            comboBox4.Items.AddRange(typeof(Advanced).GetEnumNames());
            comboBox5.Items.AddRange(typeof(Expert).GetEnumNames());
            skill = new Skill();
            CountSkillRes.Invoke();
            service = new MainService();
            comboBox1.Items.AddRange(service.GetRecipeTypes().ToArray());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            CountSkillRes.Invoke();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Visible = checkBox1.Checked;
            CountSkillRes.Invoke();
        }



        private void LoadSkill(Skill skill)
        {
            this.skill = (Skill)skill.Clone();
            label3.Text = skill.Name;
            textBox1.Text = skill.ToString();
            comboBox3.SelectedIndex = Array.IndexOf(typeof(Basic).GetEnumValues(), skill.BasicSkill);
            comboBox4.SelectedIndex = Array.IndexOf(typeof(Advanced).GetEnumValues(), skill.AdvancedSkill);
            comboBox5.SelectedIndex = Array.IndexOf(typeof(Expert).GetEnumValues(), skill.ExpertSkill);
        }

        private void CountSkillModifiers()
        {
            float add = 0;
            if (comboBox3.SelectedIndex > -1)
                skill.BasicSkill = (Basic)typeof(Basic).GetEnumValues().GetValue(comboBox3.SelectedIndex);
            if (comboBox4.SelectedIndex > -1)
                skill.AdvancedSkill = (Advanced)typeof(Advanced).GetEnumValues().GetValue(comboBox4.SelectedIndex);
            if (comboBox5.SelectedIndex > -1)
                skill.ExpertSkill = (Expert)typeof(Expert).GetEnumValues().GetValue(comboBox5.SelectedIndex);
            if (checkBox1.Checked)
            {
                add += (float)numericUpDown1.Value;
                add += (float)numericUpDown2.Value;
                add += (float)numericUpDown3.Value;
            }
            if (checkBox2.Checked)
                add += 1;
            textBox2.Text = skill.GetProcentageMatirialEffeciency().ToString() + "%";
            label11.Text = MathF.Round(skill.GetProcentageMatirialEffeciency() - add).ToString() + "%";
            Efficiency = MathF.Round(skill.GetProcentageMatirialEffeciency() - add);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            CountSkillRes.Invoke();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            CountSkillRes.Invoke();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CountSkillRes.Invoke();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            CountSkillRes.Invoke();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            CountSkillRes.Invoke();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CountSkillRes.Invoke();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > -1)
            {
                recipe = service.GetRecipesByType(comboBox1.Text)[comboBox2.SelectedIndex].Clone() as Recipe;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CountSkillModifiers();
            InventoryList rq = recipe.Requirements.Clone() as InventoryList;
            rq.Multiply(null, Efficiency / 100);
            rq.Multiply((int)numericUpDown4.Value);
            Items.ItemListVeiw f = new Items.ItemListVeiw(rq);
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            service.RecipeService.ExtendRecipe(ref recipe);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = recipe.ProductTime.ToString();
            textBox4.Text = recipe.Count.ToString();
            textBox5.Text = recipe.Cost.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(service.GetRecipesByType(comboBox1.Text).ToArray());
            if (service.GetSkillByName(comboBox1.Text) != null)
                LoadSkill(service.GetSkillByName(comboBox1.Text));
            CountSkillModifiers();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Items.ItemListCalc f = new Items.ItemListCalc();
            f.Show();
        }
    }
}

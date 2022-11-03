using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EveEchoesIndustry.Recipes;
using Newtonsoft.Json;

namespace EveIndustry.Instruments.Recipes
{
    public partial class RecipeBuilder : Form
    {
        public RecipeBuilder()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f = JsonConvert.DeserializeObject<List<Recipe>>(richTextBox1.Text);
            foreach (Recipe r in f)
                r.Type = "Structure";
            richTextBox2.Text = JsonConvert.SerializeObject(f);
        }
    }
}

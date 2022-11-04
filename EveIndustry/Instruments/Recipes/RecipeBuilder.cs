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
            RecipeTemplate f = JsonConvert.DeserializeObject<RecipeTemplate>(richTextBox1.Text);
            if (f != null)
            {
                var d = RecipeParcer.FromCSVString(richTextBox2.Lines, f);
                foreach (var item in d)
                    item.Type = "Structure";
                Clipboard.SetText(JsonConvert.SerializeObject(d));
            }
        }
    }
}

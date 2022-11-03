using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveEchoesIndustry.Items;
using EveEchoesIndustry.Recipes;
using EveEchoesIndustry.Skills;
using Newtonsoft.Json;

namespace EveEchoesIndustry.Services
{
    public class SkillService : IEnumerable<Skill>
    {
        private Storage Storage;

        private Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();

        public List<string> DefaultSkills { get; set; }
        public List<string> DefaultCSkills { get; set; }

        public SkillService(Storage storage)
        {
            Storage = storage;
            DefaultSkills = JsonConvert.DeserializeObject<List<string>>(Storage.ReadFromFile("def.json"));
            DefaultCSkills = JsonConvert.DeserializeObject<List<string>>(Storage.ReadFromFile("defC.json"));
            if (DefaultSkills != null)
                foreach (var recipeList in DefaultSkills)
                    LoadSkills(recipeList);
            if (DefaultCSkills != null)
                foreach (var recipeList in DefaultCSkills)
                    LoadCSkills(recipeList);
            if (DefaultSkills == null)
                DefaultSkills = new List<string>();
            if (DefaultCSkills == null)
                DefaultCSkills = new List<string>();
        }

        public IEnumerable<string> GetActualFiles()
        {
            return from f in Storage.GetDirectoryInfo().EnumerateFiles()
                   where f.Name != "def.json" && f.Name != "defC.json"
                   select f.Name;
        }

        public void LoadSkills(string FileName)
        {
            List<Skill> rs = JsonConvert.DeserializeObject<List<Skill>>(Storage.ReadFromFile(FileName));
            if (rs == null)
                return;
            foreach (Skill r in rs)
            {
                if (!Skills.ContainsKey(r.Name))
                    Skills.Add(r.Name, r);
            }
        }
        public void LoadCSkills(string FileName)
        {
            List<CapitalSkill> rs = JsonConvert.DeserializeObject<List<CapitalSkill>>(Storage.ReadFromFile(FileName));
            if (rs == null)
                return;
            foreach (CapitalSkill r in rs)
            {
                if (!Skills.ContainsKey(r.Name))
                    Skills.Add(r.Name, r);
            }
        }

        public void Save()
        {
            Storage.WriteToFile("def.json", JsonConvert.SerializeObject(DefaultSkills));
            Storage.WriteToFile("defC.json", JsonConvert.SerializeObject(DefaultCSkills));
        }

        public float GetSkillMultProc(string Name)
        {
            if (Skills.ContainsKey("Name"))
                return Skills[Name].GetProcentageMatirialEffeciency();
            return 150;
        }

        public void UpdateRecipeData(ref Recipe recipe, string SkillName)
        {
            float Mult = MathF.Round(GetSkillMultProc(SkillName) / 100);
            foreach (InventoryItem item in recipe.Requirements.Items)
            {
                item.Count = (long)(Mult * item.Count);
            }
        }



        public IEnumerator<Skill> GetEnumerator()
        {
            return Skills.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Skills.Values as IEnumerable).GetEnumerator();
        }
    }
}

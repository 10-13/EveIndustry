using EveEchoesIndustry.Recipes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveEchoesIndustry.Services
{
    public class RecipeService
    {
        private Storage Storage { get; set; }

        private Dictionary<string, Recipe> Recipes = new Dictionary<string, Recipe>();

        public List<string> DefaultRecipes { get; set; }

        public RecipeService(Storage storage)
        {
            Storage = storage;
            DefaultRecipes = JsonConvert.DeserializeObject<List<string>>(Storage.ReadFromFile("def.json"));
            if (DefaultRecipes != null)
                foreach (var recipeList in DefaultRecipes)
                    LoadRecipes(recipeList);
        }

        public IEnumerable<string> GetActualFiles()
        {
            return from f in Storage.GetDirectoryInfo().EnumerateFiles()
                   select f.Name;
        }

        public void LoadRecipes(string FileName)
        {
            List<Recipe> rs = JsonConvert.DeserializeObject<List<Recipe>>(Storage.ReadFromFile(FileName));
            if (rs == null)
                return;
            foreach(Recipe r in rs)
            {
                if(!Recipes.ContainsKey(r.Product.Name))
                    Recipes.Add(r.Product.Name, r);
            }
        }

        public void Save()
        {
            Storage.WriteToFile("def.json", JsonConvert.SerializeObject(DefaultRecipes));
        }
    }
}

using EveEchoesIndustry.Items;
using EveEchoesIndustry.Recipes;
using Newtonsoft.Json;
using System;
using System.Collections;
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
            foreach (Recipe r in rs)
            {
                if (!Recipes.ContainsKey(r.Product.Name))
                    Recipes.Add(r.Product.Name, r);
            }
        }

        public bool ContainsRecipe(string Name)
        {
            return Recipes.ContainsKey(Name);
        }

        public Recipe GetRecipe(string Name)
        {
            return Recipes[Name];
        }

        public void Save()
        {
            Storage.WriteToFile("def.json", JsonConvert.SerializeObject(DefaultRecipes));
        }

        public void ExtendRecipe(ref Recipe recipe)
        {
            InventoryList toRemove = new InventoryList();
            foreach (InventoryItem item in recipe.Requirements.Items)
                if (ContainsRecipe(item.Name))
                {
                    toRemove.Items.Add(item);
                }
            while (toRemove.Items.Count > 0)
            {
                foreach (InventoryItem item in toRemove.Items)
                {
                    var buf = GetRecipe(item.Name).Requirements;
                    buf.Multiply((int)item.Count);
                    recipe.Requirements.AddList(buf);
                }
                recipe.Requirements.RemoveList(toRemove, out InventoryList other);
                if (other.Items.Count > 0)
                    throw new Exception("Removed more then exsists");
                toRemove.Items.Clear();
                foreach (InventoryItem item in recipe.Requirements.Items)
                    if (ContainsRecipe(item.Name))
                    {
                        toRemove.Items.Add(item);
                    }
            }
        }
    }
}

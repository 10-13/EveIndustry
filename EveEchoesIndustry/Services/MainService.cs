using EveEchoesIndustry.Recipes;
using EveEchoesIndustry.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveEchoesIndustry.Services
{
    public class MainService
    {
        private FileSystemService fileSystemService;

        private SkillService skillService;
        private RecipeService recipeService;

        public MainService()
        {
            fileSystemService = new FileSystemService();
            skillService = new SkillService(fileSystemService.GetOrCreateNew("Skills", "Skills"));
            recipeService = new RecipeService(fileSystemService.GetOrCreateNew("Recipes", "Recipes"));
        }

        public List<Recipe> GetRecipesByType(string Type)
        {
            return new List<Recipe>(from r in recipeService
                                    where r.Type == Type
                                    select r);
        }
        public List<string> GetRecipeTypes()
        {
            Dictionary<string, string> types = new Dictionary<string, string>();
            foreach (Recipe r in recipeService)
                if (!types.ContainsKey(r.Type))
                    types.Add(r.Type, r.Type);
            return types.Values.ToList();
        }
        public List<Skill> GetSkills()
        {
            return skillService.ToList();
        }

        public List<Recipe> GetRecipesByNamePart(string NamePart)
        {
            return (from r in recipeService where r.Product.Name.Contains(NamePart) select r).ToList();
        }

        public Recipe GetRecipeByName(string Name)
        {
            return recipeService.FirstOrDefault(new Func<Recipe, bool>((Recipe r) => { return r.Product.Name == Name; }), null);
        }

        public Skill GetSkillByName(string Name)
        {
            return skillService.FirstOrDefault(new Func<Skill, bool>((Skill r) => { return r.Name == Name; }), null);
        }
    }
}

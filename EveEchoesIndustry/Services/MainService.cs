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

        public SkillService SkillService { get; set; }
        public RecipeService RecipeService { get; set; }

        public MainService()
        {
            fileSystemService = new FileSystemService();
            SkillService = new SkillService(fileSystemService.GetOrCreateNew("Skills", "Skills"));
            RecipeService = new RecipeService(fileSystemService.GetOrCreateNew("Recipes", "Recipes"));
        }

        public List<Recipe> GetRecipesByType(string Type)
        {
            return new List<Recipe>(from r in RecipeService
                                    where r.Type == Type
                                    select r);
        }
        public List<string> GetRecipeTypes()
        {
            Dictionary<string, string> types = new Dictionary<string, string>();
            foreach (Recipe r in RecipeService)
                if (!types.ContainsKey(r.Type))
                    types.Add(r.Type, r.Type);
            return types.Values.ToList();
        }
        public List<Skill> GetSkills()
        {
            return SkillService.ToList();
        }

        public List<Recipe> GetRecipesByNamePart(string NamePart)
        {
            return (from r in RecipeService where r.Product.Name.Contains(NamePart) select r).ToList();
        }

        public Recipe GetRecipeByName(string Name)
        {

            Recipe r = new Recipe();
            
            return RecipeService.FirstOrDefault(new Func<Recipe, bool>((Recipe r) => { return r.Product.Name == Name; }), null);
        }

        public Skill GetSkillByName(string Name)
        {
            return SkillService.FirstOrDefault(new Func<Skill, bool>(
                (Skill r) => 
                { 
                    return r.Name == Name;
                }), null);
        }

        public Storage GetStorage(string Name)
        {
            if (fileSystemService.ContainsStorage(Name))
                return fileSystemService.GetOrCreateNew(Name);
            return null;
        }
    }
}

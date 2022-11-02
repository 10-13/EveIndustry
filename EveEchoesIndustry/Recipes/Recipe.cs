using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EveEchoesIndustry.Items;
using EveEchoesIndustry.Lines;
using Newtonsoft.Json;

namespace EveEchoesIndustry.Recipes
{
    public class Recipe
    {
        [JsonProperty("product")]
        public Item Product { get; set; }

        [JsonProperty("requirements")]
        public InventoryList Requirements { get; set; }

        [JsonProperty("tech")]
        public int Tech { get; set; }

        [JsonProperty("cost")]
        public int Cost { get; set; }

        [JsonProperty("productTime")]
        public int ProductTime { get; set; }

        [JsonProperty("productCount")]
        public int Count { get; set; }

        [JsonProperty("blueprintCost")]
        public int BPCost { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public Recipe(Item product = null, InventoryList requirements = null)
        {
            Product = product;
            Requirements = requirements;
            if (Requirements == null)
                Requirements = new InventoryList();
            if (Product == null)
                Product = new Item();
        }

        public Project CreateProject(DateTime startTime)
        {
            return new Project(
                processTime: new DateTime(0, 0, 0, 0, 0, ProductTime),
                startDate: startTime,
                product: Product,
                requirements: Requirements,
                cost: Cost,
                id: Product.Name
                );
        }
    }
}

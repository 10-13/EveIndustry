using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EveEchoesIndustry.Items;
using EveEchoesIndustry.Lines;
using Newtonsoft.Json;

namespace EveEchoesIndustry.Recipes
{
    public class Recipe : ICloneable
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

        public Recipe(Item product = null, InventoryList requirements = null, int tech = 0,int cost = 0,int productTime = 0,int count = 0,int bPCost = 0,string type = "")
        {
            Product = product;
            Requirements = requirements;
            if (Requirements == null)
                Requirements = new InventoryList();
            if (Product == null)
                Product = new Item();
            Tech = tech;
            Cost = Cost;
            ProductTime = productTime;
            Count = count;
            BPCost = bPCost;
            Type = type;
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

        public override string ToString()
        {
            return Product.Name;// + " " + Count.ToString() + "x";
        }

        public object Clone()
        {
            return new Recipe((Item)Product.Clone(), (InventoryList)Requirements.Clone(), Tech, Cost, ProductTime, Count, BPCost, Type);
        }

        /*public Recipe GetCopy()
        {
            Recipe copy = new Recipe();
            copy.Product = new Item(Product.Name, Product.Cost);
            copy.Cost = Cost;
            copy.Tech = Tech;
        }*/
    }
}

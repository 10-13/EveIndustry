using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveEchoesIndustry.Items
{
    public class Item : ICloneable
    {
        public static Item Empty
        {
            get
            {
                return new Item();
            }
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cost")]
        public double Cost { get; set; }

        public Item(string name = "Empty", double cost = 0)
        {
            Name = name;
            Cost = cost;
        }

        public object Clone()
        {
            return new Item(Name, Cost);
        }
    }
}

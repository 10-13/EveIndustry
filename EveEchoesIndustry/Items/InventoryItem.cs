using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveEchoesIndustry.Items
{
    public class InventoryItem
    {
        [JsonProperty("item")]
        [JsonRequired]
        public Item StoredItem { get; set; }

        [JsonProperty("count")]
        [JsonRequired]
        public long Count { get; set; }

        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonIgnore]
        public double TotalCost
        {
            get
            {
                return StoredItem.Cost * Count;
            }
        }

        [JsonIgnore]
        public string Name
        {
            get
            {
                return StoredItem.Name;
            }
            set
            {
                StoredItem.Name = value;
            }
        }

        [JsonIgnore]
        public double Cost
        {
            get
            {
                return StoredItem.Cost;
            }
            set
            {
                StoredItem.Cost = value;
            }
        }

        public InventoryItem(Item item = null,int? count = null,int? id = null)
        {
            StoredItem = item == null ? Item.Empty : item;
            Count = count.HasValue ? count.Value : 0;
            Id = id;
        }
    }
}

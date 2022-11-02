using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using EveEchoesIndustry.Items;

namespace EveEchoesIndustry.Lines
{
    public class Project
    {
        [JsonProperty("id")]
        public string Identifier { get; set; }

        [JsonProperty("startDate")]
        [JsonRequired]
        public long StartDate { get; set; }

        [JsonIgnore]
        public long Length { get; set; }

        [JsonProperty("endDate")]
        [JsonRequired]
        public long EndDate
        {
            get
            {
                return Length;
            }
            set
            {
                Length = value - StartDate;
            }
        }

        [JsonProperty("product")]
        public Item Product { get; set; }

        [JsonProperty("requirement")]
        public InventoryList Requirements { get; set; }

        [JsonProperty("cost")]
        public long StartCost { get; set; }

        [JsonIgnore]
        public DateTime StartTime
        {
            get
            {
                return new DateTime(StartDate);
            }
            set
            {
                StartDate = value.Ticks;
            }
        }

        [JsonIgnore]
        public DateTime ProcessTime
        {
            get
            {
                return new DateTime(Length);
            }
            set
            {
                Length = value.Ticks;
            }
        }

        public Project(
            string id = null,
            DateTime? startDate = null,
            DateTime? processTime = null,
            Item product = null,
            InventoryList requirements = null,
            long? cost = null)
        {
            Identifier = (id == null) ? "No name" : id;
            StartTime = startDate.HasValue ? startDate.Value : new DateTime();
            ProcessTime = processTime.HasValue ? processTime.Value : new DateTime();
            Product = product == null ? new Item() : product;
            Requirements = requirements == null ? new InventoryList() : requirements;
            StartCost = cost.HasValue ? cost.Value : 0;
        }

    }
}

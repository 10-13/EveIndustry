using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveEchoesIndustry.Items
{
    public class InventoryList
    {
        [JsonProperty("items")]
        public List<InventoryItem> Items { get; set; }

        [JsonIgnore]
        public double TotalCost
        {
            get
            {
                double res = 0;
                foreach (InventoryItem item in Items)
                    res += item.TotalCost;
                return res;
            }
        }

        public InventoryList(List<InventoryItem> items = null)
        {
            Items = items != null ? items : new List<InventoryItem>();
        }

        public void Validate()
        {
            Items.RemoveAll(new Predicate<InventoryItem>((InventoryItem item) => { return item.Count == 0; }));
        }

        public void RemoveList(in InventoryList list,out InventoryList Overabundance)
        {
            Overabundance = new InventoryList();
            foreach(InventoryItem item in list.Items)
            {
                if(Items.First(new Func<InventoryItem, bool>((InventoryItem _item) => { return _item.StoredItem.Name == item.StoredItem.Name; })) != null)
                {
                    var buf = Items.First(new Func<InventoryItem, bool>((InventoryItem _item) => { return _item.StoredItem.Name == item.StoredItem.Name; }));
                    if (buf.Count < item.Count)
                    {
                        Overabundance.Items.Add(item);
                        Overabundance.Items[Overabundance.Items.Count].Count = item.Count - buf.Count;
                        buf.Count = 0;
                    }
                    else
                    {
                        buf.Count -= item.Count;
                    }
                }
                else
                {
                    Overabundance.Items.Add(item);
                }
            }
            Validate();
        }
        public void AddList(in InventoryList list)
        {
            foreach(InventoryItem item in list.Items)
            {
                if (Items.Contains(item))
                    Items.First(new Func<InventoryItem, bool>((InventoryItem _item) => { return _item.StoredItem.Name == item.StoredItem.Name; })).Count += item.Count;
                else
                    Items.Add(item);
            }
            Validate();
        }
        public void Multiply(int Mult)
        {
            foreach(InventoryItem item in Items)
                item.Count *= Mult;
        }
    }

    public static class InventoryListConverter
    {
        public static string ToIngameRow(in InventoryItem item,int id = -1)
        {
            return (id == -1 ? (item.Id == null ? 0 : item.Id) : id).ToString() + "\t" + item.StoredItem.Name + "\t" + item.Count + "\t" + item.TotalCost;
        }
        public static string ToIngameList(in InventoryList list)
        {
            string str = "ID\tName\tCount\tCost";
            for(int i = 0;i < list.Items.Count;i++)
            {
                str += "\n" + ToIngameRow(list.Items[i]);
            }
            return str;
        }
        public static InventoryItem FromIngameRow(string str)
        {
            InventoryItem item = new InventoryItem();
            if (int.TryParse(str.Substring(0, str.IndexOf("\t")),out int r1))
                item.Id = r1;
            str = str.Substring(str.IndexOf("\t") + 1);
            item.StoredItem.Name = str.Substring(0, str.IndexOf("\t"));
            str = str.Substring(str.IndexOf("\t") + 1);
            if (int.TryParse(str.Substring(0, str.IndexOf("\t")), out int r2))
                item.Count = r2;
            str = str.Substring(str.IndexOf("\t") + 1);
            if (double.TryParse(str.Substring(0, Math.Max(str.IndexOf("\n"),str.Length)), out double r3))
                item.StoredItem.Cost = item.Count > 0 ? r3 / item.Count : 0;
            return item;
        }
        public static InventoryList FromIngameList(string str)
        {
            InventoryList res = new InventoryList();
            if (!str.EndsWith("\n"))
                str += "\n";
            int end = str.IndexOf('\n') + 1;
            str = str.Substring(end);
            while(str.Length > 0)
            {
                res.Items.Add(FromIngameRow(str.Substring(0, str.IndexOf('\n'))));
                str = str.Substring(str.IndexOf('\n') + 1);
            }
            return res;
        }
    }
}

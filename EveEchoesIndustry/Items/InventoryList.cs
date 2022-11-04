using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveEchoesIndustry.Items
{
    public class InventoryList : ICloneable
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
            for (int i = 0; i < Items.Count; i++)
                Items[i].Id = i + 1;
        }

        public void RemoveList(in InventoryList list,out InventoryList Overabundance)
        {
            Overabundance = new InventoryList();
            foreach(InventoryItem item in list.Items)
            {
                if(Items.FirstOrDefault(new Func<InventoryItem, bool>((InventoryItem _item) => { return _item.StoredItem.Name == item.StoredItem.Name; }),null) != null)
                {
                    var buf = Items.First(new Func<InventoryItem, bool>((InventoryItem _item) => { return _item.StoredItem.Name == item.StoredItem.Name; }));
                    if (buf.Count < item.Count)
                    {
                        Overabundance.Items.Add(item);
                        Overabundance.Items[Overabundance.Items.Count - 1].Count = item.Count - buf.Count;
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
                if (Items.FirstOrDefault(new Func<InventoryItem,bool>((InventoryItem it) => { return it.Name == item.Name; }),null) != null)
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
        public void Multiply(Object f,float Mult)
        {
            foreach (InventoryItem item in Items)
                item.Count = (long)MathF.Round(0.5f + Mult * (float)item.Count);
        }

        public object Clone()
        {
            List<InventoryItem> nItems = new List<InventoryItem>();
            foreach (var item in Items)
                nItems.Add((InventoryItem)item.Clone());
            return new InventoryList(nItems);
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
                str += "\n" + ToIngameRow(list.Items[i],i + 1);
            }
            return str;
        }
        public static string ToGoodRow(in InventoryItem item)
        {
            StringBuilder f = new StringBuilder();
            if (item.Name.Length > 40) 
            {
                f.Append(item.Name.Substring(0, 40));
            }
            else
            {
                f.Append(item.Name);
                f.Append(' ',40 - item.Name.Length);
            }
            f.Append(item.Count.ToString());
            f.Append(' ', 20 - item.Count.ToString().Length);
            f.Append(Math.Round(item.TotalCost).ToString());
            f.Append(' ', 30 - item.TotalCost.ToString().Length);
            return f.ToString();
        }
        public static string ToGoodList(in InventoryList list)
        {
            string res = "Name                                    Count               Cost\n=====================";
            for (int i = 0; i < list.Items.Count; i++)
            {
                res += "\n" + ToGoodRow(list.Items[i]);
            }
            res += "\n=====================";
            res += "\nTotal cost:   " + list.TotalCost.ToString();
            return res;
        }
        public static InventoryItem FromIngameRow(string str)
        {
            if (str.EndsWith('\r'))
                str = str.Substring(0, str.Length - 1);
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


/*

ID  Name    Count   Cost
1   Tritanium   39236944    78473888
2   Pyerite 10216066    40864264
3   Mexallon    3107401 55933218
4   Isogen  482030  11086690
5   Nocxium 127294  22658332
6   Zydrine 54988   50588960
7   Megacyte    40597   97432800
8   Condensed Alloy 139245  22975425
9   Precious Alloy  218997  76648950
10  Crystal Compound    261887  72018925
11  Reactive Metals 15819   4903890
12  Toxic Metals    68918   53756040
 
*/
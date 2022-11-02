using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EveEchoesIndustry.Items;

namespace EveEchoesIndustry.Recipes
{
    public static class RecipeParcer
    {
        public enum ParseTarget
        {
            ParseItem,      //0
            Ignore,         //1
            Name,           //2
            Tech,           //3
            Count,          //4
            Cost,           //5
            Time,           //6
            BlueprintCost,  //7
            Type            //8
        }
        public static List<Recipe> FromCSVString(string[] CSVLines, RecipeTemplate template)
        {
            List<List<string>> SourceData = new List<List<string>>();
            string buffer = "";
            bool longValue = false;
            foreach(string line in CSVLines)
            {
                buffer = "";
                SourceData.Add(new List<string>());
                for(int i = 0; i < line.Length; i++)
                {
                    if (longValue && line[i] == '"')
                    {
                        i++;
                        longValue = false;
                        if (i == line.Length)
                        {
                            SourceData[SourceData.Count - 1].Add(buffer);
                            break;
                        }                        
                    }
                    else if(longValue)
                    {
                        if (line[i] != ',')
                            buffer += line[i];
                        continue;
                    }
                    if (line[i] == ',')
                    {
                        SourceData[SourceData.Count - 1].Add(buffer);
                        buffer = "";
                    }
                    else if (line[i] == '"')
                        longValue = true;
                    else
                        buffer += line[i];
                }
            }

            List<Recipe> recipes = new List<Recipe>();
            for (int j = 0; j < SourceData[0].Count; j++) {
                int itemIndex = 0;
                Recipe res = new Recipe();
                for (int i = 0; i < CSVLines.Length && i < template.ParseTargets.Count; i++)
                {
                    switch (template.ParseTargets[i])
                    {
                        case ParseTarget.Name:
                            res.Product = new Item(SourceData[i][j]);
                            break;
                        case ParseTarget.Type:
                            res.Type = SourceData[i][j];
                            break;
                        case ParseTarget.Tech:
                            if (int.TryParse(SourceData[i][j], out int f1))
                                res.Tech = f1;
                            break;
                        case ParseTarget.Count:
                            if (int.TryParse(SourceData[i][j], out int f2))
                                res.Count = f2;
                            break;
                        case ParseTarget.Cost:
                            if (int.TryParse(SourceData[i][j], out int f3))
                                res.Cost = f3;
                            break;
                        case ParseTarget.Time:
                            if (int.TryParse(SourceData[i][j], out int f4))
                                res.ProductTime = f4;
                            break;
                        case ParseTarget.BlueprintCost:
                            if (int.TryParse(SourceData[i][j], out int f6))
                                res.BPCost = f6;
                            break;
                        case ParseTarget.ParseItem:
                            if (int.TryParse(SourceData[i][j], out int f5))
                            {
                                InventoryItem f = new InventoryItem(template.ItemList[itemIndex], f5, res.Requirements.Items.Count + 1);
                                res.Requirements.Items.Add(f);
                                itemIndex++;
                            }
                            break;
                        default:
                            break;
                    }
                }
                recipes.Add(res);
            }
            return recipes;
        }
    }
    public class RecipeTemplate
    {
        [JsonProperty("items")]
        [JsonRequired]
        public List<Item> ItemList { get; set; }

        [JsonProperty("fieldTypes")]
        [JsonRequired]
        public List<RecipeParcer.ParseTarget> ParseTargets { get; set; }

        public RecipeTemplate()
        {
            ItemList = new List<Item>();
            ParseTargets = new List<RecipeParcer.ParseTarget>();
        }
    }
}

﻿
using Newtonsoft.Json;

namespace EveEchoesIndustry.Skills 
{
    public enum Basic
    {
        L0 = 0,
        L1 = 6,
        L2 = 12,
        L3 = 18,
        L4 = 24,
        L5 = 30,
    }
    public enum Advanced
    {
        L0 = 0,
        L1 = 4,
        L2 = 8,
        L3 = 12,
        L4 = 16,
        L5 = 20,
    }
    public enum Expert
    {
        L0 = 0,
        L1 = 1,
        L2 = 2,
        L3 = 3,
        L4 = 4,
        L5 = 5,
    }
    public enum CBasic
    {
        L0 = 0,
        L1 = 2,
        L2 = 4,
        L3 = 6,
        L4 = 8,
        L5 = 10,
    }
    public enum CAdvanced
    {
        L0 = 0,
        L1 = 1,
        L2 = 2,
        L3 = 3,
        L4 = 4,
        L5 = 5,
    }
    public enum CExpert
    {
        L0 = 0,
        L1 = 0,
        L2 = 0,
        L3 = 0,
        L4 = 0,
        L5 = 0,
    }
    public enum CABasic
    {
        L0 = 0,
        L1 = 1,
        L2 = 2,
        L3 = 3,
        L4 = 4,
        L5 = 5,
    }
    public enum CAAdvanced
    {
        L0 = 0,
        L1 = 1,
        L2 = 2,
        L3 = 3,
        L4 = 4,
        L5 = 5,
    }
    public enum CAExpert
    {
        L0 = 0,
        L1 = 1,
        L2 = 2,
        L3 = 3,
        L4 = 4,
        L5 = 5,
    }


    public class Skill
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("basic")]
        public Basic BasicSkill { get; set; }

        [JsonProperty("advanced")]
        public Advanced AdvancedSkill { get; set; }

        [JsonProperty("expert")]
        public Expert ExpertSkill { get; set; }

        public Skill() { Name = "No name"; }
        public Skill(string Name)
        {
            this.Name = Name;
        }

        public virtual float GetProcentageMatirialEffeciency()
        {
            return MathF.Round(150f - (float)BasicSkill - (float)AdvancedSkill - (float)ExpertSkill);
        }

        public float GetEffeciency()
        {
            return GetProcentageMatirialEffeciency() / 100;
        }

        public override string ToString()
        {
            return Array.IndexOf(Enum.GetValues<Basic>(), BasicSkill).ToString() + "\\" + Array.IndexOf(Enum.GetValues<Basic>(), AdvancedSkill).ToString() + "\\" + Array.IndexOf(Enum.GetValues<Expert>(), ExpertSkill).ToString();
        }
    }

    public class CapitalSkill : Skill
    {
        [JsonProperty("c_basic")]
        public CBasic BasicCSkill { get; set; }

        [JsonProperty("c_advanced")]
        public CAdvanced AdvancedCSkill { get; set; }

        [JsonProperty("c_expert")]
        public CExpert ExpertCSkill { get; set; }

        [JsonProperty("ca_basic")]
        public CABasic BasicCASkill { get; set; }

        [JsonProperty("ca_advanced")]
        public CAAdvanced AdvancedCASkill { get; set; }

        [JsonProperty("ca_expert")]
        public CAExpert ExpertCASkill { get; set; }


        public CapitalSkill() : base() {  }
        public CapitalSkill(string Name) : base(Name)
        {
            
        }
        public CapitalSkill(Skill basic) : base(basic.Name)
        {
            BasicSkill = basic.BasicSkill;
            AdvancedSkill = basic.AdvancedSkill;
            ExpertSkill = basic.ExpertSkill;
            BasicCSkill = (CBasic)Enum.GetValues<CBasic>().GetValue(Array.IndexOf(Enum.GetValues<Basic>(), basic.BasicSkill));
            BasicCASkill = (CABasic)Enum.GetValues<CABasic>().GetValue(Array.IndexOf(Enum.GetValues<Basic>(), basic.BasicSkill));
            AdvancedCSkill = (CAdvanced)Enum.GetValues<CAdvanced>().GetValue(Array.IndexOf(Enum.GetValues<Advanced>(), basic.AdvancedSkill));
            AdvancedCASkill = (CAAdvanced)Enum.GetValues<CAAdvanced>().GetValue(Array.IndexOf(Enum.GetValues<Advanced>(), basic.AdvancedSkill));
            ExpertCSkill = (CExpert)Enum.GetValues<CExpert>().GetValue(Array.IndexOf(Enum.GetValues<Expert>(), basic.ExpertSkill));
            ExpertCASkill = (CAExpert)Enum.GetValues<CAExpert>().GetValue(Array.IndexOf(Enum.GetValues<Expert>(), basic.ExpertSkill));
        }

        public override float GetProcentageMatirialEffeciency()
        {
            return MathF.Round(base.GetProcentageMatirialEffeciency() - (float)BasicCSkill - (float)ExpertCSkill - (float)AdvancedCSkill + (float)BasicCASkill + (float)ExpertCASkill + (float)AdvancedCASkill);
        }
    }
}

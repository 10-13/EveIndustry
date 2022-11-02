using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EveEchoesIndustry.Services;

namespace EveEchoesIndustry.Skills
{
    public class SkillService
    {
        private Storage Storage;

        public Dictionary<string, Skill> DefaultSkills = new Dictionary<string, Skill>();

        public SkillService(Storage storage)
        {
            Storage = storage;
        }
    }
}

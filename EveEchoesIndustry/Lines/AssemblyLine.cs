using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveEchoesIndustry.Lines
{
    public class AssemblyLine
    {
        [JsonProperty("id")]
        public string Name { get; }

        [JsonProperty("Projects")]
        public List<Project> Projects { get; set; } = new List<Project>();

        public AssemblyLine(string Name = "No name")
        {
            this.Name = Name;
        }

        public IEnumerable<Project> GetActiveProjects(DateTime time)
        {
            return from p in Projects
                   where p.StartTime.Ticks < time.Ticks && p.StartTime.Ticks + p.ProcessTime.Ticks > time.Ticks
                   select p;
        }
        public IEnumerable<Project> GetPreparingProjects(DateTime time, DateTime? prepareTime = null)
        {
            long п = prepareTime.HasValue ? prepareTime.Value.Ticks : 0;
            long t = time.Ticks;
            return from p in Projects
                   where p.StartTime.Ticks > t && p.StartTime.Ticks + p.ProcessTime.Ticks > t
                   select p;
        }
    }
}

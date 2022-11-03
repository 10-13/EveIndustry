using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EveEchoesIndustry.Services
{
    public class FileSystemService
    {
        private Dictionary<string, Storage> storages = new Dictionary<string, Storage>();

        public string Path { get; }

        public string FullPath 
        {
            get
            {
                return Environment.CurrentDirectory + Path + "\\user";
            }
        }

        public FileSystemService(string Path = "\\data")
        {
            this.Path = Path;
            if (!File.Exists(Environment.CurrentDirectory + Path + "\\app\\storages.json")) {
                if (!Directory.Exists(Environment.CurrentDirectory + Path + "\\app"))
                    Directory.CreateDirectory(Environment.CurrentDirectory + Path + "\\app");
                File.Create(Environment.CurrentDirectory + Path + "\\app\\storages.json");
            }
            int c = 0;
            Storage[] storages = null;
            reif: if (c < 10 && storages == null)
            try
            {
                storages = JsonConvert.DeserializeObject<Storage[]>(File.ReadAllText(Environment.CurrentDirectory + Path + "\\app\\storages.json"));
            }
            catch(Exception ex)
            {
                    c++;
                    Thread.Sleep(10);
                    goto reif;
            }
            if(storages != null && storages.Length > 0)
            foreach(var s in storages)
            {
                this.storages.Add(s.Name, Storage.BuildStorage(this,s.Name,s.Path));
            }
        }

        public Storage GetOrCreateNew(string Name,string Path = null)
        {
            if (Path == null)
                Path = "\\undefined_data\\F" + storages.Count.ToString();
            if (!storages.ContainsKey(Name))
                storages.Add(Name, Storage.BuildStorage(this, Name, Path));
            return storages[Name];
        }
        public Storage CreateStorage(string Name,string Path)
        {
            storages.Add(Name, Storage.BuildStorage(this, Name, Path));
            return storages[Name];
        }
        public bool ContainsStorage(string Name) => storages.ContainsKey(Name);
        public bool IsPathTaken(string Path)
        {
            foreach (var s in this.storages)
                if (s.Value.Path == Path)
                    return true;
            return false;
        }

        public void Save()
        {
            File.WriteAllText(Environment.CurrentDirectory + Path + "\\app\\storages.json", JsonConvert.SerializeObject(storages.Values.ToArray()));
            
        }
        public void CreateDump()
        {
            Save();
            if (File.Exists("AppData.zip"))
                File.Delete("AppData.zip");
            if (File.Exists("UserData.zip"))
                File.Delete("UserData.zip");
            ZipFile.CreateFromDirectory(Environment.CurrentDirectory + Path + "\\app", "AppData.zip");
            ZipFile.CreateFromDirectory(Environment.CurrentDirectory + Path + "\\user", "UserData.zip");
        }
    }

    public class Storage
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("id")]
        public string Name { get; set; }

        [JsonIgnore]
        private FileSystemService parentService = null;

        private Storage()
        {
            Path = null;
            Name = "Empty owner";
        }

        public static Storage BuildStorage(in FileSystemService service,string Name,string Path)
        {
            Storage res = new Storage();
            res.parentService = service;
            res.Name = Name;
            res.Path = Path;
            if(!Directory.Exists(res.BuildPath()))
                Directory.CreateDirectory(res.BuildPath());
            return res;
        }

        public string ReadFromFile(string FileName)
        {
            if (!File.Exists(BuildPath() + "\\" + FileName))
                File.Create(BuildPath() + "\\" + FileName);
            retry: try
            {
                return File.ReadAllText(BuildPath() + "\\" + FileName);
            }
            catch(Exception ex)
            {
                Thread.Sleep(10);
            }
            goto retry;
        }
        public void WriteToFile(string FileName,string Text)
        {
            File.AppendAllLines(BuildPath() + "\\" + FileName,new List<string>{ Text });
        }

        public DirectoryInfo GetDirectoryInfo()
        {
            return new DirectoryInfo(BuildPath());
        }

        private string BuildPath()
        {
            return parentService.FullPath + "\\" + Path;
        }
    }
}

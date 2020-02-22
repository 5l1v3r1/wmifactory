using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using wmibot.Models;
using wmibot.Models.Cache;

namespace wmibot.Factories
{
    public class WmiObjectFactory
    {
        private List<WmiObjectCacheEntry> Cache = new List<WmiObjectCacheEntry>();
        private JsonSerializer Serializer = new JsonSerializer();
        private string FileName;

        public WmiObjectFactory(string cacheFileName)
        {
            var localFileName = Path.GetFullPath(cacheFileName);
            FileName = localFileName;
            LoadCache();
        }

        private void LoadCache()
        {
            if (!string.IsNullOrEmpty(FileName) && File.Exists(FileName))
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        using (JsonReader jsonReader = new JsonTextReader(reader))
                        {
                            List<WmiObjectCacheEntry> cache = Serializer.Deserialize<List<WmiObjectCacheEntry>>(jsonReader);
                            Cache = cache;
                        }
                    }
                }
            }
        }

        public void SaveCache()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    fs.Seek(0L, SeekOrigin.Begin);
                    using (StreamWriter writer =new StreamWriter(fs ))
                    {
                        using  (JsonTextWriter jsonWriter =new JsonTextWriter(writer))
                        {
                            Serializer.Serialize(jsonWriter, Cache);
                        }
                    }
                   
                
                }
            }
        }


        public void AddCache(string rootName, string key, string value)
        {
            WmiObjectCacheEntry entry;
            if (Cache.Any(e => e.RootName == rootName))
            {
                entry = Cache.First(e => e.RootName == rootName);

            }
            else
            {
                entry = new WmiObjectCacheEntry(rootName);
                Cache.Add(entry);
            }
            entry.Values.Add(key, value);
        }

        public T Create<T>()
        {
            T instance = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] props = instance.GetType().GetProperties().Where(i => i.GetCustomAttribute<WmiObject>() != null).ToArray();
            foreach (PropertyInfo prop in props)
            {
                WmiObject accessory = prop.GetCustomAttribute<WmiObject>();
                // cache features
                if(accessory.Cache )
                {
                    if (Cache.Any(e => e.RootName == accessory.RootName))
                    {
                        WmiObjectCacheEntry entry = Cache.First(e => e.RootName == accessory.RootName);
                        if (entry.Values.Any(v => v.Key == accessory.KeyName))
                        {
                            prop.SetValue(instance, (string)entry.Values[accessory.KeyName]);
                            continue;
                        }
                    }
                }

                // end cache features
                if (!Equals(accessory, null))
                {
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From " + accessory.RootName))
                    {
                        foreach (var o in searcher.Get())
                        {
                            object wmi_value = o[accessory.KeyName];
                            if (!Equals(wmi_value, null))
                            {
                                prop.SetValue(instance, (string)wmi_value);
                                AddCache(accessory.RootName, accessory.KeyName, (string)wmi_value);
                                SaveCache();
                            }
                        }
                    }
                }
            }
            return instance;
        }
    }
}

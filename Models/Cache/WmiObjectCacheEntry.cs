using System;
using System.Collections.Generic;

namespace wmibot.Models.Cache
{
    [Serializable]
    public class WmiObjectCacheEntry
    {
        public string RootName;

        public Dictionary<string, string> Values = new Dictionary<string, string>();

        public WmiObjectCacheEntry(string rootName )
        {
            RootName = rootName;
        }
        public WmiObjectCacheEntry()
        {

        }

        public static bool operator ==(WmiObjectCacheEntry entry, string value)
        {
            return string.Equals(entry.RootName, value);
        }
        public static bool operator !=(WmiObjectCacheEntry entry, string value)
        {
            return !string.Equals(entry.RootName, value);
        }
        public override bool Equals(object obj)
        {
            return RootName.Equals(obj);
        }

        public override int GetHashCode()
        {
            return RootName.GetHashCode();
        }

        public override string ToString()
        {
            return RootName.ToString();
        }
    }
}

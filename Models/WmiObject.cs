using System;

namespace wmibot.Models
{
    public class WmiObject : Attribute
    {
        public string RootName { get; set; }
        public string KeyName { get; set; }
        public bool Cache { get; set; }
       
        public WmiObject(string rootName, string keyName, bool cache = true)
        {
            RootName = rootName;
            KeyName = keyName;
            Cache = cache;
        }
    }
}

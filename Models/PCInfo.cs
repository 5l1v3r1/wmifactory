

namespace wmibot.Models
{
    public class PCInfo
    {
        [WmiObject("Win32_Processor", "Name", true)]
        public string ProcessorName { get; set; }
        [WmiObject("Win32_BIOS", "Manufacturer")]
        public string BIOSManufacturer { get; set; }
        [WmiObject("Win32_ComputerSystem", "SystemType")]
        public string Architecture { get; set; }
    }
}

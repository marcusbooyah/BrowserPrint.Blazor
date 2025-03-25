using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserPrint.Models
{
    public class Device
    {
        public string Name { get; set; }
        public string DeviceType { get; set; }
        public string Connection { get; set; }
        public string Uid { get; set; }
        public int Version { get; set; }
        public string Provider { get; set; }
        public string Manufacturer { get; set; }
        public int ReadRetries { get; set; }
    }
}

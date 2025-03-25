using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserPrint.Models
{
    public class PrinterStatus
    {
        public bool IsReady { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

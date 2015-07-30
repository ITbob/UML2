using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class MessageEvent : EventArgs
    {
        public String type { get; set; }
        public String value { get; set; }
    }
}

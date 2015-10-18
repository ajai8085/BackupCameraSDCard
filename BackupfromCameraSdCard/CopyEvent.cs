using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupfromCameraSdCard
{
    public class CopyEvent
    {
        public string Source { get; set; }
        public string  Dest { get; set; }
        public bool Copied { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupfromCameraSdCard
{
    public class CopyInfo
    {
        public List<string> Files { get; set; }
        public string DestinationLocation { get; set; }

        public CopyInfo(List<string> files,string destinationLocation)
        {
            this.Files = files;
            this.DestinationLocation = destinationLocation;
        }
    }

}

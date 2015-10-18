using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupfromCameraSdCard
{
    public interface IHandleEvent
    {
        void Notify(CopyEvent info);
    }

   
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupfromCameraSdCard
{
    public class FilesCopier
    {
        readonly CopyInfo _copyInfo;
        public FilesCopier(CopyInfo copyInfo)
        {
            _copyInfo = copyInfo;
        }
        public void CopyFiles()
        {
            try
            {
                if (!Directory.Exists(_copyInfo.DestinationLocation))
                    Directory.CreateDirectory(_copyInfo.DestinationLocation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            _copyInfo.Files.ForEach(x =>
            {
                string destFileName = "";
                bool success = true;
                try
                {
                    destFileName = Path.Combine(_copyInfo.DestinationLocation, Path.GetFileName(x));
                    if (!File.Exists(destFileName))
                    {
                        File.Copy(x, destFileName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    success = false;
                }

                Broadcaster.GetObject().NotifyRegistered(new CopyEvent { Dest = destFileName, Source = x, Copied = success });

            });
        }
    }
}

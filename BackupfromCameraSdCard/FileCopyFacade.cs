using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupfromCameraSdCard
{
    public class FileCopyFacade
    {
        readonly SourceFilesBuilder _sourceFileBuilder;
        readonly List<DestinationLocationBuilder> _destinationLocations;

        public int FilesCount()
        {
            int count = 0;
            if (_sourceFileBuilder != null)
                count = _sourceFileBuilder.GetTotalFiles();
            if(_destinationLocations!=null)
            {
                count *= _destinationLocations.Count;
            }
            return count;
        }

        public FileCopyFacade(SourceFilesBuilder sourceFileBuilder, List<DestinationLocationBuilder> destinations)
        {
            _sourceFileBuilder = sourceFileBuilder;
            _destinationLocations = destinations;
        }


        public void CopyAll()
        {
            _destinationLocations.AsParallel().ForAll(x =>
            {
                var picture = new CopyInfo(_sourceFileBuilder.PictureFiles, x.PictureLocation);
                var video = new CopyInfo(_sourceFileBuilder.VideoFiles, x.VideoLocation);

                var t1 = Task.Run(() => { new FilesCopier(picture).CopyFiles(); });
                var t2 = Task.Run(() => { new FilesCopier(video).CopyFiles(); });
                Task.WaitAll(t1, t2);
            });

        }
    }
}

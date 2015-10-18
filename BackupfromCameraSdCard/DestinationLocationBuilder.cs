using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupfromCameraSdCard
{
    public class DestinationLocationBuilder
    {
        private readonly string _destination;

        private string _videoLocation;
        private string _pictureLocation;
        private string _folderName;

        public DestinationLocationBuilder(string destination,string folderName)
        {
            _destination = destination;
            _folderName = folderName;

            BuildDestination();
        }

        private void BuildDestination()
        {
           
            _videoLocation = Path.Combine(_destination, "Video", _folderName);
            _pictureLocation = Path.Combine(_destination, "Pictures", _folderName);

        }

        public string VideoLocation
        {
            get { BuildDestination(); return _videoLocation; }
        }

        public string PictureLocation
        {
            get { BuildDestination(); return _pictureLocation; }
        }

    }
}
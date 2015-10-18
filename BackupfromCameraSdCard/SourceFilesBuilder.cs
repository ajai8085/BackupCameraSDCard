using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupfromCameraSdCard
{
    public class SourceFilesBuilder
    {
        readonly string _source;
        List<string> _videoFiles;
        List<string> _pictureFiles;

        public int GetTotalFiles()
        {
            int count = 0;
            if (_videoFiles != null)
                count += _videoFiles.Count;
            if (_pictureFiles != null)
                count += _pictureFiles.Count;

            return count;
        }

        public SourceFilesBuilder(string source)
        {
            _source = source;
            Build();
        }

        private void Build()
        {
            _pictureFiles = Directory.GetFiles(_source, "*.JPG", SearchOption.AllDirectories).ToList();
            _videoFiles = Directory.GetFiles(_source, "*.AVI", SearchOption.AllDirectories).ToList();
        }

        public List<string> VideoFiles
        {
            get { return _videoFiles; }
        }

        public List<string> PictureFiles
        {
            get { return _pictureFiles; }
        }
    }
}
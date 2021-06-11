using MPS.Common.DTOs.FileManger;
using System.Collections.Generic;

namespace MPS.Common.ViewModels.FileManager
{
    public class FileViewModel
    {
        public string Path { get; set; }
        public string DestDirPath { get; set; }
        public string Name { get; set; }
        public IEnumerable<Folder> Folders { get; set; }
        public string GetWebRootPath { get; set; }
    }
}

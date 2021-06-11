using Moba.Common.DTOs.FileManger;
using System.Collections.Generic;

namespace Moba.Common.ViewModels.FileManager
{
   public class FolderViewModel
    {
        public string Path { get; set; }
        public string DestDirPath { get; set; }
        public string Name { get; set; }
        public IEnumerable<Folder> Folders { get; set; }
        public string GetWebRootPath { get; set; }
        public string Recursive { get; set; }
    }
}

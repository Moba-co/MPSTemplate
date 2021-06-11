using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Moba.Common.DTOs.FileManger
{
    public class Folder
    {
        public Folder()
        {
            Folders = new List<Folder>();
            Files = new List<FileInfo>();
        }
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Path { get; set; }
        public List<Folder> Folders { get; set; }
        public List<FileInfo> Files { get; set; }

        public string GetFolderPath => Path + "/" + Name;
    }
}
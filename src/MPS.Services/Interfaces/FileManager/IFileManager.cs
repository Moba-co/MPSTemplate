using Moba.Common.DTOs.FileManger;
using Moba.Common.DTOs;
using Moba.Common.ViewModels.FileManager;
using System.Threading.Tasks;
using Moba.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Moba.Services.Interfaces.FileManager
{
    public interface IFileManager
    {
        Folder GetAllDirectories();
        Folder GetDirectory(string path = null);
        ReturnMessageDto AddFolder(string path, string name);
        ReturnMessageDto DeleteFile(string path);
        ReturnMessageDto DeleteFolder(string path, bool recursive);
        ReturnMessageDto MoveFolder(FolderViewModel model);
        ReturnMessageDto RenameFolder(string path, string name);
        ReturnMessageDto RenameFile(string path, string name, string previousName);
        ReturnMessageDto MoveFile(FileViewModel model);
        string GetPreviousPath(string path);
        string GetNameType(string name);
        string GetWebRootPath { get; }
        Task<string> UploadImage(IFormFile image, ImageTypeEnum type, string name);
        bool DeleteImage(string imageName, ImageTypeEnum type, string name);
    }
}
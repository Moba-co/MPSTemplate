using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moba.Common.DTOs;
using Moba.Common.DTOs.FileManger;
using Moba.Common.ViewModels.FileManager;
using Moba.Domain.Enums;
using Moba.Services.Interfaces.FileManager;

namespace Moba.Services.Services
{
    public class FileManager : IFileManager
    {
        private readonly string _webRootPath;

        public FileManager(IWebHostEnvironment environment)
        {
            _webRootPath = environment.WebRootPath;
        }

        /// <summary>
        /// will a specific folder to this path with this certain name .  if path be null . it will make it in web root path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ReturnMessageDto AddFolder(string name, string path = null)
        {
            try
            {
                path ??= _webRootPath;
                var uploadPath = Path.Combine(path + "/" + name);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                return new ReturnMessageDto($"با موفقیت {name} ساخته شد", true, 0);
            }
            catch (Exception e)
            {
                return new ReturnMessageDto(e.Message, false, 0);
            }
        }


        /// <summary>
        /// delete file if exits in this path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ReturnMessageDto DeleteFile(string path)
        {
            try
            {
                if (!File.Exists(path)) return new ReturnMessageDto("فایل مورد نظر وجود ندارد", false, 0);
                File.Delete(path);
                return new ReturnMessageDto("فایل مورد نظر با موفقت حذف شد", true, 0);

            }
            catch (Exception e)
            {
                return new ReturnMessageDto(e.Message, false, 0);
            }
        }

        /// <summary>
        /// delte folder if exist in this path 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public ReturnMessageDto DeleteFolder(string path, bool recursive)
        {
            try
            {
                if (!Directory.Exists(path)) return new ReturnMessageDto("پوشه مورد نظر وجود ندارد", false, 0);
                Directory.Delete(path, recursive);
                return new ReturnMessageDto("پوشه مورد نظر با موفقت حذف شد", true, 0);

            }
            catch (Exception e)
            {
                return new ReturnMessageDto(e.Message, false, 0);
            }
        }

        /// <summary>
        /// will return a folder with its subfolders and files :D
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Folder GetDirectory(string path = null)
        {
            path ??= _webRootPath;
            // if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            // {
            //     path = path == _webRootPath ? _webRootPath : path.Replace(_webRootPath,"");
            // }
            var res = new Folder();
            foreach (var file in Directory.GetFiles(path))
            {
                res.Files.Add(new FileInfo(file));
            }
            foreach (var folder in Directory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    res.Folders.Add(new Folder()
                    {
                        Name = folder.Split("\\").ToList().LastOrDefault(),
                        Path = path
                    });
                }
                else
                {
                    res.Folders.Add(new Folder()
                    {
                        Name = folder.Split("/").ToList().LastOrDefault(),
                        Path = path
                    });
                }
            }
            return res;
        }

        public string GetPreviousPath(string path)
        {
            var asList = path.Split("/").ToList();
            asList.Remove(asList.LastOrDefault());
            return string.Join(
                "/",
                asList
            );
        }
        public string GetNameType(string name)
        {
            var asList = name.Split(".").ToList();
            return string.Join(
                ".",
                asList[1]
                );
        }
        public ReturnMessageDto MoveFolder(FolderViewModel model)
        {
            try
            {
                if (!Directory.Exists(model.DestDirPath) || !Directory.Exists(model.Path)) return new ReturnMessageDto("پوشه مورد نظر وجود ندارد", false, 0);
                DirectoryInfo dir = new DirectoryInfo(model.Path);
                dir.MoveTo(model.DestDirPath + "\\" + model.Name);
                return new ReturnMessageDto("پوشه مورد نظر با موفقت جا به جا شد", true, 0);
            }
            catch (Exception e)
            {
                return new ReturnMessageDto(e.Message, false, 0);
            }
        }
        public ReturnMessageDto MoveFile(FileViewModel model)
        {
            try
            {
                if (!Directory.Exists(model.DestDirPath) || !File.Exists(model.Path)) return new ReturnMessageDto("فایل مورد نظر وجود ندارد", false, 0);
                FileInfo file = new FileInfo(model.Path);
                file.MoveTo(model.DestDirPath + "\\" + model.Name);
                return new ReturnMessageDto("فایل مورد نظر با موفقت جا به جا شد", true, 0);
            }
            catch (Exception e)
            {
                return new ReturnMessageDto(e.Message, false, 0);
            }
        }
        public Folder GetAllDirectories()
        {
            var res = new Folder();
            foreach (var file in Directory.GetFiles(_webRootPath, "*.*", SearchOption.AllDirectories))
            {
                res.Files.Add(new FileInfo(file));
            }
            foreach (var folder in Directory.GetDirectories(_webRootPath, "*.*", SearchOption.AllDirectories))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    res.Folders.Add(new Folder()
                    {
                        Name = folder.Split("\\").ToList().LastOrDefault(),
                        Path = folder
                    });
                }
                else
                {
                    res.Folders.Add(new Folder()
                    {
                        Name = folder.Split("\\").ToList().LastOrDefault(),
                        Path = folder
                    });
                }
            }
            return res;
        }

        public ReturnMessageDto RenameFolder(string path, string name)
        {
            try
            {
                var previousPath = GetPreviousPath(path);
                if (!Directory.Exists(path)) return new ReturnMessageDto("فایل مورد نظر وجود ندارد", false, 0);
                Directory.Move(path, previousPath + "\\" + name);
                return new ReturnMessageDto("فایل مورد نظر با موفقت جا به جا شد", true, 0);
            }
            catch (Exception e)
            {
                return new ReturnMessageDto(e.Message, false, 0);
            }
        }

        public ReturnMessageDto RenameFile(string path, string name, string previousName)
        {
            try
            {
                var nameType = GetNameType(previousName);
                if (!Directory.Exists(path)) return new ReturnMessageDto("پوشه مورد نظر وجود ندارد", false, 0);
                File.Move(path + "/" + previousName, path + "\\" + name + "." + nameType);
                return new ReturnMessageDto("فایل مورد نظر با موفقت جا به جا شد", true, 0);
            }
            catch (Exception e)
            {
                return new ReturnMessageDto(e.Message, false, 0);
            }
        }
        public async Task<string> UploadImage(IFormFile image, ImageTypeEnum typeName, string name)
        {
            var uploadPath = Path.Combine(_webRootPath +  "\\Images\\" + typeName + "\\" + name);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            if (image.ContentType == "image/jpeg" || image.ContentType == "image/svg+xml" || image.ContentType == "image/png")
            {
                var filePath = Path.Combine(uploadPath, image.FileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream).ConfigureAwait(false);
            }
            else
            {
                throw new ArgumentException("لطفا عکس را بارگذاری کنید");
            }

            return image.FileName;
        }

        public bool DeleteImage(string imageName, ImageTypeEnum typeName, string name)
        {
            var image = Path.Combine(_webRootPath + "\\Images\\" + typeName + "\\" + name, imageName);
            if (!File.Exists(image)) return false;
            File.Delete(image);
            return true;

        }
        public string GetWebRootPath => _webRootPath;
    }
}
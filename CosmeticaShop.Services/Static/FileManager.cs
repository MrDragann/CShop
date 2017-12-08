using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CosmeticaShop.IServices.Enums;

namespace CosmeticaShop.Services.Static
{
    public static class FileManager
    {
        #region [ FileManagerSettings ]

        private static string _rootDirectory = "/Files";
        public static string RootDirectory => _rootDirectory;
        public static string PreviewName { get; } = "preview";
        public static string DefaultImage { get; } = "/Files/defaultImage.png";

        #endregion

        /// <summary>
        /// Вернуть путь
        /// </summary>
        /// <param name="directoryType"></param>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        private static string GetPath(EnumDirectoryType directoryType, string subFolder = null)
        {
            var path = _rootDirectory + "/" + directoryType + "/";
            if (!string.IsNullOrWhiteSpace(subFolder))
                path += subFolder + "/";
            return path;
        }

        /// <summary>
        /// Получить изображения для продукта
        /// </summary>
        /// <param name="directoryType"></param>
        /// <param name="id">ИД продукта</param>
        /// <returns></returns>
        public static List<string> GetAllImages(EnumDirectoryType directoryType, int id)
        {
            var path = GetPath(directoryType) + id + "/";
            var directory = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(directory))
                return null;
            var dirInfo = new DirectoryInfo(directory);
            return dirInfo.EnumerateFiles().Select(_ => path + _.Name).ToList();
        }
        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="directoryType">Тип директории</param>
        /// <param name="subFolder">Подпапка</param>
        /// <param name="fileName">Название файла (без расширения)</param>
        /// <returns></returns>
        public static bool DeleteFile(EnumDirectoryType directoryType, string subFolder = null, string fileName = null)
        {
            var path = GetPath(directoryType, subFolder);
            var directory = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(directory))
                return false;
            var fileDirectory = HttpContext.Current.Server.MapPath(path + "/" + fileName);
            var info = new DirectoryInfo(directory);
            var files = info.GetFiles();
            foreach (var file in files)
            {
                if (Path.GetFileNameWithoutExtension(file.Name) == fileName
                    || file.Name == fileName || file.Name == Path.GetFileName(fileName))
                {
                    try
                    {
                        File.Delete(file.FullName);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Получить файлы из директории
        /// </summary>
        /// <param name="directoryType">Тип директории</param>
        /// <param name="subFolder">Подпапка</param>
        /// <returns></returns>
        public static List<string> GetFileUrls(EnumDirectoryType directoryType, string subFolder = null, bool ignorePreview = true)
        {
            var path = GetPath(directoryType, subFolder);
            var directory = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(directory))
                return null;
            var info = new DirectoryInfo(directory);
            var files = info.GetFiles().Select(x => path + x.Name).ToList();
            if (ignorePreview)
                files = files.Where(x => Path.GetFileNameWithoutExtension(x) != PreviewName).ToList();
            return files;
        }
        /// <summary>
        /// Удалить директорию
        /// </summary>
        /// <param name="directoryType">Тип директории</param>
        /// <param name="subFolder">Подпапка</param>
        /// <returns></returns>
        public static bool DeleteDirectory(EnumDirectoryType directoryType, string subFolder = null)
        {
            var path = GetPath(directoryType, subFolder);
            var directory = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(directory))
                return false;
            try
            {
                Directory.Delete(directory, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Сохранить файл
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="directoryType">Тип директории</param>
        /// <param name="name">Имя файла (без расширения)</param>
        /// <param name="subFolder">Подпапка</param>
        /// <returns></returns>
        public static string SaveFile(HttpPostedFileBase file, EnumDirectoryType directoryType, string name = null,
            string subFolder = null)
        {
            var path = GetPath(directoryType, subFolder);
            var directory = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var fileName = "";
            if (string.IsNullOrWhiteSpace(name))
                fileName = file.FileName;
            else fileName = name + Path.GetExtension(file.FileName);

            try
            {
                file.SaveAs(Path.Combine(directory, fileName));
                return path + fileName;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        /// <summary>
        /// Сохранить изображение во временную директорию
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="subFolder">Подпапка</param>
        /// <param name="directoryType">Тип директории</param>
        /// <param name="name">Имя изображения (без расширения)</param>
        /// <param name="isRemoveExist">Удалить существующее если названия совпадают</param>
        /// <returns></returns>
        public static string SaveImage(HttpPostedFileBase file, EnumDirectoryType directoryType, string name = null,
            string subFolder = null, bool isRemoveExist = true)
        {
            var path = RootDirectory + "/" + directoryType.ToString() + "/";
            if (!string.IsNullOrWhiteSpace(subFolder))
                path += subFolder + "/";
            var directory = HttpContext.Current.Server.MapPath(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var fullName = name + Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(name))
                fullName = file.FileName;
            if (isRemoveExist)
            {
                var fileName = (string.IsNullOrEmpty(name)) ? file.FileName : name;
                DeleteFile(directoryType, fileName: fileName);
            }
            var serverFileName = Path.Combine(directory, fullName);
            file.SaveAs(serverFileName);
            return path + fullName;
        }
    }
}

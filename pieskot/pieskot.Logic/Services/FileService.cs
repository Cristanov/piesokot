using System;
using System.IO;

namespace NaSpacerDo.Logic
{
    public class FileService : IFileService
    {
        // TODO: wyrzucić tworzenie ściężki z tej klasy?
        private readonly string rootPath;
        private readonly string uploadFolderPath;

        /// <summary>
        /// Klasa do operacji na plikach
        /// </summary>
        /// <param name="rootPath">Fizyczna ścieżka do katalogu głównego</param>
        /// <param name="uploadPath">Relatywna ścieżka do folderu plików względem katalogu głównego</param>
        public FileService(string rootPath, string uploadPath)
        {
            this.rootPath = rootPath;
            uploadFolderPath = uploadPath;
        }

        public string CreateCompanyDirectory(string name)
        {
            string physicalPath = Path.Combine(rootPath, uploadFolderPath, name);
            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath));

            return physicalPath;
        }

        public void Delete(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public void DeleteFile(string path)
        {
            string physicalPath = GetPhysicalPath(path);

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
        }
        
        public long GetSize(string path)
        {
            string physicalPath = GetPhysicalPath(path);

            FileInfo fileInfo = new FileInfo(physicalPath);

            return fileInfo.Length;
        }

        public string Save(string fileName, string companyName, byte[] content)
        {
            string uniqeFilename = GetUniqeFileName(fileName);
            string physicalPath = Path.Combine(rootPath, uploadFolderPath, companyName, uniqeFilename);

            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath));

            using (MemoryStream stream = new MemoryStream(content))
            {
                using (FileStream fs = new FileStream(physicalPath, FileMode.Create))
                {
                    fs.Write(content, 0, content.Length);
                }
            }
            return CreateRelativeFilePath(companyName, uniqeFilename);
        }

        private string CreateRelativeFilePath(string companyName, string uniqeFilename)
        {
            return Path.Combine("/", uploadFolderPath, companyName, uniqeFilename).Replace('\\', '/');
        }

        private string GetUniqeFileName(string fileName)
        {
            return string.Format("{0}_{1}", Guid.NewGuid().ToString(), fileName);
        }

        public string GetPhysicalPath(string relativefilePath)
        {
            return Path.Combine(rootPath, relativefilePath.TrimStart('/'));
        }
    }
}

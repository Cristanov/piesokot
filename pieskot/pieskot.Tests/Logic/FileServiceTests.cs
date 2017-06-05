using NaSpacerDo.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaSpacerDo.Tests.Logic
{
    [TestFixture]
    public class FileServiceTests
    {
        private const string TEMPFOLDERNAME = "tempFolder";
        private string ROOTPATH = AppDomain.CurrentDomain.BaseDirectory;

        private IFileService fileService;

        [SetUp]
        public void SetUp()
        {
            fileService = new FileService(ROOTPATH, TEMPFOLDERNAME);
        }

        [TearDown]
        public void SetDown()
        {
            string path = Path.Combine(ROOTPATH, TEMPFOLDERNAME);
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        [Test]
        public void FileService_ReturnsValidFileSize()
        {
            string path = fileService.Save("test.jpg", "testCompany", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            string physicalPath = Path.Combine(ROOTPATH, path.TrimStart('/'));
            long size = fileService.GetSize(physicalPath);

            long expectedSize = new FileInfo(physicalPath).Length;

            Assert.AreEqual(expectedSize, size);
        }

        [Test]
        public void FileService_ReturnsValidPhysicalPath()
        {
            string relativefilePath = "someCompany/someFile.txt";

            string physicalPath = fileService.GetPhysicalPath(relativefilePath);

            string expectedPath = Path.Combine(ROOTPATH, relativefilePath);

            Assert.AreEqual(expectedPath, physicalPath);
        }

        [Test]
        public void FileService_CanDeleteFile()
        {
            string path = fileService.Save("test.jpg", "testCompany", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            string physicalPath = Path.Combine(ROOTPATH, path.TrimStart('/'));

            Assert.IsTrue(File.Exists(physicalPath));

            fileService.DeleteFile(physicalPath);

            Assert.IsFalse(File.Exists(physicalPath));
        }
    }
}

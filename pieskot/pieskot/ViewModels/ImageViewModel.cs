using NaSpacerDo.Attributes;
using NaSpacerDo.LocalResource;
using System.IO;
using System.Web;

namespace NaSpacerDo.ViewModels
{
    public class ImageViewModel
    {
        public int Id { get; set; }

        [FileExtensions(".png;.jpg;.jpeg", ErrorMessageResourceName = "InvalidImageExtensionErrorMessage", ErrorMessageResourceType = typeof(Resource))]
        public string FileName { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public string Path { get; set; }

        public bool IsLogo { get; set; }

        /// <summary>
        /// Konwertuje plik na tablicę bajtów
        /// </summary>
        /// <param name="file">plik</param>
        /// <returns>Tablica bajtów</returns>
        public static byte[] FileToByteArray(HttpPostedFileBase file)
        {
            if (file == null || file.InputStream.Length == 0)
            {
                return null;
            }

            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                return binaryReader.ReadBytes((int)file.InputStream.Length);
            }
        }
    }

    public class DropzoneImageViewModel: ImageViewModel
    {
        public long Size { get; set; }
    }
}
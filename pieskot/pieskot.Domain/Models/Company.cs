using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace NaSpacerDo.Domain.Models
{
    public class Company
    {
        public Company()
        {
            Images = new List<Image>();
            Ratings = new List<Rate>();
        }

        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Www { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        /// <summary>
        /// Data utworzenia. UTC
        /// </summary>
        public DateTime CreationDate { get; set; }

        public virtual Address Address { get; set; }

        [NotMapped]
        public Image Logo { get { return Images?.FirstOrDefault(x => x.IsLogo); } }

        public virtual ICollection<Image> Images { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Rate> Ratings { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public string OwnerId { get; set; }

        /// <summary>
        /// Konwertuje stream na tablicę bajtów i wpisuje do Logo w bieżącym obiekcie Company
        /// </summary>
        /// <param name="fileName">Nazwa pliku</param>
        /// <param name="inputStream">stream</param>
        public void SetLogo(string fileName, Stream inputStream)
        {
            Logo.FileName = fileName;
            Logo.Title = Path.GetFileNameWithoutExtension(fileName);

            using (var binaryReader = new BinaryReader(inputStream))
            {
                Logo.Content = binaryReader.ReadBytes((int)inputStream.Length);
            }
        }
    }
}

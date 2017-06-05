using System.Collections.Generic;
using NaSpacerDo.Domain.Models;

namespace NaSpacerDo.Logic
{
    public interface ICompanyBusinessProcess
    {
        Company AddCompany(string userId, Company company);

        ICollection<Company> GetCompanies();

        ICollection<Company> GetCompanies(CompanySearchCriteria searchCriteria);

        Company GetCompany(long id);

        Company GetCompany(string name);

        Company UpdateCompany(string userId, Company company);

        /// <summary>
        /// Sprawdza czy istnieje w bazie danych podany obiekt. Sprawdza tylko po nazwie, ale tylko wtedy kiedy obiekt w bazie ma inne ID.
        /// </summary>
        /// <param name="company">Obiekt</param>
        /// <returns>true / false</returns>
        bool IsExist(Company company);

        /// <summary>
        /// Dodaje obraz do galerii danej firmy
        /// </summary>
        /// <param name="companyId">Identyfikator firmy</param>
        /// <param name="image">Obraz</param>
        Image AddImage(int companyId, Image image);

        /// <summary>
        /// Zwraca kolekcje zdjęć dla obiektu o podanym identyfikatorze
        /// </summary>
        /// <param name="companyId">Identyfikator obiektu</param>
        /// <returns>Kolekcja zdjęć</returns>
        ICollection<Image> GetImages(int companyId);

        /// <summary>
        /// Usuwa zdjęcie
        /// </summary>
        /// <param name="id">Identyfikator zdjęcia</param>
        void DeleteImage(int id);

        /// <summary>
        /// Dodaje ocene obiektu
        /// </summary>
        /// <param name="companyId">Identyfikator obiektu</param>
        /// <param name="rate">Ocena</param>
        /// <returns>Para wartości <id oceny, ocena średnia></returns>
        KeyValuePair<long, double> Rate(long companyId, double rate);

        /// <summary>
        /// Aktualizuje ocene obiektu
        /// </summary>
        /// <returns>Para wartości <id oceny, ocena średnia></returns>
        KeyValuePair<long, double> UpdateRate(long companyId, long rateId, double rateValue);

        Company GetCompanyToEdit(string userName, string companyId);
    }
}

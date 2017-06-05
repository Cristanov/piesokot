using NaSpacerDo.Domain.Models;
using NaSpacerDo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Transactions;
using System.Linq.Expressions;
using log4net;

namespace NaSpacerDo.Logic
{
    public class CompanyBusinessProcess : ICompanyBusinessProcess
    {
        private readonly IFileService fileService;
        private readonly ApplicationDbContext context;
        private readonly IParametersProvider parameters;
        private readonly ILog logger;

        public CompanyBusinessProcess(ApplicationDbContext context, IFileService fileService, IParametersProvider parametersProvider,
            ILog logger)
        {
            this.context = context;
            this.fileService = fileService;
            this.parameters = parametersProvider;
            this.logger = logger;
        }

        public Company AddCompany(string usesrName, Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            if (IsLogoTooBig(company.Logo?.Content))
            {
                string message = string.Format("Obraz logo jest za duży. Rozmiar nie może przekraczać {0} MB", 2);
                throw new CompanyException(message);
            }

            ApplicationUser user = context.Users.FirstOrDefault(x => x.UserName == usesrName);

            string companyFolderPath = fileService.CreateCompanyDirectory(company.Name);
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (company.Logo != null)
                    {
                        company.Logo.Path = SaveLogo(company);
                    }

                    company.CreationDate = DateTime.UtcNow;
                    company.Owner = user;

                    var addedCompany = context.Companies.Add(company);
                    context.SaveChanges();

                    scope.Complete();
                    return addedCompany;
                }
                catch (Exception ex)
                {
                    fileService.Delete(companyFolderPath);
                    throw new CompanyException("Nie udało się dodać obiektu.", ex);
                }
            }
        }

        private string SaveLogo(Company company)
        {
            if (company.Logo?.Content != null && company.Logo.Content.Length > 0)
            {
                string imagePath = fileService.Save(company.Logo.FileName, company.Name, company.Logo.Content);
                return imagePath;
            }

            return null;
        }

        public ICollection<Company> GetCompanies()
        {
            return context.Companies.ToList();
        }

        public ICollection<Company> GetCompanies(CompanySearchCriteria searchCriteria)
        {
            IQueryable<Company> companies = context.Companies;
            if (searchCriteria != null)
            {
                if (!string.IsNullOrWhiteSpace(searchCriteria.Name))
                {
                    companies = companies.Where(x => x.Name.ToLower().Contains(searchCriteria.Name.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(searchCriteria.City))
                {
                    companies = companies.Where(x => x.Address.City.Name.ToLower().Contains(searchCriteria.City.ToLower()));
                }
            }
            else
            {
                companies = context.Companies;
            }

            return companies.ToList();
        }

        public Company GetCompany(long id)
        {
            Company company = context.Companies.FirstOrDefault(x => x.Id == id);

            if (company == null)
            {
                throw new CompanyException("Obiekt nie istnieje");
            }
            else
            {
                return company;
            }
        }

        public Company GetCompany(string name)
        {
            Company company;
            long companyId;

            if (long.TryParse(name, out companyId))
            {
                company = GetCompany(companyId);
            }
            else
            {
                company = context.Companies.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            }

            if (company == null)
            {
                throw new CompanyException("Obiekt nie istnieje");
            }
            else
            {
                return company;
            }
        }

        public Company UpdateCompany(string userName, Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            ApplicationUser user = context.Users.FirstOrDefault(x => x.UserName == userName);

            if (company.OwnerId != user.Id)
            {
                throw new PermissionCompanyException("Nie jesteś właścicielem obiektu który chcesz edytować");
            }

            context.Entry(company).State = EntityState.Modified;
            context.Entry(company.Address).State = EntityState.Modified;
            context.Entry(company.Address.City).State = EntityState.Modified;

            string oldLogoPath = company.Logo?.Path;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (company.Logo != null && company.Logo.Content?.Length > 0)
                    {
                        company.Logo.Path = SaveLogo(company);
                        context.Entry(company.Logo).State = EntityState.Modified;
                        if (!string.IsNullOrWhiteSpace(oldLogoPath))
                        {
                            fileService.DeleteFile(oldLogoPath);
                        }
                    }
                    context.SaveChanges();
                    scope.Complete();
                    logger.InfoFormat("Zaktualizowano dane obiektu: id: {0}", company.Id);
                }
                catch (Exception ex)
                {
                    logger.Error("Błąd podczas aktualizowania obiektu", ex);
                    if (company.Logo?.Path != null && company.Logo?.Path != oldLogoPath)
                    {
                        fileService.DeleteFile(company.Logo.Path);
                    }
                    throw new CompanyException("Nie udało się zaktualizować obiektu", ex);
                }
            }

            return company;
        }

        public bool IsExist(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            return context.Companies.Any(x => x.Name == company.Name && x.Id != company.Id);
        }

        private bool IsLogoTooBig(byte[] array)
        {
            if (array == null)
            {
                return false;
            }

            double sizeInMb = array.Length / 1024.0 / 1024;

            return sizeInMb > parameters.MaxLogoSize;
        }

        public Image AddImage(int companyId, Image image)
        {
            Company company = context.Companies.FirstOrDefault(x => x.Id == companyId);
            if (company == null)
            {
                string message = $"Nie istnieje obiekt o podanym identyfikatorze: {companyId}";
                logger.Error(message);
                throw new ArgumentException(message);
            }
            // TODO: rozmiar i typ
            using (TransactionScope scope = new TransactionScope())
            {
                string path = fileService.Save(image.FileName, company.Name, image.Content);

                image.Path = path;
                image.Company = company;

                context.Images.Add(image);
                context.SaveChanges();
                scope.Complete();
                logger.InfoFormat("Dodano zdjęcie do obiektu. Id zdjęcia: {0}. Id obiektu: {1}", image.Id, companyId);
            }

            return image;
        }

        public ICollection<Image> GetImages(int companyId)
        {
            if (!IsExist(companyId))
            {
                throw new CompanyException($"Obiekt o podanym identyfikatorze '{companyId}' nie istnieje");
            }

            return context.Images.Where(x => x.Company.Id == companyId).ToList();
        }

        private bool IsExist(int companyId)
        {
            return context.Companies.Any(x => x.Id == companyId);
        }

        public void DeleteImage(int id)
        {
            Image image = context.Images.FirstOrDefault(x => x.Id == id);
            if (image == null)
            {
                string message = $"Nie znaleziono zdjęcia o podanym identyfikatorze: {id}";
                logger.Error(message);
                throw new ArgumentException(message, nameof(id));
            }

            using (TransactionScope scope = new TransactionScope())
            {
                fileService.DeleteFile(fileService.GetPhysicalPath(image.Path));
                context.Images.Remove(image);
                context.SaveChanges();
                scope.Complete();
                logger.InfoFormat("Usunięto zdjęcie o id: {0}", id);
            }
        }

        public KeyValuePair<long, double> Rate(long companyId, double rate)
        {
            Company company = context.Companies.FirstOrDefault(x => x.Id == companyId);
            if (company == null)
            {
                string message = $"Obiekt o podanym identyfikatorze '{companyId}' nie istnieje";
                logger.ErrorFormat("Podczas dodawania oceny do obiektu wystąpił błąd: {0}", message);
                throw new CompanyException(message);
            }

            Rate rateToAdd = new Rate()
            {
                Company = company,
                Value = rate
            };

            company.Ratings.Add(rateToAdd);
            context.SaveChanges();

            double averageRate = Math.Round(company.Ratings.Sum(x => x.Value) / company.Ratings.Count, 1);
            return new KeyValuePair<long, double>(rateToAdd.Id, averageRate);
        }

        public KeyValuePair<long, double> UpdateRate(long companyId, long rateId, double rateValue)
        {
            Company company = context.Companies.FirstOrDefault(x => x.Id == companyId);
            if (company == null)
            {
                string message = $"Obiekt o podanym identyfikatorze '{companyId}' nie istnieje";
                logger.ErrorFormat("Podczas dodawania oceny do obiektu wystąpił błąd: {0}", message);
                throw new CompanyException();
            }

            Rate rateToUpdate = company.Ratings.FirstOrDefault(x => x.Id == rateId);
            if (rateToUpdate == null)
            {
                string message = "Nie znaleziono oceny o podanym identyfikatorze";
                logger.ErrorFormat("Podczas aktualizacji oceny obiektu wystąpił błąd: {0}", message);
                throw new CompanyException(message);
            }

            rateToUpdate.Value = rateValue;

            context.SaveChanges();

            double averageRate = Math.Round(company.Ratings.Sum(x => x.Value) / company.Ratings.Count, 1);
            return new KeyValuePair<long, double>(rateToUpdate.Id, averageRate);
        }

        public Company GetCompanyToEdit(string userName, string companyId)
        {
            Company company = GetCompany(companyId);

            if (company.Owner.UserName != userName)
            {
                throw new PermissionCompanyException("Nie masz uprawnień do edycji tego obiektu");
            }

            return company;
        }
    }
}

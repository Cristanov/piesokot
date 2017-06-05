using AutoMapper;
using PagedList;
using NaSpacerDo.Domain.Models;
using NaSpacerDo.Logic;
using NaSpacerDo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using log4net;
using NaSpacerDo.Domain.Enums;
using Microsoft.AspNet.Identity;

namespace NaSpacerDo.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICompanyBusinessProcess companyBusinessProcess;
        private readonly IParametersProvider parameters;
        private readonly IFileService fileService;
        private readonly IMapper mapper;

        public CompanyController(ILog log, IMapper mapper, ICompanyBusinessProcess companyBusinessProcess, IParametersProvider parameterProvider, IFileService fileService)
            : base(log)
        {
            this.mapper = mapper;
            this.companyBusinessProcess = companyBusinessProcess;
            this.parameters = parameterProvider;
            this.fileService = fileService;
        }

        public ActionResult Index(CompanyIndexViewModel vm)
        {
            ICollection<Company> companies = companyBusinessProcess.GetCompanies(vm.SearchCriteria);

            vm.Companies = companies.ToPagedList(vm.Page, parameters.PageSize);

            return View(vm);
        }

        [Attributes.Authorize(EnumRoles = Roles.Admin | Roles.User)]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Attributes.Authorize(EnumRoles = Roles.Admin | Roles.User)]
        public ActionResult Add(AddCompanyViewModel vm)
        {
            try
            {
                Company company = Mapper.Map<CompanyViewModel, Company>(vm.Company);

                // TODO: wywalić to sprawdzanie z kontrollera
                ValidateCompanyName(company);
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                companyBusinessProcess.AddCompany(User.Identity.Name, company);

                Success("Dodano nowy obiekt do bazy");
                return RedirectToAction(nameof(Show), new { id = company.Name });
            }
            catch (CompanyException ex)
            {
                log.Error("Błąd podczas dodawania obiektu", ex);
                Error(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Błąd podczas dodawania obiektu", ex);
                Error(string.Format("Podczas dodawania obiektu wystąpił nieoczekiwany błąd: {0}", ex.Message));
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Show(string id)
        {
            try
            {
                Company company = companyBusinessProcess.GetCompany(id);
                CompanyViewModel companyVM = Mapper.Map<CompanyViewModel>(company);
                return View(companyVM);
            }
            catch (CompanyException ex)
            {
                log.Error("Błąd podczas wyświetlania obiektu", ex);
                Error(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Błąd podczas wyświetlania obiektu", ex);
                Error("Wystąpił nieoczekiwany błąd");
            }

            return RedirectToAction(nameof(Index));
        }

        [Attributes.Authorize(EnumRoles = Roles.Admin | Roles.User)]
        public ActionResult Edit(string id)
        {
            try
            {
                Company company = companyBusinessProcess.GetCompanyToEdit(User.Identity.Name, id);

                EditCompanyViewModel vm = new EditCompanyViewModel
                {
                    Company = mapper.Map<Company, CompanyViewModel>(company),
                    MaxFilesLimit = parameters.CompanyImagesMaxLimit
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                log.Error("Błąd podczas edycji obiektu", ex);
                Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Attributes.Authorize(EnumRoles = Roles.Admin | Roles.User)]
        public ActionResult Edit(EditCompanyViewModel vm)
        {
            try
            {
                Company company = Mapper.Map<CompanyViewModel, Company>(vm.Company);

                // TODO: wywalić to sprawdzanie z kontrollera
                ValidateCompanyName(company);
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                companyBusinessProcess.UpdateCompany(User.Identity.Name, company);

                Success("Zaktualizowano dane firmy");
            }
            catch (Exception ex)
            {
                log.Error("Błąd podczas edycji obiektu", ex);
                Error("Wystąpił nieoczekiwany błąd: {0}", ex.Message);
            }

            return RedirectToAction(nameof(Show), new { id = vm.Company.Name });
        }

        public JsonResult Exist(Company Company)
        {
            try
            {
                bool isExist = companyBusinessProcess.IsExist(Company);

                return Json(!isExist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error("Błąd podczas sprawdzania istnienia obiektu", ex);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddImage(int companyId)
        {
            try
            {
                Image addedImage = new Image();
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];

                    Image image = new Image
                    {
                        FileName = file.FileName,
                    };

                    using (BinaryReader reader = new BinaryReader(file.InputStream))
                    {
                        image.Content = reader.ReadBytes((int)reader.BaseStream.Length);
                    }

                    addedImage = companyBusinessProcess.AddImage(companyId, image);
                }

                return Json(new { msg = string.Empty, id = addedImage.Id });
            }
            catch (Exception ex)
            {
                log.Error("Błąd podczas dodawania zdjęcia", ex);
                return Json(new { message = ex.Message, status = "error", code = "403" });
            }
        }

        public JsonResult GetImages(int companyId)
        {
            ICollection<DropzoneImageViewModel> images = new List<DropzoneImageViewModel>();
            try
            {
                images = (from i in companyBusinessProcess.GetImages(companyId)
                          where !i.IsLogo
                          select new DropzoneImageViewModel
                          {
                              FileName = i.FileName,
                              Id = i.Id,
                              IsLogo = i.IsLogo,
                              Path = i.Path,
                              Size = fileService.GetSize(i.Path)
                          }).ToList();
            }
            catch (Exception ex)
            {
                log.Error("Błąd podczas pobierania zdjęć", ex);
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { images = images }, JsonRequestBehavior.AllowGet);
        }

        private void ValidateCompanyName(Company company)
        {
            if (companyBusinessProcess.IsExist(company))
            {
                ModelState.AddModelError("Company.Name", "Obiekt o podanej nazwie już istnieje");
            }
        }

        [Attributes.Authorize(EnumRoles = Roles.Admin | Roles.User)]
        public JsonResult DeleteImage(int id)
        {
            try
            {
                companyBusinessProcess.DeleteImage(id);
            }
            catch (Exception ex)
            {
                log.Error("Błąd podczas usuwania zdjęcia", ex);
                return Json(new { error = ex.Message });
            }

            return Json(new { });
        }

        public JsonResult AddRate(long companyId, double rate)
        {
            try
            {
                KeyValuePair<long, double> rating = companyBusinessProcess.Rate(companyId, rate);

                return Json(new { rating = rating.Value, rateId = rating.Key, companyId = companyId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Wystąpił błąd podczas oceny obiektu. Id: {companyId}", ex);
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateRate(long companyId, long rateId, double rate)
        {
            try
            {
                KeyValuePair<long, double> updateResult = companyBusinessProcess.UpdateRate(companyId, rateId, rate);
                return Json(new { rating = updateResult.Value, rateId = updateResult.Key, companyId = companyId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error($"Wystąpił błąd podczas oceny obiektu. Id: {companyId}", ex);
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
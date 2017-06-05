using AutoMapper;
using log4net;
using Moq;
using NaSpacerDo.Controllers;
using NaSpacerDo.Logic;
using NaSpacerDo.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace NaSpacerDo.Tests.Controllers
{
    [TestFixture]
    public class CompanyControllerTests
    {
        private CompanyController controller;
        Mock<ILog> logMock;
        Mock<ICompanyBusinessProcess> companyBusinessProcessMock;
        Mock<IParametersProvider> parameterProviderMock;
        Mock<IFileService> fileServiceMock;
        Mock<IMapper> mapperMock;

        [SetUp]
        public void SetUp()
        {
            logMock = new Mock<ILog>();
            companyBusinessProcessMock = new Mock<ICompanyBusinessProcess>();
            parameterProviderMock = new Mock<IParametersProvider>();
            fileServiceMock = new Mock<IFileService>();
            mapperMock = new Mock<IMapper>();

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("userName");
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            controllerContextMock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            controller = new CompanyController(logMock.Object, mapperMock.Object, companyBusinessProcessMock.Object,
                        parameterProviderMock.Object, fileServiceMock.Object)
            {
                ControllerContext = controllerContextMock.Object
            };
        }

        [Test]
        public void Company_Edit_HasMaxImagesLimit()
        {
            byte maxFilesLimit = 4;
            parameterProviderMock.SetupGet(x => x.CompanyImagesMaxLimit).Returns(maxFilesLimit);
            companyBusinessProcessMock.Setup(x => x.GetCompanyToEdit(It.IsAny<string>(), It.IsAny<string>())).Returns(new Domain.Models.Company());
            ViewResult viewResult = controller.Edit("1") as ViewResult;

            EditCompanyViewModel viewModel = viewResult.Model as EditCompanyViewModel;

            Assert.AreEqual(maxFilesLimit, viewModel.MaxFilesLimit);
        }

        [Test]
        public void Company_AddRate_ReturnsValidRating()
        {
            long companyId = 12;
            double rate = 2.5;
            double averageRating = 1.5;
            long rateId = 123;
            KeyValuePair<long, double> ratingResult = new KeyValuePair<long, double>(rateId, averageRating);
            companyBusinessProcessMock.Setup(x => x.Rate(companyId, rate)).Returns(ratingResult);

            JsonResult result = controller.AddRate(companyId, rate);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);

            dynamic data = result.Data;
            Assert.AreEqual(averageRating, data.rating);
            Assert.AreEqual(companyId, data.companyId);
            Assert.AreEqual(rateId, data.rateId);
        }

        [Test]
        public void Company_AddRate_ReturnsErrorWhenCantFindCompanyAndLogError()
        {
            long companyId = 0;
            double rate = 2.5;
            companyBusinessProcessMock.Setup(x => x.Rate(companyId, rate)).Throws(new CompanyException());
            JsonResult result = controller.AddRate(companyId, rate);

            dynamic data = result.Data;

            Assert.IsFalse(string.IsNullOrWhiteSpace(data.error));
            logMock.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>()));
        }

        [Test]
        public void Company_UpdateRate_ReturnsErrorWhenCantFindCompanyAndLogError()
        {
            long companyId = 0;
            long rateId = 2;
            double rateValue = 2.2;
            companyBusinessProcessMock.Setup(x => x.UpdateRate(companyId, rateId, rateValue)).Throws(new CompanyException());

            JsonResult result = controller.UpdateRate(companyId, rateId, rateValue);
            dynamic data = result.Data;

            Assert.False(string.IsNullOrWhiteSpace(data.error));
            logMock.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>()));
        }

        [Test]
        public void Company_UpdateRate_ReturnsValidResult()
        {
            long companyId = 2;
            long rateId = 2;
            double rateValue = 2.2;
            double averageRating = 4.8;
            KeyValuePair<long, double> updateRateResult = new KeyValuePair<long, double>(rateId, averageRating);

            companyBusinessProcessMock.Setup(x => x.UpdateRate(companyId, rateId, rateValue)).Returns(updateRateResult);
            JsonResult result = controller.UpdateRate(companyId, rateId, rateValue);
            dynamic data = result.Data;

            Assert.IsNotNull(data);
            Assert.AreEqual(averageRating, data.rating);
            Assert.AreEqual(companyId, data.companyId);
            Assert.AreEqual(rateId, data.rateId);
        }
    }
}

using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using AutoMapper;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using NaSpacerDo.Domain;
using NaSpacerDo.Domain.Models;
using NaSpacerDo.Identity;
using NaSpacerDo.Logic;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace NaSpacerDo.App_Start
{
    public class PieskotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            Mapper.Initialize(x => x.AddProfile<AutoMapperConfiguration>());
            builder.RegisterInstance(Mapper.Instance);
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<CompanyBusinessProcess>().AsImplementedInterfaces().AsSelf();
            log4net.Config.XmlConfigurator.Configure();
            builder.Register(c => LogManager.GetLogger(typeof(object))).As<ILog>();

            string physicalRoot = HostingEnvironment.ApplicationPhysicalPath;
            string uploadPath = GetParameter(Constants.UploadImagesFolderPathParameterName);

            List<Parameter> fileServiceParameters = new List<Parameter> {
                new NamedParameter("rootPath", physicalRoot),
                new NamedParameter("uploadPath", uploadPath)
            };
            builder.RegisterType<FileService>().As<IFileService>().WithParameters(fileServiceParameters);

            IParametersProvider parameterProvider = new ParametersProvider
            {
                MaxLogoSize = double.Parse(GetParameter(Constants.MaxLogoSizeParameterName)),
                PageSize = int.Parse(GetParameter(Constants.PageSizeParameterName)),
                CompanyImagesMaxLimit = byte.Parse(GetParameter(Constants.CompanyImagesMaxLimit))
            };

            builder.RegisterInstance(parameterProvider);

            List<Parameter> emailServiceParameters = new List<Parameter> {
                new NamedParameter("sender", GetParameter(Constants.NaSpacerDoEmailParameterName)),
                new NamedParameter("password", GetParameter(Constants.NaSpacerDoEmailPasswordParameterName)),
                new NamedParameter("host", GetParameter(Constants.NaSpacerDoEmailHostParameterName)),
                new NamedParameter("port", int.Parse(GetParameter(Constants.NaSpacerDoEmailPortParameterName))),
            };
            builder.RegisterType<Logic.Services.EmailService>().WithParameters(emailServiceParameters)
                .AsImplementedInterfaces().AsSelf();

            builder.RegisterType<EmailService>().AsSelf().AsImplementedInterfaces();

            builder.Register(c => new UserStore<ApplicationUser>(c.Resolve<ApplicationDbContext>())).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>
            {
                DataProtectionProvider = new DpapiDataProtectionProvider("Application​")
            });
        }

        private string GetParameter(string parameterName)
        {
            return ConfigurationManager.AppSettings[parameterName].ToString();
        }
    }
}
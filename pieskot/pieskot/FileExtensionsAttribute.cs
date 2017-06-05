using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace NaSpacerDo.Attributes
{
    public class FileExtensionsAttribute : ValidationAttribute, IClientValidatable
    {
        public FileExtensionsAttribute(string extensions)
        {
            if (string.IsNullOrWhiteSpace(extensions))
            {
                throw new ArgumentException($"{nameof(extensions)} can't be null or whitespace.");
            }

            Extensions = extensions.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] Extensions { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (var extension in Extensions)
            {
                if (value == null || value.ToString().EndsWith(extension))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(", ", Extensions));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.PropertyName),
                ValidationType = "fileextensions",
            };

            rule.ValidationParameters.Add("extensions", string.Join(";", Extensions));

            yield return rule;
        }
    }
}
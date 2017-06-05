using NaSpacerDo.Domain.Enums;
using System;

namespace NaSpacerDo.Attributes
{
    public class Authorize : System.Web.Mvc.AuthorizeAttribute
    {
        public Roles EnumRoles
        {
            get
            {
                throw new NotImplementedException($"Właściwość {nameof(EnumRoles)} nie jest zaimplementowana");
            }
            set
            {
                Roles = value.ToString();
            }
        }
    }
}
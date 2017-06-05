using System.Web.SessionState;
using System.Threading;
using System.Globalization;
using NaSpacerDo.Enums;

namespace NaSpacerDo.Helpers
{
    public class CultureHelper
    {
        private static Language language;
       
        public static Language Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;

                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language.GetDescription());
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
        }
    }
}
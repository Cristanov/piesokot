namespace NaSpacerDo.App_Start
{
    public class Constants
    {
        public enum Alerts
        {
            Error,
            Warning,
            Success,
            Info
        }
        public const string ERROR = "AlertError";
        public const string WARNING = "AlertWarning";
        public const string SUCCESS = "AlertSuccess";

        /// <summary>
        /// Nazwa parametru ścieżki do folderu w którym są zapisywane zdjęcia dodawane przez użytkowników
        /// </summary>
        public const string UploadImagesFolderPathParameterName = "UploadImagesFolderPath";

        /// <summary>
        /// Nazwa parametru zawierającego maksymalny rozmiar logo w megabajtach
        /// </summary>
        public const string MaxLogoSizeParameterName = "MaxLogoSize";

        /// <summary>
        /// Nazwa parametru zawierającego liczbę elementów na stronie
        /// </summary>
        public const string PageSizeParameterName = "PageSize";

        /// <summary>
        /// Nazwa parametru zawierającego maksymalną liczbę zdjęć możliwych do dodania do obiektu
        /// </summary>
        public const string CompanyImagesMaxLimit = "CompanyImagesMaxLimit";

        public const string NaSpacerDoEmailParameterName = "NaSpacerDoEmail";

        public const string NaSpacerDoEmailPasswordParameterName = "NaSpacerDoEmailPassword";

        public const string NaSpacerDoEmailHostParameterName = "NaSpacerDoEmailHost";

        public const string NaSpacerDoEmailPortParameterName = "NaSpacerDoEmailPort";
    }
}
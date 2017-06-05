namespace NaSpacerDo.Logic
{
    public class ParametersProvider : IParametersProvider
    {
        public byte CompanyImagesMaxLimit
        {
            get; set;
        }

        public double MaxLogoSize
        {
            get; set;
        }

        public int PageSize
        {
            get; set;
        }
    }
}

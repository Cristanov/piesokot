namespace NaSpacerDo.Logic
{
    public interface IParametersProvider
    {
        double MaxLogoSize { get; }
        int PageSize { get; }
        byte CompanyImagesMaxLimit { get; }
    }
}

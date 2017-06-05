namespace NaSpacerDo.ViewModels
{
    public class RateViewModel
    {
        public long Id { get; set; }

        public double Value { get; set; }

        public virtual CompanyViewModel Company { get; set; }
    }
}
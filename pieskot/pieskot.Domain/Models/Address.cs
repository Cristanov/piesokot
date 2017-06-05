namespace NaSpacerDo.Domain.Models
{
    public class Address
    {
        public long Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string ZipCode { get; set; }

        public virtual City City { get; set; }
    }
}
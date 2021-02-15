using EFZ.Core.Entities;

namespace EFZ.Entities.Entities
{
    public class InvoiceAddress : BaseEntity
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LAstName { get; set; }
        public string Company { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string AlternativeStreetNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        // ReSharper disable once InconsistentNaming
        public string IC { get; set; }
        // ReSharper disable once InconsistentNaming
        public string VAT { get; set; }
        // ReSharper disable once InconsistentNaming
        public string DIC { get; set; }
    }
}

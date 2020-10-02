using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Institution.ViewModel
{
    public class InstitutionAddressVM
    {
        public int? InstitutionId { get; set; }

        public int? AddressId { get; set; }

        public int? LanguageId { get; set; }

        public int? InstitutionDetailId { get; set; }

        public string LanguageCode { get; set; }

        public string LanguageName { get; set; }

        [Required]
        public string InstitutionName { get; set; }

        public string InstitutionDescription { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public string Province { get; set; }

        [Required]
        public string Region { get; set; }

        public string Country { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }
    }
}

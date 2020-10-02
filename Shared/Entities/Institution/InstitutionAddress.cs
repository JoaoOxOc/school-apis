using Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Institution
{
    public class InstitutionAddress : BaseModel
    {
        [Key]
        public int? Id { get; set; }

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

        #region ForeignKeys

        public int? InstitutionId { get; set; }

        [ForeignKey("InstitutionId")]
        public virtual Institution Institution { get; set; }

        #endregion

        #region MetaData

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        #endregion
    }
}

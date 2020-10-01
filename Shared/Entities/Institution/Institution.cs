using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Institution
{
    public class Institution : BaseModel
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        #region ForeignKeys

        public virtual ICollection<InstitutionAddress> InstitutionAddresses { get; set; }

        public virtual ICollection<InstitutionLanguageDetail> InstitutionDetailLanguages { get; set; }

        #endregion

        #region MetaData

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        #endregion
    }
}

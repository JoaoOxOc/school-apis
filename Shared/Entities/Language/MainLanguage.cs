using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Language
{
    public class MainLanguage : BaseModel
    {
        [Key]
        public int? Id { get; set; }

        public string Code { set; get; }

        [Required]
        public string Name { get; set; }

        public string PhonePrefix { get; set; }

        public string PhoneNumberStructure { get; set; }

        public string ValidateAddressApiUrl { get; set; }

        #region MetaData

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        #endregion
    }
}

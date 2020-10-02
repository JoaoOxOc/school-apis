using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Entities.Language;

namespace Entities.Institution
{
    public class InstitutionLanguageDetail
    {
        [Key]
        public int? Id { get; set; }

        #region TranslatableFields

        public string Description { get; set; }

        #endregion

        #region ForeignKeys

        public int? InstitutionId { get; set; }

        public int? LanguageId { get; set; }

        [ForeignKey("InstitutionId")]
        public virtual Institution Institution { get; set; }

        [ForeignKey("LanguageId")]
        public virtual MainLanguage Language { get; set; }

        #endregion
    }
}

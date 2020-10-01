using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Entities.DMS
{
    public class File
    {
        #region Properties

        [DataMember]
        [Required]
        [Key]
        public int Id { get; set; }

        [DataMember]
        public string MimeType { get; set; }

        [DataMember]
        [Required]
        public string FilePath { get; set; }

        [DataMember]
        [Required]
        public string OriginalFilename { get; set; }

        [DataMember]
        [Required]
        public string Filename { get; set; }

        [DataMember]
        public string Extension { get; set; }

        #endregion

        #region Metadata

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public DateTime UpdatedAt { get; set; }

        [DataMember]
        public int? CreatedBy { get; set; }

        [DataMember]
        public int? UpdatedBy { get; set; }

        #endregion

        #region Constructors

        public File() { }

        #endregion
    }
}

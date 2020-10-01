using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Entities.Hangfire
{
    public class AppJob
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int NumberOfParameters { get; set; }

        public string Fields { get; set; }

        public bool IsActive { get; set; }
    }
}

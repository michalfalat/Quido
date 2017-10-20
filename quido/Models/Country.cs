using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace quido.Models
{
    public class Country
    {
        [Key]
        public string ISOCode { get; set; }
        public string Name { get; set; }
    }
}
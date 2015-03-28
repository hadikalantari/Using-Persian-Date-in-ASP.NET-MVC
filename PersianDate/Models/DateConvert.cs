using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersianDate.Models
{
    public class DateConvert
    {
        [Display(Name = "Gregorian Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:YYYY-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime gregorianDateFrom { get; set; }

        [Display(Name = "Persian Date")]
        [DataType(DataType.Text)]
        public string persianDateFrom { get; set; }

        [Display(Name = "Gregorian Date")]
        [DataType(DataType.Date)]
        public DateTime? gregorianDateConverted { get; set; }
        
        [Display(Name = "Persian Date")]
        public PersianDateTime persianDateConverted { get; set; }

    }

}
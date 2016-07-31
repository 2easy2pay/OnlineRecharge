using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class SmsDetails
    {
        public SmsDetails()
        {
        }
        [Key]
        public int SmsDetailsId { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; } 
        public string DeviceType { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
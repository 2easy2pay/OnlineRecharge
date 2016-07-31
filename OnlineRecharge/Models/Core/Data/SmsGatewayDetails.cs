using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Data
{
    public class SmsGatewayDetails
    {
        public SmsGatewayDetails()
        {
        }
        [Key]
        public int SmsGatewayDetailsId { get; set; }
        public string SmsGateway { get; set; } 
        public int SmsGatewayCode { get; set; }
        public string Username { get; set; } 
        public string Password { get; set; }
        public string ApiId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
using OnlineRecharge.Models.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Mapping
{
    public class SmsDetailsMap : EntityTypeConfiguration<SmsDetails>
    {
        public SmsDetailsMap()
        {
            //key  
            HasKey(t => t.SmsDetailsId);

            //fieds  

            Property(t => t.CountryCode).IsOptional().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.Date).IsRequired();
            Property(t => t.DeviceType).IsOptional();
            Property(t => t.Message).IsOptional();
            Property(t => t.PhoneNumber).IsOptional();
            Property(t => t.Status).IsOptional();
           
            //table  
            ToTable("SmsDetails");
        }
    }
}
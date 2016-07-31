using OnlineRecharge.Models.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Mapping
{
    public class SmsGatewayDetailsMap : EntityTypeConfiguration<SmsGatewayDetails> 
    {
        public SmsGatewayDetailsMap()
        {
            //key  
            HasKey(t => t.SmsGatewayDetailsId);

            //fieds  

            Property(t => t.ApiId).IsOptional().HasMaxLength(50).HasColumnType("nvarchar");
            Property(t => t.SmsGateway).IsRequired();
            Property(t => t.Username).IsRequired();
            Property(t => t.Password).IsRequired();
            Property(t => t.CreatedDate).IsOptional();
            Property(t => t.IsActive).IsOptional();
           
            //table  
            ToTable("SmsGatewayDetails");
        }
    }
}
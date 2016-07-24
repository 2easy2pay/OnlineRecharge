using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineRecharge.Models.Core.Data;

namespace OnlineRecharge.Models.Core.Mapping
{
    public class NationalRechargeAPIResponseDetailMap : EntityTypeConfiguration<NationalRechargeAPIResponseDetails>
    {
        public NationalRechargeAPIResponseDetailMap()
        {
            //key  
            HasKey(t => t.ID);

            //fieds  
            Property(t => t.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.PaymentID).IsRequired().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.Response).IsRequired().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.ResponseDescription).IsRequired().HasMaxLength(1000).HasColumnType("nvarchar");
            Property(t => t.PaymentRef).IsRequired().HasMaxLength(300).HasColumnType("nvarchar");
            Property(t => t.Date).IsRequired().HasColumnType("datetime");
            //table  
            ToTable("NationalRechargeAPIResponseDetails");

            //relationship  
            HasRequired(t => t.NationalRecharge).WithRequiredDependent(u => u.NationalRechargeAPIResponseDetail);
          
        }
    }
}
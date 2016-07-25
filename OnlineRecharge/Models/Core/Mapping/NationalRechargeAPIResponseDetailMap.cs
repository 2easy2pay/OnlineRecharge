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
            Property(t => t.PaymentID).IsOptional().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.Response).IsRequired().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.ResponseDescription).IsOptional().HasMaxLength(1000).HasColumnType("nvarchar");
            Property(t => t.PaymentRef).IsOptional().HasMaxLength(300).HasColumnType("nvarchar");
            Property(t => t.Date).IsOptional().HasColumnType("datetime");
            Property(t => t.Denomination).IsOptional().HasColumnType("decimal");
            Property(t => t.OperatorName).IsOptional().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.Password).IsOptional().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.RechargeCode).IsOptional().HasMaxLength(50).HasColumnType("nvarchar");
            Property(t => t.SerialNo).IsOptional().HasMaxLength(50).HasColumnType("nvarchar");
            //table  
            ToTable("NationalRechargeAPIResponseDetails");


          
        }
    }
}
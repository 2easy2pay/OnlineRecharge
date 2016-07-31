using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineRecharge.Models.Core.Data;

namespace OnlineRecharge.Models.Core.Mapping
{
    public class RechargeParameterMap : EntityTypeConfiguration<RechargeParameters>
    {
        public RechargeParameterMap()
        {
            //key  
            HasKey(t => t.ID);
            //fieds  
            Property(t => t.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.ServiceType).IsRequired().HasMaxLength(30).HasColumnType("nvarchar");
            Property(t => t.RechargeType).IsRequired().HasMaxLength(30).HasColumnType("nvarchar");
            Property(t => t.OperatorCode).IsRequired().HasMaxLength(30).HasColumnType("nvarchar"); Property(t => t.MobileNumber).IsRequired().HasMaxLength(30).HasColumnType("nvarchar");
            Property(t => t.Amount).IsRequired().HasColumnType("decimal");

            Property(t => t.IsActive).IsRequired();
            Property(t => t.IsDeleted).IsOptional();
            Property(t => t.CreatedBy).IsRequired();
            Property(t => t.CreatedDate).IsRequired();
            Property(t => t.ModifiedBy).IsOptional();
            Property(t => t.ModifiedDate).IsOptional();
            Property(t => t.DeletedBy).IsOptional();
            Property(t => t.DeletedDate).IsOptional();
            //table  
            ToTable("RechargeParameters");
            
        }
    }
}
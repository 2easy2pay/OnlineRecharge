using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineRecharge.Models.Core.Data;

namespace OnlineRecharge.Models.Core.Mapping
{
    public class DataCardRechargeMap : EntityTypeConfiguration<DataCardRecharges>
    {
        public DataCardRechargeMap()
        {
            //key  
            HasKey(t => t.ID);

            //fieds  
            Property(t => t.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.MobileNumber).IsRequired().HasMaxLength(20).HasColumnType("nvarchar");
            Property(x => x.CustomerID).IsRequired().HasColumnType("int");
            Property(t => t.amount).IsRequired().HasColumnType("decimal");
            Property(t => t.IsActive).IsRequired();
            Property(t => t.IsDeleted).IsOptional();
            Property(t => t.CreatedBy).IsRequired();
            Property(t => t.CreatedDate).IsRequired();
            Property(t => t.ModifiedBy).IsOptional();
            Property(t => t.ModifiedDate).IsOptional();
            Property(t => t.DeletedBy).IsOptional();
            Property(t => t.DeletedDate).IsOptional();

            //table  
            ToTable("DataCardRecharges");

        }
    }
}
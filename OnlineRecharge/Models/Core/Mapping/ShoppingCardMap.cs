using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineRecharge.Models.Core.Data;

namespace OnlineRecharge.Models.Core.Mapping
{
    public class ShoppingCardMap : EntityTypeConfiguration<ShoppingCards>
    {
        public ShoppingCardMap()
        {
            //key  
            HasKey(t => t.ID);

            //fieds  
            Property(t => t.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
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
            ToTable("ShoppingCards");

        }
    }
}
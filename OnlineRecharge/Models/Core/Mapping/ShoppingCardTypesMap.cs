using OnlineRecharge.Models.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace OnlineRecharge.Models.Core.Mapping
{
    public class ShoppingCardTypesMap : EntityTypeConfiguration<ShoppingCardTypes>
    {
        public ShoppingCardTypesMap()
        {
            //key  
            HasKey(t => t.ID);

            //fieds  
            Property(t => t.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Name).IsRequired().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.Code).IsRequired().HasMaxLength(10).HasColumnType("nvarchar");
            Property(t => t.IsActive).IsRequired();
            Property(t => t.IsDeleted).IsOptional();
            Property(t => t.CreatedBy).IsRequired();
            Property(t => t.CreatedDate).IsRequired();
            Property(t => t.ModifiedBy).IsOptional();
            Property(t => t.ModifiedDate).IsOptional();
            Property(t => t.DeletedBy).IsOptional();
            Property(t => t.DeletedDate).IsOptional();

            //table  
            ToTable("ShoppingCardTypes");
        }
    }
}
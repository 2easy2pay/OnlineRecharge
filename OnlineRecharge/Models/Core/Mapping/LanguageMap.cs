using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineRecharge.Models.Core.Data;

namespace OnlineRecharge.Models.Core.Mapping
{
    public class LanguageMap : EntityTypeConfiguration<Languages>
    {
        public LanguageMap()
        {
            //key  
            HasKey(t => t.ID);

            //fieds  
            Property(t => t.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Code).IsRequired().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.Name).IsRequired().HasMaxLength(100).HasColumnType("nvarchar");
            Property(t => t.IsActive).IsRequired();
            Property(t => t.IsDeleted).IsOptional();
            Property(t => t.CreatedBy).IsRequired();
            Property(t => t.CreatedDate).IsRequired();
            Property(t => t.ModifiedBy).IsOptional();
            Property(t => t.ModifiedDate).IsOptional();
            Property(t => t.DeletedBy).IsOptional();
            Property(t => t.DeletedDate).IsOptional();

            //table  
            ToTable("Languages");

            //relationship  
            HasRequired(t => t.Flag).WithRequiredDependent(u => u.Language);
            HasRequired(t => t.Directions).WithRequiredDependent(u => u.Language);
        }
    }
}
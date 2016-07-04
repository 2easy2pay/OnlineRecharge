using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineRecharge.Models.Core.Data;

namespace OnlineRecharge.Models.Core.Mapping
{
    public class LanguageDirectionMap : EntityTypeConfiguration<LanguageDirections>
    {
        public LanguageDirectionMap()
        {
            //Key  
            HasKey(t => t.ID);

            //Fields  
            Property(t => t.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.Direction).IsRequired().HasMaxLength(10).HasColumnType("nvarchar");
            //table  
            ToTable("LanguageDirections");
        }
    }
}
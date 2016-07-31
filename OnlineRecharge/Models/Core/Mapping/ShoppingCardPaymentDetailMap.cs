using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OnlineRecharge.Models.Core.Data;

namespace OnlineRecharge.Models.Core.Mapping
{
    public class ShoppingCardPaymentDetailMap : EntityTypeConfiguration<ShoppingCardPaymentDetails>
    {
        public ShoppingCardPaymentDetailMap()
        {
            //key  
            HasKey(t => t.ID);

            //fieds  
            Property(t => t.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.PaymentID).IsRequired().HasMaxLength(50).HasColumnType("nvarchar");
            Property(t => t.TrackID).IsRequired().HasMaxLength(50).HasColumnType("nvarchar");
            Property(t => t.TransID).IsRequired().HasMaxLength(50).HasColumnType("nvarchar");
            Property(t => t.Result).IsRequired().HasMaxLength(50).HasColumnType("nvarchar");
            Property(t => t.Ref).IsRequired().HasMaxLength(50).HasColumnType("nvarchar");
            //table  
            ToTable("ShoppingCardPaymentDetails");

          
        }
    }
}
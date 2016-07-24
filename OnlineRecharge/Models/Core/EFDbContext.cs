using OnlineRecharge.Models.Core.Data;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;

namespace OnlineRecharge.Models.Core
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=DbConnectionString")
        {

        }

        public virtual DbSet<ServiceProviders> ServiceProiders
        { get; set; }
        public virtual DbSet<NationalRechargeTypes> NationalRechargeTypes
        { get; set; }
        public virtual DbSet<NationalRecharges> NationalRecharges
        { get; set; }
        public virtual DbSet<NationalRechargePaymentDetails> NationalRechargePaymentDetails
        { get; set; }
        public virtual DbSet<NationalRechargeAPIResponseDetails> NationalRechargeAPIResponseDetails
        { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
           .Where(type => !String.IsNullOrEmpty(type.Namespace))
           .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
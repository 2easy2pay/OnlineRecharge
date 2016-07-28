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

        public virtual DbSet<Countries> Countries
        { get; set; }

        #region International
       
        public virtual DbSet<InternationalRecharges> InternationalRecharges
        { get; set; }
        public virtual DbSet<InternationalRechargePaymentDetails> InternationalRechargePaymentDetails
        { get; set; }
        public virtual DbSet<InternationalRechargeAPIResponseDetails> InternationalRechargeAPIResponseDetails
        { get; set; }


        #endregion

        #region Data Card

        public virtual DbSet<DataCardRecharges> DataCardRecharge { get; set; }

        public virtual DbSet<DataCardRechargePaymentDetails> DataCardRechargePaymentDetail { get; set; }

        public virtual DbSet<DataCardRechargeAPIResponseDetails> DataCardRechargeApiDetail { get; set; }


        #endregion

        public virtual DbSet<InternationalServiceProviders> internationalServiceProviders
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
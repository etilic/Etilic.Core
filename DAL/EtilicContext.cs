using Etilic.Core.Diagnostics;
using Etilic.Core.Extensibility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etilic.Core.DAL
{
    public class EtilicContext : DbContext
    {
        #region Properties
        /// <summary>
        /// Gets or sets the collection of people.
        /// </summary>
        public DbSet<Person> People
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the collection of bundle configuration entries.
        /// </summary>
        public DbSet<BundleConfigEntry> BundleConfiguration
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the collection of diagnostic messages.
        /// </summary>
        public DbSet<Diagnostic> Diagnostics
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public EtilicContext()
            : base("EtilicContext")
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public EtilicContext(String connectionString)
            : base(connectionString)
        {

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Add(new DateTimeConvention());
        }
    }
}

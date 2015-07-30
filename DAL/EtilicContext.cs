using Etilic.Core.Extensibility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etilic.Core.DAL
{
    public class EtilicContext : DbContext
    {
        #region Properties
        public DbSet<Person> People
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
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            foreach(Bundle bundle in BundleManager.Bundles.Values)
            {
                bundle.RegisterEntities(modelBuilder);
            }
        }
    }
}

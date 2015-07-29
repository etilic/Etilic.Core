using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.RegisterEntityType()
        }
    }
}

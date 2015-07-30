using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etilic.Core.Extensibility
{
    /// <summary>
    /// Represents a bundle which may be loaded 
    /// </summary>
    public abstract class Bundle
    {
        #region Properties
        /// <summary>
        /// When overriden by a deriving class, this property gets the globally 
        /// unique ID of this bundle.
        /// </summary>
        public abstract Guid BundleID
        {
            get;
        }
        /// <summary>
        /// When overriden by a deriving class, this property gets an array of
        /// globally unique IDs which identify the bundles on which this bundle
        /// depends.
        /// </summary>
        public abstract Guid[] Dependencies
        {
            get;
        }
        #endregion

        #region RegisterEntities
        /// <summary>
        /// Allows deriving classes to register database entities.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public virtual void RegisterEntities(DbModelBuilder modelBuilder)
        {

        }
        #endregion
    }
}

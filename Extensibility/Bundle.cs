using Etilic.Core.Diagnostics;
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
        #region Instance members
        /// <summary>
        /// The configuration of this bundle.
        /// </summary>
        private BundleConfig config;
        #endregion

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
        public virtual Guid[] Dependencies
        {
            get { return new Guid[] {}; }
        }
        /// <summary>
        /// Gets the configuration for this bundle.
        /// </summary>
        public BundleConfig Configuration
        {
            get { return this.config; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// When overriden by a deriving class, this constructor initialises the
        /// generic components of a bundle.
        /// </summary>
        public Bundle()
        {
            this.config = new BundleConfig(this.BundleID);
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


        public virtual void OnAdded()
        {

        }

        #region WriteDiagnostic
        /// <summary>
        /// Writes a diagnostic.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void WriteDiagnostic(DiagnosticLevel level, String message)
        {
            Diagnostic diagnostic = new Diagnostic(this.BundleID);
            diagnostic.Level = level;
            diagnostic.Message = message;


        }
        #endregion
    }
}

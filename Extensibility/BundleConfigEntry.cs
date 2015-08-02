using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etilic.Core.Extensibility
{
    /// <summary>
    /// Represents an entry in a bundle's configuration.
    /// </summary>
    public class BundleConfigEntry
    {
        #region Properties
        /// <summary>
        /// Gets or sets the globally unique ID of this entry.
        /// </summary>
        public Guid ID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the globally unique ID of the bundle to which this entry belongs.
        /// </summary>
        public Guid BundleID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the key of this entry.
        /// </summary>
        public String Key
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the value of this entry.
        /// </summary>
        public String Value
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialises a new instance of the <see cref="Etilic.Core.Extensibility.BundleConfigEntry"/> class
        /// with a new globally unique ID.
        /// </summary>
        public BundleConfigEntry()
        {
            this.ID = Guid.NewGuid();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etilic.Core.Diagnostics
{
    /// <summary>
    /// Represents a diagnostic message.
    /// </summary>
    public class Diagnostic
    {
        #region Properties
        /// <summary>
        /// Gets or sets the globally unique ID of this diagnostic message.
        /// </summary>
        public Guid ID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the <see cref="System.DateTime"/> object describing when this message was created.
        /// </summary>
        [Required]
        public DateTime Created
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the globally unique ID indicating the origin of this diagnostic message.
        /// </summary>
        public Guid ServiceID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the severity of this message.
        /// </summary>
        [Required]
        public DiagnosticLevel Level
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [Required]
        public String Message
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialises a new <see cref="Etilic.Core.Diagnostics.Diagnostic"/> object with
        /// a new globally unique ID, created at the current universal time.
        /// </summary>
        public Diagnostic()
        {
            this.ID = Guid.NewGuid();
            this.Created = DateTime.UtcNow;
        }
        /// <summary>
        /// Initialises a new <see cref="Etilic.Core.Diagnostics.Diagnostic"/> object with
        /// a new globally unique ID, created at the current universal time.
        /// </summary>
        /// <param name="originID">
        /// The globally unique ID indicating where this diagnostic originated.
        /// </param>
        public Diagnostic(Guid originID)
            : this()
        {
            this.ServiceID = originID;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etilic.Core.Diagnostics
{
    /// <summary>
    /// Enumerates severity levels for diagnostic messages.
    /// </summary>
    public enum DiagnosticLevel : short
    {
        /// <summary>
        /// The message is for debugging only.
        /// </summary>
        Debug = 0,
        /// <summary>
        /// The message is providing information only.
        /// </summary>
        Info = 10,
        /// <summary>
        /// The message is a warning.
        /// </summary>
        Warning = 20,
        /// <summary>
        /// The message is an error.
        /// </summary>
        Error = 30,
        /// <summary>
        /// The message is a fatal error.
        /// </summary>
        Fatal = 40
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etilic.Core
{
    /// <summary>
    /// A base class for exceptions in the Etilic framework.
    /// </summary>
    public class EtilicException : Exception
    {
        #region Constructors
        /// <summary>
        /// Initialises a new instance of the <see cref="Etilic.Core.EtilicException"/> class.
        /// </summary>
        public EtilicException()
            : base()
        {
        }
        /// <summary>
        /// Initialises a new instance of the <see cref="Etilic.Core.EtilicException"/> class 
        /// with the specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EtilicException(String message)
            : base(message)
        {
        }
        /// <summary>
        /// Initialises a new instance of the <see cref="Etilic.Core.EtilicException"/> class 
        /// with the specified error message and a reference to the inner exception that is
        /// the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of this exception.</param>
        public EtilicException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        #endregion
    }
}

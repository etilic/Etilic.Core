using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etilic.Core.DAL
{
    /// <summary>
    /// Forces EF to use datetime2 for columns generated from System.DateTime properties, instead of datetime.
    /// </summary>
    public class DateTimeConvention : Convention
    {
        #region Constructors
        /// <summary>
        /// Constructs a new <see cref="System.Data.Entity.ModelConfiguration.Conventions.Convention"/> object which
        /// maps <see cref="System.DateTime"/> objects to the datetime2 type.
        /// </summary>
        public DateTimeConvention()
        {
            this.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }
        #endregion
    }
}

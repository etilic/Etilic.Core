using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Etilic.Core.Migrations;
using System.Data.Entity.Migrations;

namespace Etilic.Core.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class EtilicInitializer : MigrateDatabaseToLatestVersion<EtilicContext, Configuration>
    {
        public override void InitializeDatabase(EtilicContext context)
        {
            base.InitializeDatabase(context);
        }
    }
}

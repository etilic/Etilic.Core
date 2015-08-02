using Etilic.Core.DAL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etilic.Core.Diagnostics
{
    public static class DiagnosticQueue
    {
        #region Static Members
        private static Thread thread = null;
        private static EtilicContext context = null;
        private static ConcurrentQueue<Diagnostic> queue = new ConcurrentQueue<Diagnostic>();
        private static Int32 cached = 0;
        #endregion

        public static Boolean IsLogging
        {
            get;
            set;
        }

        #region Add
        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnostic"></param>
        public static void Add(Diagnostic diagnostic)
        {
            if (diagnostic == null)
                throw new ArgumentNullException("diagnostic");

            if (!IsLogging)
                return;

            queue.Enqueue(diagnostic);
        }
        #endregion

        /// <summary>
        /// Asynchronously starts writing diagnostics to the database.
        /// </summary>
        public static void Start()
        {
            context = new EtilicContext();
            thread = new Thread(ThreadMain);
            thread.IsBackground = true;
            thread.Start();

            IsLogging = true;
        }

        private static void ThreadMain()
        {
            try
            {
                Diagnostic diagnostic = null;

                while (queue.TryDequeue(out diagnostic))
                {
                    context.Diagnostics.Add(diagnostic);
                }

                context.SaveChanges();

                // indicate to the scheduler that we don't need to run right now
                Thread.Sleep(1);
            }
            catch(Exception ex)
            {
                try
                {
                    // try to report the exception
                    Diagnostic diagnostic = new Diagnostic();
                    diagnostic.Level = DiagnosticLevel.Fatal;
                    diagnostic.Message = String.Format("Logging cannot continue: {0}", ex.Message);

                    context.Diagnostics.Add(diagnostic);
                }
                catch { }
            }
            finally
            {
                try
                {
                    // make one last attempt to save changes
                    context.SaveChanges();

                    // then dispose of the connection
                    context.Dispose();
                }
                catch { }
            }
        }
    }
}

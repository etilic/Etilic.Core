using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Etilic.Core.Extensibility
{
    public static class BundleManager
    {
        private static Dictionary<Guid, Bundle> bundles = new Dictionary<Guid,Bundle>();

        #region Properties
        public static IReadOnlyDictionary<Guid, Bundle> Bundles
        {
            get
            {
                return bundles;
            }
        }
        #endregion

        /// <summary>
        /// Registers the specified bundle.
        /// </summary>
        /// <param name="bundle">The bundle to register.</param>
        public static void RegisterBundle(Bundle bundle)
        {
            // we can't register the bundle if it is null
            if (bundle == null)
                throw new ArgumentNullException("bundle");

            // can't register two bundles with the same ID
            if(bundles.ContainsKey(bundle.BundleID))
            {
                throw new InvalidOperationException(String.Format(
                    "A bundle with ID {0} is already registered.", bundle.BundleID));
            }

            // register the bundle
            bundles.Add(bundle.BundleID, bundle);

            bundle.OnAdded();
        }

        #region GetBundle
        /// <summary>
        /// Finds the <see cref="Etilic.Extensibility.Bundle"/> object that has the
        /// specified globally unique ID and then returns it as an object of type
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetBundle<T>(Guid id) where T : class
        {
            return bundles[id] as T;
        }
        /// <summary>
        /// Finds the first <see cref="Etilic.Extensibility.Bundle"/> object that is of
        /// type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of bundle to find.</typeparam>
        /// <returns>
        /// Returns the first <see cref="Etilic.Extensibility.Bundle"/> object that is of
        /// type <typeparamref name="T"/>; otherwise, null.
        /// <returns>
        public static T GetBundle<T>() where T : class
        {
            foreach(Bundle b in bundles.Values)
            {
                if(typeof(T).Equals(b.GetType()))
                    return b as T;
            }

            return null;
        }
        #endregion
    }
}

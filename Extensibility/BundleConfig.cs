using Etilic.Core.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etilic.Core.Extensibility
{
    /// <summary>
    /// A key/value store for a bundle.
    /// </summary>
    public class BundleConfig : IDictionary<String, String>
    {
        #region Instance members
        /// <summary>
        /// The globally unique ID of the bundle.
        /// </summary>
        private Guid bundleID;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the value of the entry with the specified key.
        /// </summary>
        /// <param name="key">The key of the entry whose value should be looked up.</param>
        /// <returns>Returns the value of the entry with the specified key.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">No entry whose key is <paramref name="key"/> exists.</exception>
        public String this[String key]
        {
            get
            {
                // key can't be null
                if (key == null)
                    throw new ArgumentNullException("key");

                using(EtilicContext context = new EtilicContext())
                {
                    // try to get a configuration entry with the specified key
                    BundleConfigEntry entry = context.BundleConfiguration.SingleOrDefault(
                        x => x.BundleID.Equals(this.bundleID) && x.Key.Equals(key));

                    // throw an exception if it does not exist
                    if (entry == null)
                        throw new KeyNotFoundException();

                    // otherwise return the value
                    return entry.Value;
                }
            }
            set
            {
                // key can't be null
                if (key == null)
                    throw new ArgumentNullException("key");

                using(EtilicContext context = new EtilicContext())
                {
                    using(var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            // try to get a configuration entry with the specified key
                            BundleConfigEntry entry = context.BundleConfiguration.SingleOrDefault(
                                x => x.BundleID.Equals(this.bundleID) && x.Key.Equals(key));

                            // create it if it doesn't exist
                            if (entry == null)
                            {
                                entry = new BundleConfigEntry();
                                entry.BundleID = this.bundleID;
                                entry.Key = key;

                                // add the entry to the local set
                                context.BundleConfiguration.Add(entry);
                            }

                            // update the entry
                            entry.Value = value;

                            // save all changes
                            context.SaveChanges();
                            transaction.Commit();
                        }
                        catch(Exception ex)
                        {
                            // roll-back all changes
                            transaction.Rollback();

                            // re-throw the exception
                            throw ex;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gets an <see cref="System.Collections.Generic.ICollection<String>"/> containing the keys of
        /// all configuration entries for the current bundle.
        /// </summary>
        public ICollection<String> Keys
        {
            get
            {
                using (EtilicContext context = new EtilicContext())
                {
                    return context.BundleConfiguration.Where(
                        x => x.BundleID.Equals(this.bundleID)).Select(
                        x => x.Key).ToList();
                }
            }
        }
        /// <summary>
        /// Gets an <see cref="System.Collections.Generic.ICollection<String>"/> containing the values of
        /// all configuration entries for the current bundle.
        /// </summary>
        public ICollection<String> Values
        {
            get 
            {
                using (EtilicContext context = new EtilicContext())
                {
                    return context.BundleConfiguration.Where(
                        x => x.BundleID.Equals(this.bundleID)).Select(
                        x => x.Value).ToList();
                }
            }
        }
        /// <summary>
        /// Gets the number of configuration entries for the current bundle.
        /// </summary>
        public Int32 Count
        {
            get
            {
                using (EtilicContext context = new EtilicContext())
                {
                    return context.BundleConfiguration.Where(x =>
                        x.BundleID.Equals(this.bundleID)).Count();
                }
            }
        }
        /// <summary>
        /// Returns false.
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialises a new instance of the <see cref="Etilic.Core.Extensibility.BundleConfig"/> class
        /// for the bundle with the specified globally unique ID.
        /// </summary>
        /// <param name="bundleID">The globally unique ID of the bundle this configuration store is for.</param>
        public BundleConfig(Guid bundleID)
        {
            this.bundleID = bundleID;
        }
        #endregion

        #region Add
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<String, String> item)
        {
            this.Add(item.Key, item.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(String key, String value)
        {
            // the key cannot be null
            if (key == null)
                throw new ArgumentNullException("key");

            // initialise a database connection
            using(EtilicContext context = new EtilicContext())
            {
                // initialise a transaction to avoid a race condition which would allow
                // a configuration entry with the specified key to be added between
                // testing whether an entry for the specified key exists and adding
                // the new entry
                using(var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // test whether the database contains an entry for this key
                        if (context.BundleConfiguration.Any(x => x.BundleID.Equals(this.bundleID) && x.Key.Equals(key)))
                            throw new ArgumentException("The specified key already exists.");

                        // initialise a new entry for this key, with the specified initial value
                        BundleConfigEntry entry = new BundleConfigEntry();
                        entry.BundleID = this.bundleID;
                        entry.Key = key;
                        entry.Value = value;

                        // add the key to the local collection
                        context.BundleConfiguration.Add(entry);

                        // save changes to the database and commit all changes
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        // roll back the transaction
                        transaction.Rollback();

                        // throw an exception
                        throw new EtilicException(
                            @"Unable to add key to the bundle's configuration because the database transaction failed. See the inner exception for more details.", ex);
                    }
                }
            }
        }
        #endregion

        #region ContainsKey
        /// <summary>
        /// Determines whether the configuration for the current bundle contains an entry with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the configuration for the current bundle.</param>
        /// <returns>Returns true if a configuration entry for the current bundle with the specified name exists.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public Boolean ContainsKey(String key)
        {
            // key can't be null
            if (key == null)
                throw new ArgumentNullException("key");

            using(EtilicContext context = new EtilicContext())
            {
                // see if the database contains a configuration entry for the current
                // bundle with the specified key
                return context.BundleConfiguration.Any(
                    x => x.BundleID.Equals(this.bundleID) && x.Key.Equals(key));
            }
        }
        #endregion

        #region Remove
        /// <summary>
        /// Removes <paramref name="item"/> from the current bundle's configuration.
        /// </summary>
        /// <param name="item">The entry that should be removed.</param>
        /// <returns>Returns true if the entry is successfully removed; otherwise, false.</returns>
        public Boolean Remove(KeyValuePair<String, String> item)
        {
            return this.Remove(item.Key);
        }
        /// <summary>
        /// Removes the entry with the specified key from the current bundle's configuration.
        /// </summary>
        /// <param name="key">The key of the entry to remove.</param>
        /// <returns>Returns true if the entry is successfully removed; otherwise, false.</returns>
        public Boolean Remove(String key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            using(EtilicContext context = new EtilicContext())
            {
                using(var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // try to find the entry with the specified key
                        BundleConfigEntry entry = context.BundleConfiguration.SingleOrDefault(
                            x => x.BundleID.Equals(this.bundleID) && x.Key.Equals(key));

                        // return false if it doesn't exist
                        if (entry == null)
                            return false;

                        // remove the entry from the local set
                        context.BundleConfiguration.Remove(entry);

                        // save all changes
                        context.SaveChanges();
                        transaction.Commit();

                        // the entry has successfully been removed
                        return true;
                    }
                    catch(Exception)
                    {
                        // roll-back all changes
                        transaction.Rollback();

                        // indicate failure (the IDictionary specification doesn't want us to throw an exception)
                        return false;
                    }
                }
            }
        }
        #endregion

        #region TryGetValue
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found; 
        /// otherwise, the default value for the type of the value parameter. 
        /// This parameter is passed uninitialised.</param>
        /// <returns>Returns true if an entry with the specified key exists; otherwise, false.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public Boolean TryGetValue(String key, out String value)
        {
            // key can't be null
            if (key == null)
                throw new ArgumentNullException("key");

            using (EtilicContext context = new EtilicContext())
            {
                // try to find the entry with the specified key
                BundleConfigEntry entry = context.BundleConfiguration.SingleOrDefault(
                    x => x.BundleID.Equals(this.bundleID) && x.Key.Equals(key));

                // return the default String value/false if it doesn't exist
                if (entry == null)
                {
                    value = default(String);
                    return false;
                }

                // return the entry's value if it does
                value = entry.Value;

                // indicate success
                return true;
            }
        }
        #endregion

        #region Clear
        /// <summary>
        /// Removes all entries from the current bundle's configuration.
        /// </summary>
        public void Clear()
        {
            using(EtilicContext context = new EtilicContext())
            {
                using(var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // get all entries belonging to the current bundle 
                        ICollection<BundleConfigEntry> entries = context.BundleConfiguration.Where(
                            x => x.BundleID.Equals(this.bundleID)).ToList();

                        // remove all entries
                        foreach (BundleConfigEntry entry in entries)
                        {
                            context.BundleConfiguration.Remove(entry);
                        }

                        // save all changes
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        // roll-back all changes
                        transaction.Rollback();

                        // re-throw the exception
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region Contains
        /// <summary>
        /// Determines whether the current bundle's configuration contains
        /// an entry with matching key and value.
        /// </summary>
        /// <param name="item">The entry to find.</param>
        /// <returns>Returns true if the entry exists; otherwise, false.</returns>
        public Boolean Contains(KeyValuePair<String, String> item)
        {
            using(EtilicContext context = new EtilicContext())
            {
                BundleConfigEntry entry = context.BundleConfiguration.SingleOrDefault(
                    x => x.BundleID.Equals(this.bundleID) && x.Key.Equals(item.Key) && x.Value.Equals(item.Value));

                return entry != null;
            }
        }
        #endregion

        #region CopyTo
        /// <summary>
        /// Copies the entries of the current bundle's configuration to an <see cref="System.Array"/>, 
        /// starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the elements 
        /// copies from the current bundle's configuration. The <see cref="System.Array"/> must have
        /// zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        public void CopyTo(KeyValuePair<String, String>[] array, Int32 arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex");

            using(EtilicContext context = new EtilicContext())
            {
                // get all entries
                List<BundleConfigEntry> entries = context.BundleConfiguration.Where(
                    x => x.BundleID.Equals(this.bundleID)).ToList();

                // verify that there is enough space in the target array
                if (entries.Count > array.Length - arrayIndex)
                    throw new ArgumentException("The number of configuration entries is greater than the available space from arrayIndex to the end of the desitation array.");

                foreach(BundleConfigEntry entry in entries)
                {
                    // put a key-value pair into the array
                    array[arrayIndex++] = new KeyValuePair<String, String>(entry.Key, entry.Value);
                }
            }
        }
        #endregion

        private class BundleConfigEnumerator : IEnumerator<KeyValuePair<String, String>>, IEnumerator
        {
            private List<BundleConfigEntry> entries;
            private IEnumerator<BundleConfigEntry> enumerator;

            #region Properties
            public KeyValuePair<String, String> Current
            {
                get 
                {
                    return new KeyValuePair<String, String>(
                        this.enumerator.Current.Key,
                        this.enumerator.Current.Value);
                }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }
            #endregion

            #region Constructors
            public BundleConfigEnumerator(List<BundleConfigEntry> entries)
            {
                if (entries == null)
                    throw new ArgumentNullException("entries");

                this.entries = entries;
                this.enumerator = this.entries.GetEnumerator();
            }
            #endregion

            #region Dispose
            public void Dispose()
            {
                this.enumerator.Dispose();
            }
            #endregion

            #region MoveNext
            public Boolean MoveNext()
            {
                return this.enumerator.MoveNext();
            }
            #endregion

            #region Reset
            public void Reset()
            {
                this.enumerator.Reset();
            }
            #endregion
        }

        #region GetEnumerator
        /// <summary>
        /// Returns an enumerator that iteratres through all entries in the current bundle's configuration.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator"/> object that can be used to iteratre through 
        /// all entries in the current bundle's configuration.
        /// </returns>
        public IEnumerator<KeyValuePair<String, String>> GetEnumerator()
        {
            using (EtilicContext context = new EtilicContext())
            {
                List<BundleConfigEntry> entries = context.BundleConfiguration.Where(
                    x => x.BundleID.Equals(this.bundleID)).ToList();

                return new BundleConfigEnumerator(entries);
            }
        }
        #endregion

        #region IEnumerable.GetEnumerator
        /// <summary>
        /// Returns an enumerator that iterates through all entries in the current bundle's configuration.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator"/> object that can be used to iteratre through 
        /// all entries in the current bundle's configuration.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}

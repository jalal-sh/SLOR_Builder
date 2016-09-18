using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetIR.BaseIndexing
{
    /// <summary>
    /// The base class representing the document we are indexing
    /// Note that if you want to serialize the index, the document objects must be serializable
    /// </summary>
    public class Document
    {
        /// <summary>
        /// a Uri that gives a document a unique identifier
        /// </summary>
        public Uri Identifier { get; set; }

        /// <summary>
        /// the path where the document is Cached
        /// </summary>
        public string CachePath { get; set; }

        /// <summary>
        /// The metadata of this Document
        /// </summary>
        public Metadata Metadata { get; protected set; }

        /// <summary>
        /// returns a hash code associated with this document based on its Identifier
        /// </summary>
        /// <returns>a Unique Integer representing the Document</returns>
        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
        /// <summary>
        /// Tests the Equality of two documents based on their idetfiers
        /// </summary>
        /// <param name="obj">another document</param>
        /// <returns>True if the two documents have the same ID, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Document))
                return false;
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}

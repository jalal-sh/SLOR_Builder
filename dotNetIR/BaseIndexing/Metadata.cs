using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetIR.BaseIndexing
{
    /// <summary>
    /// Base class of creating a document metadata to search in
    /// </summary>
    public abstract class Metadata
    {
        /// <summary>
        /// The original Document
        /// </summary>
        public Document Original { get; set; }
        
        /// <summary>
        /// Initializes metadata for Document
        /// This needs to be overriden in derived class in order to extract metadata
        /// </summary>
        /// <param name="doc">The original document</param>
        public Metadata(Document doc)
        {
            Original = doc;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotNetIR.BaseIndexing;
namespace SlorIndexer
{
    /// <summary>
    /// Notions in the Learning Object
    /// </summary>
    public class SemanticMetadata : Metadata
    {
        /// <summary>
        /// List of Notions in a Document
        /// </summary>
        public List<Uri> Notions { get; protected set; }
        /// <summary>
        /// Fills the notions in the Learning object from the database
        /// </summary>
        /// <param name="lo">The Learning Object</param>
        /// <param name="notions">List of Notions in the Learning Object</param>
        public SemanticMetadata(LearningObject lo, List<string> notions) : base(lo)
        {


            Notions = new List<Uri>();
            foreach (var notion in notions)
            {
                Notions.Add(new Uri(notion.NotionUri));
            }


        }
    }
}

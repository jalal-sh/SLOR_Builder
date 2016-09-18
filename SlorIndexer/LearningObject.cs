using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotNetIR.BaseIndexing;
namespace SlorIndexer
{
    /// <summary>
    /// A Learning Object to be Indexed
    /// </summary>
    public class LearningObject : Document
    {
        /// <summary>
        /// Title of the Learning Object
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Description of the Learning Object
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Loads the Learning object from the DB
        /// </summary>
        /// <param name="lo">DB learning Object</param>
        public LearningObject(DBContext.LearningObject lo)
        {
            Title = lo.Title;
            Description = lo.Description;
            Identifier = new Uri(lo.Uri);
            CachePath = lo.ScormPackageLocation;
            Metadata = new SemanticMetadata(this, lo.Notions.Select(c => c.NotionUri).ToList());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace dotNetIR.BaseQuerying
{
    /// <summary>
    /// Class representing the Matched Document
    /// </summary>
    public class MatchedDocument
    {
        /// <summary>
        /// The original Document
        /// </summary>
        public BaseIndexing.Document Doc { get; set; }

        /// <summary>
        /// The score of current document
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Creates a new instance of matching document
        /// </summary>
        /// <param name="Doc">The Document Matched</param>
        /// <param name="Score">The Score of this Document</param>
        public MatchedDocument(BaseIndexing.Document Doc, double Score)
        {
            this.Doc = Doc;
            this.Score = Score;
        }
        /// <summary>
        /// Converts an instance of MatchedDocument to Bare Document
        /// </summary>
        /// <param name="doc">The Matched Document</param>
        public static implicit operator BaseIndexing.Document(MatchedDocument doc)
        {
            return doc.Doc;
        }
    }

}

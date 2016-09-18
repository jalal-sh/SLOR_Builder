using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetIR.BaseQuerying
{
    /// <summary>
    /// Calculates the Score of a Document in an index according to a specific search criteria
    /// </summary>
    public interface IScorer
    {
        /// <summary>
        /// Calculates the Score of a Document in an index according to a specific search criteria
        /// </summary>
        /// <param name="processedUserQuery">The processed user query</param>
        /// <param name="index">the index in which the search is performed</param>
        /// <param name="document">the doument we want to score</param>
        /// <returns>The Score of the Document</returns>
        double GetScoreOfDocument(object processedUserQuery, BaseIndexing.Index index, BaseIndexing.Document document);
    }
}

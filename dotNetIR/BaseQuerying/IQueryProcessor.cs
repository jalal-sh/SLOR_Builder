 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetIR.BaseQuerying
{
    /// <summary>
    /// Interface to process a user Query
    /// </summary>
    public interface IQueryProcessor
    {
        /// <summary>
        /// Method to process User Query
        /// </summary>
        /// <param name="userQuery">User query input</param>
        /// <returns>The processed User query as an object</returns>
        /// <example>
        /// If your query processing is only to segment and stem the input:
        /// ProcessQuery("Weather on Monday"); //returns a list of strings {"Weather","Monday"}  
        /// </example>
        object ProcessQuery(string userQuery);
    }
}
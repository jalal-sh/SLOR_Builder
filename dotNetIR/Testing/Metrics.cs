using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetIR.Testing
{
    /// <summary>
    /// Provides Metrics to measure the qualit yof an IR system
    /// </summary>
    public sealed class Metrics
    {
        /// <summary>
        /// Precision of the System
        /// </summary>
        /// <remarks>TP/(TP+FP)</remarks>
        public double Precision { get { return (double)TP / (TP + FP); } }
        /// <summary>
        /// Recall of the System
        /// </summary>
        /// <remarks>TP/(TP+FN)</remarks> 
        public double Recall { get { return (double)TP / (TP + FN); } }
        /// <summary>
        /// F-Measure Metric
        /// </summary>
        /// <remarks> Precision*Recall / (Precision+Recall)</remarks>
        public double FMeasure { get { return Precision * Recall / (Precision + Recall); } }
        /// <summary>
        /// Number of True Positives
        /// </summary>
        public int TP { get; private set; }
        /// <summary>
        /// Number of True Negatives
        /// </summary>
        public int TN { get; private set; }
        /// <summary>
        /// Number of False Positives
        /// </summary>
        public int FP { get; private set; }
        /// <summary>
        /// Number of False Negatives
        /// </summary>
        public int FN { get; private set; }
        private bool[,] queryDocumentResult;
        /// <summary>
        /// Did the Query return that Document
        /// </summary>
        /// <param name="q">User Query</param>
        /// <param name="doc">Document</param>
        /// <returns>True if the query returned the document false otherwise</returns>
        public bool this[string q, BaseIndexing.Document doc]
        {
            get
            {
                if (queriesIndex.ContainsKey(q) && docIndex.ContainsKey(doc))
                    return queryDocumentResult[queriesIndex[q], docIndex[doc]];
                return false;
            }
            private set
            {
                queryDocumentResult[queriesIndex[q], docIndex[doc]] = value;
            }
        }
        private Dictionary<BaseIndexing.Document, int> docIndex;
        private Dictionary<string, int> queriesIndex;
        private Metrics()
        { }
        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="allQuerys">List of unique queries that will be used to test the system</param>
        /// <param name="indexSize">the size of the index that will be tested</param>
        internal Metrics(List<string> allQuerys, int indexSize)
        {
            IndexSize = indexSize;

            AllQuerys = allQuerys;
            queryDocumentResult = new bool[allQuerys.Count, indexSize];
            docIndex = new Dictionary<BaseIndexing.Document, int>();
            queriesIndex = new Dictionary<string, int>();
            for (int i = 0; i < queryDocumentResult.GetLength(0); i++)
            {
                for (int j = 0; j < queryDocumentResult.GetLength(1); j++)
                {
                    queryDocumentResult[i, j] = false;
                }
            }
            TP = FP = TN = FN = 0;

        }

        private int curQ = 0, curDoc = 0;
        /// <summary>
        /// Informs this class what a particular query has returned and what was it expected to return
        /// </summary>
        /// <param name="query">Particular user query</param>
        /// <param name="retrievedDocs">the docs returned by this query</param>
        /// <param name="expectedDocs">the docs that this query is expected to return</param>
        internal void DocumentsByQuery(string query, BaseQuerying.MatchedDocument[] retrievedDocs, HashSet<BaseIndexing.Document> expectedDocs)
        {
            int curFP = 0, curTP = 0;
            foreach (var rdoc in retrievedDocs)
            {
                BaseIndexing.Document doc = rdoc;
                if (!queriesIndex.ContainsKey(query))
                {
                    queriesIndex[query] = curQ++;
                }
                if (!docIndex.ContainsKey(doc))
                    docIndex[doc] = curDoc++;
                this[query, doc] = true;

                if (expectedDocs.Contains(doc))
                {
                    curTP++;
                }
                else
                {
                    curFP++;
                }
            }
            int curFN = expectedDocs.Count - curTP;
            int curTN = IndexSize - curTP - curFP - curFN;
            TP += curTP;
            FP += curFP;
            TN += curTN;
            FN += curFN;
        }
        /// <summary>
        /// List of all queries that might be used to test the system
        /// </summary>
        public List<string> AllQuerys { get; private set; }
        /// <summary>
        /// Number of Docs in the index being tested
        /// </summary>
        public int IndexSize { get; private set; }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetIR.Testing
{
    /// <summary>
    /// This class is to be used inside any test project that uses the framework
    /// </summary>
    public class TestUtility
    {
        /// <summary>
        /// The index to be tested
        /// </summary>
        public BaseIndexing.Index Index { get; set; }
        /// <summary>
        /// The query processor to be tested
        /// </summary>
        public BaseQuerying.IQueryProcessor QProcessor { get; set; }
        /// <summary>
        /// The scorer to be tested
        /// </summary>
        public BaseQuerying.IScorer Scorer { get; set; }

        /// <summary>
        /// Constructor of the Test Utility Class
        /// </summary>
        /// <param name="index">The index that will be tested</param>
        /// <param name="queryProcessor">The Query Processor to be Tested</param>
        /// <param name="scorer">The scorer to be Tested</param>
        public TestUtility(BaseIndexing.Index index, BaseQuerying.IQueryProcessor queryProcessor, BaseQuerying.IScorer scorer)
        {
            Index = index;
            QProcessor = queryProcessor;
            Scorer = scorer;
        }
        /// <summary>
        /// Tests the query procesor
        /// to Be correctly tested the type returned by that query processor must override Equals()
        /// </summary>
        /// <param name="query">The user Query</param>
        /// <param name="expectedProcessed">Expected Output from the query processor</param>
        /// <returns>True if the result matches the expected result, false otherwise</returns>
        public bool QueryProcessorTesting(string query, object expectedProcessed)
        {
            return expectedProcessed == QProcessor.ProcessQuery(query);
        }
        /// <summary>
        /// Tests the scorer based on a preprocessed user query
        /// </summary>
        /// <param name="processedQuery">Preprocessed user Query</param>
        /// <param name="document">The document on which we want to calculate the score</param>
        /// <param name="expectedScore">the Expected Score of this document in the index</param>
        /// <returns>True if the result matches the expected result, false otherwise</returns>
        public bool ScorerTest(object processedQuery, BaseIndexing.Document document, double expectedScore)
        {
            return Scorer.GetScoreOfDocument(processedQuery, Index, document) == expectedScore;
        }
        /// <summary>
        ///Tests the scorer based on a unprocessed user query - note that this will test both the Scorer and the query processor
        /// </summary>
        /// <param name="query">unprocessed user Query</param>
        /// <param name="document">The document on which we want to calculate the score</param>
        /// <param name="expectedScore">the Expected Score of this document in the index</param>
        /// <returns>True if the result matches the expected result, false otherwise</returns>
        public bool ScorerTest(string query, BaseIndexing.Document document, double expectedScore)
        {
            return Scorer.GetScoreOfDocument(QProcessor.ProcessQuery(query), Index, document) == expectedScore;
        }
        /// <summary>
        /// Gets the <see cref="Metrics"/> of the current IR system 
        /// </summary>
        /// <param name="expectedDocumentsByQuery">A Dictionary that maps each query to a HashSet of Documents expeted to be returned by it</param>
        /// <param name="scoreThres">The threshold that if a document recieved would be considered accepted and returned by the Matcher</param>
        /// <returns>An instance of the <see cref="Metrics"/> calss </returns>
        public Metrics getMetrics(Dictionary<string, HashSet<BaseIndexing.Document>> expectedDocumentsByQuery, double scoreThres = 0.5)
        {

            Metrics metrics = new Metrics(expectedDocumentsByQuery.Keys.ToList(), Index.CurrentDocCount);
            foreach (var expectedDocs in expectedDocumentsByQuery)
            {
                string query = expectedDocs.Key;
                BaseQuerying.MatchedDocument[] matchedDocs = Index.Matcher(QProcessor.ProcessQuery(query), Scorer, scoreThres);
                metrics.DocumentsByQuery(query, matchedDocs, expectedDocs.Value);
            }
            return metrics;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotNetIR.BaseQuerying;

namespace dotNetIR.BaseIndexing
{
    /// <summary>
    /// Base Index Class - No Serialization Predefined
    /// </summary>
    public abstract class Index
    {
        /// <summary>
        /// Array of Documents in Index
        /// </summary>
        protected Document[] Docs;

        /// <summary>
        /// Initializes the Documents Set
        /// </summary>
        /// <param name="MaxSize">Maximum number of documents the index may have</param>
        public Index(int MaxSize)
        {
            Docs = new Document[MaxSize];
        }
        /// <summary>
        /// A dictionary to find the index of a doc in the <see cref="Docs"/> array
        /// </summary>
        protected Dictionary<Uri, int> docMapper;

        /// <summary>
        /// Returns the document with the speicfic identifier
        /// </summary>
        /// <param name="DocUri">Identifies a Single Document</param>
        /// <returns>The document with the specified Uri</returns>
        public Document getDocumentByUri(Uri DocUri)
        {
            if (docMapper.ContainsKey(DocUri))
                return Docs[docMapper[DocUri]];
            return null;
        }

        /// <summary>
        /// The current Size of index in terms of the documents count
        /// </summary>
        public int CurrentDocCount { get; protected set; }

        /// <summary>
        /// Adds the document ot the docs array and mapps its uri to its location
        /// </summary>
        /// <param name="newDoc">The Document to be Added</param>
        protected void AddDocToRepo(Document newDoc)
        {
            Docs[CurrentDocCount] = newDoc;
            docMapper[newDoc.Identifier] = CurrentDocCount;
            CurrentDocCount++;
        }

        /// <summary>
        /// Adds a new Document to the index
        /// Calls AddDocToRepo then  calls the DocIndexingHandler
        /// </summary>
        /// <param name="newDoc">The new Document to be Added</param>
        public void AddDoc(Document newDoc)
        {
            AddDocToRepo(newDoc);
            DocIndexingHandler(newDoc);
        }

        /// <summary>
        /// Handles what happens when a new Document is added to the index 
        /// </summary>
        /// <param name="newDoc">New Document being indexed</param>
        protected abstract void DocIndexingHandler(Document newDoc);


        /// <summary>
        /// The Default Matcher to Search the index
        /// </summary>
        ///<param name="processedUserQuery">The User Query after being processed</param>
        /// <param name="scorer">an instance of IScorer to claculate the score of each document</param>
        /// <param name="scoreThresh"> a threshold to filter out documents below it (default is 0 returns all matched documents)</param>
        /// <returns>an array of Matched documents</returns>
        public virtual MatchedDocument[] Matcher(object processedUserQuery, IScorer scorer, double scoreThresh = 0.5)
        {
            List<MatchedDocument> docs = new List<MatchedDocument>();
            foreach (var item in Docs)
            {
                double score = scorer.GetScoreOfDocument(processedUserQuery, this, item);
                if (score >= scoreThresh)
                    docs.Add(new MatchedDocument(item, score));
            }
            return docs.ToArray();
        }

        /// <summary>
        /// Counts the number of Matched Documents for a specific query
        /// </summary>
        ///<param name="processedUserQuery">The User Query after being processed</param>
        /// <param name="scorer">an instance of IScorer to claculate the score of each document</param>
        /// <param name="scoreThresh"> a threshold to filter out documents below it (default is 0 returns all matched documents)</param>
        /// <returns>Number of Matched Document for the query</returns>
        public virtual int MatchesCount(object processedUserQuery, IScorer scorer, double scoreThresh = 0.5)
        {
            int matches = 0;
            foreach (var item in Docs)
            {
                double score = scorer.GetScoreOfDocument(processedUserQuery, this, item);
                if (score >= scoreThresh)
                    matches++;
            }
            return matches;
        }
    }
}

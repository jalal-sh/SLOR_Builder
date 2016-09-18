using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotNetIR.BaseIndexing;
namespace SlorIndexer
{
    /// <summary>
    /// The index for the Semantic Learning Object Repository SLOR
    /// </summary>
    public class SlorIndex : Index
    {
        /// <summary>
        /// Loads the index from database
        /// </summary>
        /// <param name="MaxLOs">Maximum number of Learning objects that could be indexed</param>
        public SlorIndex(int MaxLOs) : base(MaxLOs)

        {
            try
            {
                using (DBContext.SLOREntities context = new DBContext.SLOREntities())
                {
                    foreach (var lo in context.LearningObjects)
                    {
                        AddDoc(new LearningObject(lo));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logger.LogException(ex, "SlorIndex initialization");
            }
        }
        /// <summary>
        /// Maps a notion to the indices of the documents containing this notion
        /// </summary>
        protected Dictionary<Uri, List<int>> map;
        /// <summary>
        ///  Retrieves the Learning objects that have a particular notion
        /// </summary>
        /// <param name="notion">The notion to search for</param>
        /// <returns>List of Learning objects that have this notion</returns>
        public List<LearningObject> getLearningObjectsByNotion(Uri notion)
        {
            List<LearningObject> res = new List<LearningObject>();
            map[notion].ForEach((x) =>
            {
                res.Add((LearningObject)Docs[x]);
            });
            return res;
        }
        private int curID = 0;
        /// <summary>
        /// Adds the learning object to the map of notions
        /// </summary>
        /// <param name="learningObject">The learning Object to Add</param>
        protected override void DocIndexingHandler(Document learningObject)
        {
            LearningObject lo = learningObject as LearningObject;
            foreach (var notion in (lo.Metadata as SemanticMetadata).Notions)
            {
                if (!map.ContainsKey(notion))
                    map[notion] = new List<int>();
                map[notion].Add(curID++);
            }
        }
    }
}

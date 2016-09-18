using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
namespace LomEditorUtils
{
    /// <summary>
    /// An utility class to help Editing LOM
    /// </summary>
    public static class LomEditor
    {
        /// <summary>
        /// Lists all Notions in a specific LOM Document
        /// </summary>
        /// <param name="lomDocument">The LOM Document to List the Notions from</param>
        /// <returns>List of all Notions in the specified LOM Document</returns>
        public static List<string> ListNotions(XDocument lomDocument)
        {
            XElement notions;
            if (lomDocument.Elements().ElementAt(0).Elements().Count(c => c.Name.LocalName == "Notions") == 0)
            {
                return new List<string>();

            }
            notions = (from XElement x in lomDocument.Elements().ElementAt(0).Elements()
                       where x.Name.LocalName == "Notions"
                       select x).ElementAt(0);
            return (from XElement x in notions.Elements() select x.Value).ToList();
        }
        /// <summary>
        /// Adds a new Notion the LOM Document
        /// </summary>
        /// <param name="notion">The Notion to Add provided as a Unique Resource Identifier(URI)</param>
        /// <param name="lomDocument">The Document to add the Notion to</param>
        public static void AddNotion(string notion, XDocument lomDocument)
        {
            XElement notionElement = new XElement("Notion", notion);
            XElement notions;
            if (lomDocument.Elements().ElementAt(0).Elements().Count(c => c.Name.LocalName == "Notions") == 0)
            {
                notions = new XElement("Notions", "");
                lomDocument.Elements().ElementAt(0).Add(notions);
            }
            else
            {
                notions = (from XElement x in lomDocument.Elements().ElementAt(0).Elements()
                           where x.Name.LocalName == "Notions"
                           select x).ElementAt(0);
            }
            notions.Add(notionElement);
        }
        /// <summary>
        /// Writes back the LOM XDocument to the file
        /// </summary>
        /// <param name="lom">XDocument represneting LOM</param>
        /// <param name="scormFileLocation">The SCORM Package</param>
        /// <param name="lomFileName">Name of LOM file inside the SCORM package</param>
        public static void WriteBackLom(XDocument lom, string scormFileLocation, string lomFileName)
        {
            string tempFile = Path.GetTempFileName();
            lom.Save(tempFile);
            ParsedScorm.UpdateFileInScorm(scormFileLocation, lomFileName, tempFile);
        }
    }
}
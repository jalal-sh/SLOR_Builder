using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using System.Xml.Linq;
using System.IO;
using static Logger.Logger;
namespace LomEditorUtils
{
    /// <summary>
    /// Parses SCORM package files in purpose of managing LOM
    /// </summary>
    public class ParsedScorm
    {
        /// <summary>
        /// Update a file in the SCORM package
        /// </summary>
        /// <param name="scormFile">The path to the SCORM package</param>
        /// <param name="oldFile">The Name of File to Update in the SCROM package</param>
        /// <param name="newFile">The path to File to replace the Old file in the SCORM package</param>
        public static void UpdateFileInScorm(string scormFile, string oldFile, string newFile)
        {
            try
            {
                using (ZipFile scormZip = ZipFile.Read(scormFile))
                {
                    ZipEntry entry = scormZip[oldFile];

                    scormZip.UpdateEntry(oldFile, new FileStream(newFile, FileMode.Open));

                    scormZip.Save();
                }
            }
            catch (Exception e)
            {
                LogException(e);
                throw;
            }
        }
        private ParsedScorm()
        {
            LomFiles = new List<XDocument>();
            ManifestDoc = new XDocument();
            LomFilesLocations = new List<string>();
        }
        /// <summary>
        /// XDocument representing the SCORM package manifest File
        /// </summary>
        public XDocument ManifestDoc { get; set; }
        /// <summary>
        /// List of all LOM files inside the SCORM package
        /// </summary>
        public List<string> LomFilesLocations { get; set; }
        /// <summary>
        /// List of XDocuments representing the LOM files inside the SCORM package
        /// </summary>
        public List<XDocument> LomFiles { get; set; }
        /// <summary>
        /// Parses a SCORM file and returns an object represnting the parsed SCORM file in purpose of LOM files Editing
        /// </summary>
        /// <param name="filePath">Location of SCORM package on disk</param>
        /// <returns>Parsed SCORM package</returns>
        /// <exception cref="Exception">SCORM package not found on Disk</exception> 
        public static ParsedScorm ParseScorm(string filePath)
        {
            try
            {
                ParsedScorm scorm = new ParsedScorm();
                using (ZipFile scormFile = ZipFile.Read(filePath))
                {
                    ZipEntry manifestEntry = scormFile["imsmanifest.xml"];
                    using (StreamReader reader = new StreamReader(manifestEntry.OpenReader()))
                        scorm.ManifestDoc = XDocument.Parse(reader.ReadToEnd());
                    var metas = (from XElement y in scorm.ManifestDoc.Elements().ElementAt(0).Elements() where y.Name.LocalName == "metadata" select y);
                    scorm.LomFilesLocations = (from XElement x in metas.Elements()
                                               where x.Name.LocalName == "location"
                                               select x.Value).ToList();
                    for (int i = 0; i < scorm.LomFilesLocations.Count; i++)
                    {
                        ZipEntry lomEntry = scormFile[scorm.LomFilesLocations[0]];
                        using (StreamReader reader = new StreamReader(manifestEntry.OpenReader()))
                        {
                            scorm.LomFiles.Add(XDocument.Parse(reader.ReadToEnd()));
                        }
                    }
                }

                return scorm;
            }
            catch (Exception e)
            {
                LogException(e);
                throw;
            }
        }
    }
}

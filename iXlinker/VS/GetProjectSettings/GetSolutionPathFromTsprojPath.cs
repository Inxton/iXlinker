using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TwincatXmlSchemas.TcSmProject;
using Microsoft.Build.Construction;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        private static string GetSolutionPathFromTsprojPath(string tsProjFilePath)
        {
            string slnPath = null;
            string tsProjGUID= null;
            if (File.Exists(tsProjFilePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(tsProjFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(TcSmProject));
                        TcSmProject Tc = (TcSmProject)serializer.Deserialize(reader);
                        if (Tc != null && Tc.Project != null && Tc.Project.ProjectGUID != null)
                        {
                            tsProjGUID = Tc.Project.ProjectGUID;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    Console.ReadLine();
                }

                if (tsProjGUID != null)
                {
                    string dir = tsProjFilePath.Substring(0, tsProjFilePath.LastIndexOf("\\"));
                    bool endSearch = false;
                    while(!endSearch)
                    {
                        List<string> slnfFiles = new List<string>();
                        List<string> slnFiles = new List<string>();
                        foreach (string file in Directory.GetFiles(dir))
                        {
                            if (file.EndsWith(".slnf"))
                            {
                                slnfFiles.Add(file);
                            }
                            if (file.EndsWith(".sln"))
                            {
                                slnFiles.Add(file);
                            }
                        }
                        if (slnfFiles.Count >= 1)
                        {
                            foreach(string slnfFile in slnfFiles)
                            {
                                var slnf = SolutionFile.Parse(slnfFile);
                                if (slnf != null && slnf.ProjectsByGuid != null)
                                {
                                    foreach (KeyValuePair<string, ProjectInSolution> project in slnf.ProjectsByGuid)
                                    {
                                        if (project.Key.Contains(tsProjGUID))
                                        {
                                            slnPath = slnfFile;
                                            endSearch = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (slnFiles.Count >= 1)
                        {
                            foreach (string slnFile in slnFiles)
                            {
                                var sln = SolutionFile.Parse(slnFile);
                                if(sln != null && sln.ProjectsByGuid != null)
                                {
                                    foreach(KeyValuePair<string, ProjectInSolution> project in sln.ProjectsByGuid)
                                    {
                                        if (project.Key.Contains(tsProjGUID))
                                        {
                                            slnPath = slnFile;
                                            endSearch = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (!endSearch)
                        {
                            dir = Path.GetFullPath(Path.Combine(dir, @"..\"));
                            if(dir == Path.GetPathRoot(dir))
                            {
                                slnPath = null;
                                endSearch = true;
                                break;
                            }
                        }

                    }

                }
            }
            return slnPath;
        }
    }
}

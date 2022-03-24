using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using iXlinkerDtos;
using TwincatXmlSchemas.TcPlcProj;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void GetPlcLibraries(Solution vs)
        {
            string plcProjPath = vs.PlcProject.CompletePathInFileSystem;
            XmlSerializer serializer = new XmlSerializer(typeof(Project));
            StreamReader reader = new StreamReader(plcProjPath);
            Project plcProject = (Project)serializer.Deserialize(reader);
            reader.Close();

            foreach (ProjectItemGroup projectItemGroup in plcProject.ItemGroup)
            {
                if (projectItemGroup != null && projectItemGroup.PlaceholderReference != null)
                {
                    foreach (ProjectItemGroupPlaceholderReference projectItemGroupPlaceholderReference in projectItemGroup.PlaceholderReference)
                    {
                        string[] defaultResolution = projectItemGroupPlaceholderReference.DefaultResolution.Split(',', '(');
                        if (defaultResolution.Length == 3)
                        {
                            PlcLibrary plcLibrary = new PlcLibrary()
                            {
                                Title = defaultResolution[0],
                                Version = defaultResolution[1],
                                CompanyName = defaultResolution[2].Replace(")", ""),
                                NameSpace = projectItemGroupPlaceholderReference.Namespace,
                                PlaceHolder = projectItemGroupPlaceholderReference.Include
                            };

                            if (plcLibrary.Version.Contains("*"))
                            {
                                foreach (PlcLibRepository plcLibRepository in vs.PlcLibRepositories)
                                {
                                    string _path = (plcLibRepository.RepositoryPath + "\\" + plcLibrary.CompanyName + "\\" + plcLibrary.Title).Replace("\\\\", "\\");
                                    if (Directory.Exists(_path))
                                    {
                                        string[] directories = Directory.GetDirectories(_path);
                                        if (directories.Length > 1)
                                        {
                                            directories = directories.OrderByDescending(d => d).ToArray();
                                        }
                                        plcLibrary.EffectiveVersion = directories[0].Substring(directories[0].LastIndexOf("\\") + 1);
                                        plcLibrary.Path = directories[0];
                                        PlcLibraries.Add(plcLibrary);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                foreach (PlcLibRepository plcLibRepository in vs.PlcLibRepositories)
                                {
                                    plcLibrary.EffectiveVersion = plcLibrary.Version;
                                    string _path = (plcLibRepository.RepositoryPath + "\\" + plcLibrary.CompanyName + "\\" + plcLibrary.Title + "\\" + plcLibrary.EffectiveVersion).Replace("\\\\", "\\");
                                    if (Directory.Exists(_path))
                                    {
                                        plcLibrary.Path = _path;
                                        PlcLibraries.Add(plcLibrary);
                                        break;
                                    }
                                }
                            }


                        }
                    }
                }
            }
        }
    }
}

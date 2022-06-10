using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using iXlinkerDtos;
using TwincatXmlSchemas.TcPlcProj;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void GetPlcLibraries(Solution vs)
        {
            string plcProjPath = vs.PlcProject.Plcproj.CompletePathInFileSystem;
            XmlSerializer serializer = new XmlSerializer(typeof(Project));
            StreamReader reader = new StreamReader(plcProjPath);
            Project plcProject = (Project)serializer.Deserialize(reader);
            reader.Close();

            //Discover Plc libraries
            foreach (ProjectItemGroup projectItemGroup in plcProject.ItemGroup)
            {
                if (projectItemGroup != null && projectItemGroup.PlaceholderReference != null)
                {
                    //Collect all libraries
                    foreach (ProjectItemGroupPlaceholderReference projectItemGroupPlaceholderReference in projectItemGroup.PlaceholderReference)
                    {
                        string[] defaultResolution = projectItemGroupPlaceholderReference.DefaultResolution.Split(',', '(');
                        if (defaultResolution.Length == 3)
                        {
                            PlcLibrary plcLibrary = new PlcLibrary()
                            {
                                Title = defaultResolution[0],
                                Version = defaultResolution[1].Replace(" ",""),
                                CompanyName = defaultResolution[2].Replace(")", ""),
                                Namespace = projectItemGroupPlaceholderReference.Namespace,
                                PlaceHolder = projectItemGroupPlaceholderReference.Include
                            };

                            PlcLibraries.Add(plcLibrary);
                        }
                    }
                }
                if (projectItemGroup != null && projectItemGroup.PlaceholderResolution != null)
                {
                    //Collect all exact library type if defined
                    foreach (ProjectItemGroupPlaceholderResolution projectItemGroupPlaceholderResolution in projectItemGroup.PlaceholderResolution)
                    {
                        string[] Resolution = projectItemGroupPlaceholderResolution.Resolution.Split(',', '(');
                        if (Resolution.Length == 3)
                        {
                            PlcLibrary plcLibraryWithDefinedVersion = new PlcLibrary()
                            {
                                Title = Resolution[0],
                                Version = Resolution[1].Replace(" ", ""),
                                CompanyName = Resolution[2].Replace(")", ""),
                                PlaceHolder = projectItemGroupPlaceholderResolution.Include
                            };

                            foreach (PlcLibrary plcLibrary in PlcLibraries)
                            {
                                if (plcLibrary.Title.Equals(plcLibraryWithDefinedVersion.Title) &&
                                    plcLibrary.CompanyName.Equals(plcLibraryWithDefinedVersion.CompanyName) &&
                                    plcLibrary.PlaceHolder.Equals(plcLibraryWithDefinedVersion.PlaceHolder))
                                {
                                    plcLibrary.Version = plcLibraryWithDefinedVersion.Version;
                                }
                            }
                        }
                    }
                }
            }
            //Discover Plc library paths
            foreach (PlcLibrary plcLibrary in PlcLibraries)
            {
                Version _highestVersion = null;
                string _pathOfTheHighestVersion = null;

                if (plcLibrary.Version.Contains("*"))
                {
                    foreach (PlcLibRepository plcLibRepository in vs.PlcLibRepositories)
                    {
                        string _path = (plcLibRepository.RepositoryPath + "\\" + plcLibrary.CompanyName + "\\" + plcLibrary.Title).Replace("\\\\", "\\");
                        if (Directory.Exists(_path))
                        {
                            string[] directories = Directory.GetDirectories(_path);
                            foreach (string directory in directories)
                            {
                                Version version = Conversion.StringToVersion(directory.Substring(directory.LastIndexOf("\\") + 1));
                                if (_highestVersion == null)
                                {
                                    _highestVersion = version;
                                    _pathOfTheHighestVersion = directory;
                                }
                                else if (version > _highestVersion)
                                {
                                    _highestVersion = version;
                                    _pathOfTheHighestVersion = directory;
                                }
                            }
                        }
                    }
                    plcLibrary.EffectiveVersion = _highestVersion.ToString();
                    plcLibrary.Path = _pathOfTheHighestVersion;
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
                            //break;
                        }
                    }
                }
            }
        }
    }
}

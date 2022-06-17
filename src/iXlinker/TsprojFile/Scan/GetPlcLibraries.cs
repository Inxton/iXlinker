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
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Project));
                StreamReader reader = new StreamReader(plcProjPath);
                Project plcProject = (Project)serializer.Deserialize(reader);
                reader.Close();

                //Discover Plc libraries
                try
                {
                    foreach (ProjectItemGroup projectItemGroup in plcProject.ItemGroup)
                    {
                        if (projectItemGroup != null && projectItemGroup.PlaceholderReference != null)
                        {
                            //Collect all libraries
                            foreach (ProjectItemGroupPlaceholderReference projectItemGroupPlaceholderReference in projectItemGroup.PlaceholderReference)
                            {
                                string details = "";
                                try
                                {
                                    details = "Include: " + (projectItemGroupPlaceholderReference.Include != null ? projectItemGroupPlaceholderReference.Include : "???") + " | "
                                                   + "Namespace: " + (projectItemGroupPlaceholderReference.Namespace != null ? projectItemGroupPlaceholderReference.Namespace : "???") + " | "
                                                   + "DefaultResolution: " + (projectItemGroupPlaceholderReference.DefaultResolution != null ? projectItemGroupPlaceholderReference.DefaultResolution : "???");

                                    string[] defaultResolution = projectItemGroupPlaceholderReference.DefaultResolution.Split(',', '(');
                                    if (defaultResolution.Length == 3)
                                    {
                                        if(projectItemGroupPlaceholderReference.Namespace != null)
                                        {
                                            if (projectItemGroupPlaceholderReference.Include != null)
                                            {
                                                PlcLibrary plcLibrary = new PlcLibrary()
                                                {
                                                    Title = defaultResolution[0],
                                                    Version = defaultResolution[1].Replace(" ", ""),
                                                    CompanyName = defaultResolution[2].Replace(")", ""),
                                                    Namespace = projectItemGroupPlaceholderReference.Namespace,
                                                    PlaceHolder = projectItemGroupPlaceholderReference.Include
                                                };

                                                PlcLibraries.Add(plcLibrary);
                                            }
                                            else
                                            {
                                                EventLogger.Instance.Logger.Error(@"Unable to discover Include for the Plc library: " + details
                                                     + Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<ProjectItemGroupPlaceholderReference>(x => x.Include) + " has an invalid reference!!!"
                                                     + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine);
                                            }
                                        }
                                        else
                                        {
                                            EventLogger.Instance.Logger.Error(@"Unable to discover Namespace for the Plc library: " + details
                                                 + Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<ProjectItemGroupPlaceholderReference>(x => x.Namespace) + " has an invalid reference!!!"
                                                 + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine);
                                        }

                                    }
                                    else
                                    {
                                        EventLogger.Instance.Logger.Error(@"Unable to discover default resolution for the Plc library: " + details
                                             + Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<ProjectItemGroupPlaceholderReference>(x => x.DefaultResolution) + " has the value of: "+ projectItemGroupPlaceholderReference.DefaultResolution.ToString() + "!!!"
                                             + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EventLogger.Instance.Logger.Error(@"Unable to discover default resolution for the Plc library: " + details 
                                         + Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<ProjectItemGroupPlaceholderReference>(x => x.DefaultResolution) + " has an invalid reference!!!"
                                         + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                                }
                            }
                        }
                        if (projectItemGroup != null && projectItemGroup.PlaceholderResolution != null)
                        {
                            //Collect all exact library type if defined
                            foreach (ProjectItemGroupPlaceholderResolution projectItemGroupPlaceholderResolution in projectItemGroup.PlaceholderResolution)
                            {
                                string details = "";
                                try
                                {
                                    details = "Include: " + (projectItemGroupPlaceholderResolution.Include != null ? projectItemGroupPlaceholderResolution.Include : "???") + " | "
                                                   + "Resolution: " + (projectItemGroupPlaceholderResolution.Resolution != null ? projectItemGroupPlaceholderResolution.Resolution : "???");
                                    string[] Resolution = projectItemGroupPlaceholderResolution.Resolution.Split(',', '(');
                                    if (Resolution.Length == 3)
                                    {
                                        if (projectItemGroupPlaceholderResolution.Include != null)
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
                                                if (plcLibrary.Title != null && plcLibrary.CompanyName != null && plcLibrary.PlaceHolder != null)
                                                {
                                                    if (plcLibrary.Title.Equals(plcLibraryWithDefinedVersion.Title) &&
                                                        plcLibrary.CompanyName.Equals(plcLibraryWithDefinedVersion.CompanyName) &&
                                                        plcLibrary.PlaceHolder.Equals(plcLibraryWithDefinedVersion.PlaceHolder))
                                                    {
                                                        //Assign the exact library version
                                                        plcLibrary.Version = plcLibraryWithDefinedVersion.Version;
                                                        plcLibraryWithDefinedVersion = null;
                                                        break;
                                                    }
                                                 }
                                                else
                                                {
                                                    details = "Title: " + (plcLibraryWithDefinedVersion.Title != null ? plcLibraryWithDefinedVersion.Title : "???") + " | "
                                                            + "Version: " + (plcLibraryWithDefinedVersion.Version != null ? plcLibraryWithDefinedVersion.Version : "???") + " | "
                                                            + "CompanyName: " + (plcLibraryWithDefinedVersion.CompanyName != null ? plcLibraryWithDefinedVersion.CompanyName : "???") + " | "
                                                            + "PlaceHolder: " + (plcLibraryWithDefinedVersion.PlaceHolder != null ? plcLibraryWithDefinedVersion.PlaceHolder : "???");

                                                    EventLogger.Instance.Logger.Error(@"Library with some invalid library details found: " + details
                                                         + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine);
                                                }
                                            }
                                            //Library to which the exact version needs to be assigned has not been found
                                            if(plcLibraryWithDefinedVersion != null)
                                            {
                                                details = "Title: " + (plcLibraryWithDefinedVersion.Title != null ? plcLibraryWithDefinedVersion.Title : "???") + " | "
                                                        + "Version: " + (plcLibraryWithDefinedVersion.Version != null ? plcLibraryWithDefinedVersion.Version : "???") + " | "
                                                        + "CompanyName: " + (plcLibraryWithDefinedVersion.CompanyName != null ? plcLibraryWithDefinedVersion.CompanyName : "???") + " | "
                                                        + "PlaceHolder: " + (plcLibraryWithDefinedVersion.PlaceHolder != null ? plcLibraryWithDefinedVersion.PlaceHolder : "???");

                                                EventLogger.Instance.Logger.Error(@"Unable to set the version for the library: " + details
                                                     + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine);
                                            }
                                        }
                                        else
                                        {
                                            EventLogger.Instance.Logger.Error(@"Unable to discover Include for the Plc library: " + details
                                                 + Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<ProjectItemGroupPlaceholderReference>(x => x.Include) + " has an invalid reference!!!"
                                                 + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine);
                                        }
                                    }
                                    else
                                    {
                                        EventLogger.Instance.Logger.Error(@"Unable to discover the resolution for the Plc library: " + details
                                             + Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<ProjectItemGroupPlaceholderResolution>(x => x.Resolution) + " has the value of: " + projectItemGroupPlaceholderResolution.Resolution.ToString() + "!!!"
                                             + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EventLogger.Instance.Logger.Error(@"Unable to discover default resolution for the Plc library: " + details
                                         + Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<ProjectItemGroupPlaceholderReference>(x => x.DefaultResolution) + " has an invalid reference!!!"
                                         + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    EventLogger.Instance.Logger.Error(@"Unable to discover Plc libraries from the Plc project file: " + plcProjPath + "!!!"
                         + Environment.NewLine + @"File was not succesfully deserialized!!!"
                         + Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<Project>(x => x.ItemGroup) + " has an invalid reference!!!"
                         + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

                //Discover Plc library paths
                if (vs.PlcLibRepositories != null)
                {
                    foreach (PlcLibrary plcLibrary in PlcLibraries)
                    {
                        Version _highestVersion = null;
                        string _pathOfTheHighestVersion = null;
                        string details = "Title: " + (plcLibrary.Title != null ? plcLibrary.Title : "???") + " | "
                                         + "Version: " + (plcLibrary.Version != null ? plcLibrary.Version : "???") + " | "
                                         + "CompanyName: " + (plcLibrary.CompanyName != null ? plcLibrary.CompanyName : "???") + " | "
                                         + "PlaceHolder: " + (plcLibrary.PlaceHolder != null ? plcLibrary.PlaceHolder : "???");

                        if (plcLibrary.Version != null)
                        {
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
                                if (_highestVersion != null)
                                {
                                    plcLibrary.EffectiveVersion = _highestVersion.ToString();
                                    plcLibrary.Path = _pathOfTheHighestVersion;
                                }
                                else
                                {
                                    EventLogger.Instance.Logger.Error(@"Unable to find complete path for Plc library: " + details + " in any Plc repository!!!"
                                         + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine);
                                }
                            }
                            else
                            {
                                foreach (PlcLibRepository plcLibRepository in vs.PlcLibRepositories)
                                {
                                    plcLibrary.EffectiveVersion = plcLibrary.Version;
                                    plcLibrary.Path = null;
                                    string _path = (plcLibRepository.RepositoryPath + "\\" + plcLibrary.CompanyName + "\\" + plcLibrary.Title + "\\" + plcLibrary.EffectiveVersion).Replace("\\\\", "\\");
                                    if (Directory.Exists(_path))
                                    {
                                        plcLibrary.Path = _path;
                                        //break;
                                    }
                                }
                                if (plcLibrary.Path == null)
                                {
                                    EventLogger.Instance.Logger.Error(@"Unable to find complete path for Plc library: " + details + " in any Plc repository!!!"
                                         + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine);
                                }
                            }
                        }
                        else
                        {
                            EventLogger.Instance.Logger.Error(@"Invalid version of the Plc library: " + details
                                 + Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<PlcLibrary>(x => x.Version) + " has an invalid reference!!!"
                                 + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine);
                        }
                    }
                }
                else
                {
                    EventLogger.Instance.Logger.Error(@"Unable to discover Plc library repositories from the Plc project file: " + plcProjPath + "!!!"
                         + Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<Solution>(x => x.PlcLibRepositories) + " has an invalid reference!!!" + Environment.NewLine
                         + System.Reflection.MethodBase.GetCurrentMethod().Name );
                }
            }
            catch (Exception ex)
            {
                EventLogger.Instance.Logger.Error(@"Unable to discover Plc libraries from the Plc project file: " + plcProjPath + "!!!"
                     + Environment.NewLine + @"File not found or invalid!!!" + Environment.NewLine +
                     System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }
        }
    }
}

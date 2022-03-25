using iXlinker.Utils;
using iXlinkerDtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        internal static ObservableCollection<PlcLibRepository> GetPlcLibraryRepositories()
        {
            ObservableCollection<PlcLibRepository> LibraryRepositories = new ObservableCollection<PlcLibRepository>();
            string SystemLibraryRepositoryPath = Environment.GetEnvironmentVariable("TWINCAT3DIR")+ "Components\\Plc\\Managed Libraries\\";
            if (Directory.Exists(SystemLibraryRepositoryPath) && File.Exists(SystemLibraryRepositoryPath + "cache"))
            {
                PlcLibRepository SystemLibraryRepository = new PlcLibRepository() { RepositoryPath = SystemLibraryRepositoryPath, RepositoryName = "System" };
                LibraryRepositories.Add(SystemLibraryRepository);
            }

            string UserReposDefPath = Environment.GetEnvironmentVariable("ProgramData") + "\\TwinCAT PLC Control\\";
            string UserReposDefFile = UserReposDefPath + "TwinCAT PLC Control.opt";

            if (Directory.Exists(UserReposDefPath) && File.Exists(UserReposDefFile))
            {
                try
                {
                    var xml = XDocument.Load(UserReposDefFile);
                    var query = from c in xml.Root.Descendants("Single") where c.FirstAttribute.Value.Contains("b8d40b08-7d22-4669-afd2-1d431a18aafc") select c.Elements();

                    foreach (IEnumerable repo in query)
                    {
                        List<XElement> repos = repo.Cast<XElement>().ToList();
                        if(repos.Count == 2)
                        {
                            if(repos[0].FirstAttribute.Value.Equals("RootFolder") && repos[1].FirstAttribute.Value.Equals("Name"))
                            {
                                string UserLibraryRepositoryPath = (repos[0].Value.ToString() + "\\").Replace("\\\\", "\\");
                                string UserLibraryRepositoryFile = (UserLibraryRepositoryPath + "cache");
                                if (Directory.Exists(UserLibraryRepositoryPath) && File.Exists(UserLibraryRepositoryFile))
                                {
                                    PlcLibRepository UserLibraryRepository = new PlcLibRepository() { RepositoryPath = UserLibraryRepositoryPath, RepositoryName = repos[1].Value.ToString()};
                                    LibraryRepositories.Add(UserLibraryRepository);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

            }
            return LibraryRepositories;
        }
    }
}



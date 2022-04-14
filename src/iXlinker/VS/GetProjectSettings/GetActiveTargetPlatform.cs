using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using iXlinkerDtos;

namespace iXlinker.TsprojFile.Mapping
{
    internal partial class VS
    {
        private static string GetActiveTargetPlatform(string slnFolderPath, string slnName)
        {
            string ret = "Release|TwinCAT RT (x64)";
            string versionFolder = (slnFolderPath + "\\.vs\\" + slnName).Replace("\\\\","\\");
            string suoPath = "";
            if (Directory.Exists(versionFolder))
            {
                string[] dirs = Directory.GetDirectories(versionFolder);
                if(dirs.Length == 1)
                {
                    suoPath = (dirs[0] + "\\.suo").Replace("\\\\", "\\");
                }
                else if (dirs.Length > 1)
                {
                    int maxVersionSuported = -1;
                    bool maxVersionRetrieved = Int32.TryParse(VsProgID.Replace("VisualStudio.DTE.", "").Substring(0, VsProgID.Replace("VisualStudio.DTE.", "").IndexOf(".", StringComparison.Ordinal)), out maxVersionSuported);
                    int prevFolderVersion = -1;
                    string folderWithHighestSupportedVersion = "";
                    foreach(string dir in dirs)
                    {
                        string dirName = dir.Substring(dir.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                        int currentFolderVersion = -1;
                        bool prevFolderVersionRetrieved = Int32.TryParse(dirName.Replace("v", ""), out currentFolderVersion);
                        if(maxVersionRetrieved && prevFolderVersionRetrieved && currentFolderVersion > prevFolderVersion && currentFolderVersion <= maxVersionSuported)
                        {
                            prevFolderVersion = currentFolderVersion;
                            folderWithHighestSupportedVersion = dirName;
                        }
                    }
                    suoPath = (versionFolder + "\\" + folderWithHighestSupportedVersion + "\\.suo").Replace("\\\\", "\\");
                }

                FileStream fs = new FileStream(suoPath, FileMode.Open);
                int len = (int) fs.Length;
                byte[] byteArr = new byte[len];
                byte[] asciiArr = new byte[len];
                fs.Read(byteArr, 0, len);
                int i = 0;
                foreach (byte b in byteArr)
                {
                    if (b >= 0x20 && b <= 0x7f)
                    {
                        asciiArr[i] = b;
                        i++;
                    }
                }
                string configKeyWord = "ActiveCfg=";
                string str = Encoding.ASCII.GetString(asciiArr);
                string config = null;
                List<string> configs = new List<string>();
                while(str != null && str.Contains(configKeyWord))
                {
                    int configKeyWordIndex = str.IndexOf(configKeyWord, StringComparison.Ordinal);
                    config = str.Substring(configKeyWordIndex + configKeyWord.Length);
                    config = config.Substring(0,config.IndexOf(";", StringComparison.Ordinal));
                    configs.Add(config);
                    str = str.Substring(configKeyWordIndex + configKeyWord.Length + config.Length + 1 );
                }
                bool allConfigsAreEqual = true;
                if(!string.IsNullOrEmpty(config))
                {
                    foreach (string cfg in configs)
                    {
                        if(cfg != config)
                        {
                            allConfigsAreEqual = false;
                            break;
                        }
                    }
                }
                else
                {
                    allConfigsAreEqual = false;
                }
                if (allConfigsAreEqual)
                {
                    ret = config;
                }
            }

            return ret;
        }
    }
}

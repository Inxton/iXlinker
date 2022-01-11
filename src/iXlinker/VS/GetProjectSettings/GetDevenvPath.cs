using Microsoft.VisualStudio.Setup.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ViewModels;

namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        private static string GetDevenvPath()
        {
            string ret = "";
            Version minVersion = new Version(minVsSupportedVersionIncluded);
            Version maxVersion = new Version(maxVsSupportedVersionExcluded);

            try
            {
                SetupConfiguration query = new SetupConfiguration();
                ISetupConfiguration2 query2 = (ISetupConfiguration2)query;
                IEnumSetupInstances e = query2.EnumAllInstances();

                int fetched;
                List<VisualStudioInstalationViewModel> visualStudioInstances = new List<VisualStudioInstalationViewModel>();
                ISetupInstance[] instances = new ISetupInstance[1];
                do
                {
                    e.Next(1, instances, out fetched);
                    if (fetched > 0)
                    {
                        ISetupInstance2 instance2 = (ISetupInstance2)instances[0];
                        InstanceState state = instance2.GetState();

                        bool isComplete = state.Equals(InstanceState.Complete);
                        Version version = new Version((string)instances[0].GetInstallationVersion());

                        if (isComplete && version >= minVersion && version < maxVersion)
                            if ((state & InstanceState.Local) == InstanceState.Local)
                            {
                                VisualStudioInstalationViewModel visualStudioInstance = new VisualStudioInstalationViewModel(version, instance2.GetInstallationPath());
                                visualStudioInstances.Add(visualStudioInstance);
                            }
                    }
                }
                while (fetched > 0);
                VisualStudioInstalationViewModel lastVisualStudioInstance = null;
                foreach (VisualStudioInstalationViewModel vsi in visualStudioInstances)
                {
                    if (lastVisualStudioInstance == null)
                        lastVisualStudioInstance = vsi;
                    if (vsi.Version > lastVisualStudioInstance.Version)
                        lastVisualStudioInstance = vsi;
                }
                if (visualStudioInstances.Count>=1 && lastVisualStudioInstance != null && !string.IsNullOrEmpty(lastVisualStudioInstance.InstalationPath))
                {
                    ret = (lastVisualStudioInstance.InstalationPath + "\\Common7\\IDE\\devenv.com").Replace("\\\\", "\\");
                }


            }
            catch (COMException ex) when (ex.HResult == unchecked((int)0x80040154))
            {
                Console.WriteLine("The query API is not registered. Assuming no instances are installed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error 0x{ex.HResult:x8}: {ex.Message}");
            }

            return ret;
        }

    }
}
    

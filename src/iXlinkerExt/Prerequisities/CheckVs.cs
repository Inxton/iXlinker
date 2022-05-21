using iXlinkerExt.WPF.ViewModels;
using Microsoft.VisualStudio.Setup.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;

namespace iXlinkerExt
{
    partial class Prerequisites
    {
        internal static bool CheckVs(out string devenvPath)
        {
            bool vsOK = false;

            devenvPath = "";
            Version minVersion = new Version(Constants.minVsSupportedVersionIncluded);
            Version maxVersion = new Version(Constants.maxVsSupportedVersionExcluded);

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
                if (visualStudioInstances.Count >= 1 && lastVisualStudioInstance != null && !string.IsNullOrEmpty(lastVisualStudioInstance.InstalationPath))
                {
                    devenvPath = (lastVisualStudioInstance.InstalationPath + "\\Common7\\IDE\\devenv.com").Replace("\\\\", "\\");
                    vsOK = true;
                }


            }
            catch (COMException ex) when (ex.HResult == unchecked((int)0x80040154))
            {
                MessageBox.Show("The query API is not registered. Assuming no instances are installed.", "iXlinker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "iXlinker", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!vsOK)
            {
                MessageBox.Show("Unable to find supported version of the Visual Studio installed." + Environment.NewLine + "Installed version needs to be higher or equal to: {" + minVersion.ToString() + "}" + Environment.NewLine + "and lower then:{" + maxVersion.ToString() + "} at least Community edition!!!", "iXlinker", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return vsOK;
        }
    }
}

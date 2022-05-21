using iXlinkerExt.WPF.ViewModels;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace iXlinkerExt
{
    partial class SolutionDetails
    {
        internal static List<XaeProjectViewModel> GetAllXaeProjectsInTheSolution(IVsSolution solution)
        {
            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }

            IEnumHierarchies penum = null;
            var nullGuid = Guid.Empty;
            var hr = solution.GetProjectEnum((uint)__VSENUMPROJFLAGS.EPF_ALLPROJECTS, ref nullGuid, out penum);
            int items = 0;
            int plcProjectsCount = 0;
            XaeProjectViewModel xaeProject = new XaeProjectViewModel();

            List<XaeProjectViewModel> xaeProjects = new List<XaeProjectViewModel>();

            if (ErrorHandler.Succeeded(hr) && (penum != null))
            {
                uint fetched = 0;
                var rgelt = new IVsHierarchy[1];
                while (penum.Next(1, rgelt, out fetched) == 0 && fetched == 1)
                {
                    items++;
                    bool isXae = false;
                    xaeProject = new XaeProjectViewModel();
                    try
                    {
                        PropertyDescriptorCollection propertyDescriptorCollection = null;
                        try
                        {
                            propertyDescriptorCollection = TypeDescriptor.GetProperties(rgelt[0]);
                        }
                        catch (Exception ex)
                        {
                        }
                        if (propertyDescriptorCollection != null)
                        {
                            foreach (PropertyDescriptor descrip in propertyDescriptorCollection)
                            {
                                dynamic drgelt0 = rgelt[0];

                                if (descrip.Name == "ConfigurationManager")
                                {
                                    drgelt0 = rgelt[0];
                                    if (drgelt0 != null && drgelt0.ConfigurationManager != null && drgelt0.ConfigurationManager.ActiveTargetPlatform != null)
                                    {
                                        xaeProject.ActiveTargetConfigurationPlatform = (string)drgelt0.ConfigurationManager.ActiveTargetPlatform;
                                        isXae = true;
                                    }
                                }
                                if (descrip.Name == "VsProject")
                                {
                                    drgelt0 = rgelt[0];
                                    if (drgelt0 != null && drgelt0.VsProject != null && drgelt0.VsProject.FullName != null)
                                    {
                                        xaeProject.CompletePathInFileSystem = (string)drgelt0.VsProject.FullName;
                                        xaeProject.FileNameInFileSystem = xaeProject.CompletePathInFileSystem.Substring(xaeProject.CompletePathInFileSystem.LastIndexOf("\\") + 1);
                                        xaeProject.FolderPathInFileSystem = xaeProject.CompletePathInFileSystem.Substring(0, xaeProject.CompletePathInFileSystem.LastIndexOf("\\"));
                                        isXae = true;
                                    }
                                    if (drgelt0 != null && drgelt0.VsProject != null && drgelt0.VsProject.Name != null)
                                    {
                                        xaeProject.Name = (string)drgelt0.VsProject.Name;
                                        isXae = true;
                                    }
                                    if (drgelt0 != null && drgelt0.VsProject != null && drgelt0.VsProject.UniqueName != null)
                                    {
                                        xaeProject.UniqueName = (string)drgelt0.VsProject.UniqueName;
                                        isXae = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "iXlinker", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    if (isXae)
                    {
                        try
                        {
                            IVsSolutionBuildManager5 vsSolutionBuildManager5 = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager5;
                            string activeTargetPlatform = "";
                            int result = -1;

                            solution.GetGuidOfProject(rgelt[0], out Guid pguidProjectID);
                            result = vsSolutionBuildManager5.FindActiveProjectCfgName(pguidProjectID, out activeTargetPlatform);
                            xaeProject.GUID = pguidProjectID;
                            if (result == VSConstants.S_OK)
                            {
                                xaeProject.ActiveTargetConfigurationPlatform = activeTargetPlatform;
                            }
                            xaeProject.PlcProjects = GetPlcprojsFromXae(xaeProject.CompletePathInFileSystem);
                            xaeProjects.Add(xaeProject);

                            plcProjectsCount += xaeProject.PlcProjects.Count;

                            Notification.ShowInStatusBar("So far, " + xaeProjects.Count.ToString() + " XAE projects and " + plcProjectsCount.ToString() + " PLC projects has been found in this solution.");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "iXlinker", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
            }
            if (items == 0)
            {
                MessageBox.Show("No solution opened.", "iXlinker", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (xaeProjects.Count() == 0)
            {
                MessageBox.Show("No Twincat project found in this solution.", "iXlinker", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (plcProjectsCount == 0)
            {
                MessageBox.Show("No PLC project found in this solution.", "iXlinker", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return xaeProjects;
        }
    }
}

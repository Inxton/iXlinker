using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace iXlinkerExt
{
    /// <summary>
    /// Interaction logic for iXlinkerStartControl.
    /// </summary>
    public partial class iXlinkerStartWindow : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iXlinkerStartWindow"/> class.
        /// </summary>
        public iXlinkerStartWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]

        private void generateMappings_Click(object sender, RoutedEventArgs e)
        {
            string xaeCompletePath = iXlinkerExtCommand.Instance.XAE.CompletePathInFileSystem;
            string xaeName = iXlinkerExtCommand.Instance.XAE.Name;
            string xaeUniqueName = iXlinkerExtCommand.Instance.XAE.UniqueName;
            string platform = iXlinkerExtCommand.Instance.targetConfigurationPlatform;
            string plcPath = iXlinkerExtCommand.Instance.PLC.CompletePathInFileSystem;
            string plcProjName = iXlinkerExtCommand.Instance.PLC.Name;
            string disabledIO = iXlinkerExtCommand.Instance.doNotGenerateDisabledIO.ToString();
            string devenvPath = iXlinkerExtCommand.Instance.devenvPath;
            string maxEthercatFrameIndex = iXlinkerExtCommand.Instance.maxEthercatFrameIndex.ToString();
            bool isIndependent = iXlinkerExtCommand.Instance.PLC.IsIndependent;
            string xtiPathInFileSystem = iXlinkerExtCommand.Instance.PLC.XtiPathInFileSystem.ToString();

            List<string> list = new List<string>();
            list.Add($"-t \"{xaeCompletePath}\"");
            list.Add($"-p \"{platform}\"");
            list.Add($"-c \"{plcPath}\"");
            list.Add($"-g {disabledIO}");
            list.Add($"-d \"{devenvPath}\"");
            list.Add($"-n {maxEthercatFrameIndex}");
            String[] argumentList = list.ToArray();

            IVsSolution ivsSolution = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution;
            ErrorHandler.ThrowOnFailure(ivsSolution.GetSolutionInfo(out string solutionDirectory, out string solutionName, out string solutionDirectory2));
            string solutionFileName = solutionDirectory + Path.GetFileName(solutionName);
            string iXlinkerExeFolder = (solutionDirectory + "\\_Vortex\\iXlinker").Replace("\\\\", "\\");
            string iXlinkerExe = (iXlinkerExeFolder + "\\" + "iXlinker.exe").Replace("\\\\", "\\");
            bool isFilteredSolution = solutionDirectory2.Contains(".slnf");
            string slnfPath = string.Empty;

            if (isFilteredSolution)
            {
                slnfPath = SlnfFinder.DiscoverSlnfFilePath();
            }

            iXlinkerExtCommand.CloseToolWindow();

            if(File.Exists(iXlinkerExe))
            {
                int close = -1;

                close = ivsSolution.CloseSolutionElement((uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_ForceSave, null, 0);
                if (close == VSConstants.S_OK)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = iXlinkerExe;
                    startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    startInfo.Arguments = string.Join(" ", argumentList);

                    try
                    {
                        System.Diagnostics.Process exeProcess = System.Diagnostics.Process.Start(startInfo);
                        exeProcess.WaitForExit();
                        Notification.ShowInStatusBar("iXlinker finished succesfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                            ex.Message),
                            "iXlinkerStart", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    int open = -1;
                    if (!isFilteredSolution)
                    {
                        open = ivsSolution.OpenSolutionFile((uint)__VSSLNOPENOPTIONS.SLNOPENOPT_DontConvertSLN, solutionFileName);
                        Notification.ShowInStatusBar("Solution: {" + solutionFileName.ToString() + "} openned succesfully.");
                        if (open == VSConstants.S_OK)
                        {
                            if (isIndependent)
                            {
                                DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
                                dte.ExecuteCommand("File.SaveAll");
                                Notification.ShowInStatusBar("Solution: {" + solutionFileName.ToString() + "} saved succesfully.");
                            }
                        }
                    }
                    else if (!String.IsNullOrEmpty(slnfPath))
                    {
                        open = ivsSolution.OpenSolutionFile((uint)__VSSLNOPENOPTIONS.SLNOPENOPT_DontConvertSLN, slnfPath);
                        Notification.ShowInStatusBar("Filtered solution: {" + slnfPath.ToString() + "} openned succesfully.");
                        if (open == VSConstants.S_OK)
                        {
                            if (isIndependent)
                            {
                                Notification.ShowInStatusBar("Filtered solution: {" + slnfPath.ToString() + "} saved succesfully.");
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                    "Executable file iXlinker.exe has not been found in the directory.\n" + iXlinkerExeFolder),
                    "iXlinkerStart", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            iXlinkerExtCommand.CloseToolWindow();
        }

        private void targetConfigurationPlatformView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            iXlinkerExtCommand.CheckIfGenerationCouldStart();
        }

        private void checkBoxDoNotGenerateDisabledIO_Checked(object sender, RoutedEventArgs e)
        {
            iXlinkerExtCommand.Instance.doNotGenerateDisabledIO = true;
        }

        private void checkBoxDoNotGenerateDisabledIO_Unchecked(object sender, RoutedEventArgs e)
        {
            iXlinkerExtCommand.Instance.doNotGenerateDisabledIO = false;
        }

        private void maxEthercatFrameIndexView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            iXlinkerExtCommand.CheckIfGenerationCouldStart();
        }
    }
}
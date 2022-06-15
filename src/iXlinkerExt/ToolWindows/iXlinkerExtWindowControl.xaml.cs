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
            string xaePath = iXlinkerExtCommand.Instance.XAE.CompletePathInFileSystem;
            string platform = iXlinkerExtCommand.Instance.targetConfigurationPlatform;
            string plcPath = iXlinkerExtCommand.Instance.PLC.CompletePathInFileSystem;
            string disabledIO = iXlinkerExtCommand.Instance.doNotGenerateDisabledIO.ToString();
            string devenvPath = iXlinkerExtCommand.Instance.devenvPath;
            string maxEthercatFrameIndex = iXlinkerExtCommand.Instance.maxEthercatFrameIndex.ToString();
            bool isIndependent = iXlinkerExtCommand.Instance.PLC.IsIndependent;

            List<string> list = new List<string>();
            list.Add($"-t \"{xaePath}\"");
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

            if (File.Exists(iXlinkerExe))
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                            ex.Message),
                            "iXlinkerStart", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (!isFilteredSolution)
                    {
                        ivsSolution.OpenSolutionFile((uint)__VSSLNOPENOPTIONS.SLNOPENOPT_DontConvertSLN, solutionFileName);
                    }
                    else if (!String.IsNullOrEmpty(slnfPath))
                    {
                        ivsSolution.OpenSolutionFile((uint)__VSSLNOPENOPTIONS.SLNOPENOPT_DontConvertSLN, slnfPath);
                    }
                    if (isIndependent)
                    {
                        ivsSolution.SaveSolutionElement((uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_ForceSave, null, 0);
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
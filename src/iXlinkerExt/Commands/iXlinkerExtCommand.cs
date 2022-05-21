using iXlinkerExt.WPF.ViewModels;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Events;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Task = System.Threading.Tasks.Task;

namespace iXlinkerExt
{
    internal sealed class iXlinkerExtCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("2ceba715-5d41-4032-9847-5d5d95d3adf6");
        public List<XaeProjectViewModel> XAEs = new List<XaeProjectViewModel>();
        public List<PlcProjectViewModel> PLCs = new List<PlcProjectViewModel>();
        public List<string> targetConfigurationPlatformList = new List<string> { "Debug|TwinCAT CE7 (ARMV7)", "Debug|TwinCAT OS (ARMT2)", "Debug|TwinCAT RT (x64)", "Debug|TwinCAT RT (x86)",
                                                                                 "Release|TwinCAT CE7 (ARMV7)", "Release|TwinCAT OS (ARMT2)", "Release|TwinCAT RT (x64)", "Release|TwinCAT RT (x86)" };


        public List<ushort> maxEthercatFrameIndexList = new List<ushort> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };


        public string devenvPath = "";
        public bool generateMappingsEnabled = false;
        public XaeProjectViewModel XAE = new XaeProjectViewModel();
        public PlcProjectViewModel PLC = new PlcProjectViewModel();
        public string targetConfigurationPlatform = "";
        public ushort maxEthercatFrameIndex = 0;
        public bool doNotGenerateDisabledIO = true;
        public bool isFilteredSolution = false;

        //iXlinkerPackage IXlinkerPackage = new iXlinkerPackage();

        private readonly AsyncPackage package;
        private iXlinkerExtCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }
        public static iXlinkerExtCommand Instance
        {
            get;
            private set;
        }
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }
        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new iXlinkerExtCommand(package, commandService);

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            SolutionEvents.OnAfterOpenSolution += OnAfterOpenSolution;
            SolutionEvents.OnBeforeCloseSolution += OnBeforeCloseSolution;
        }

        private static void OnAfterOpenSolution(object sender = null, OpenSolutionEventArgs e = null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            CloseToolWindow();
        }
        private static void OnBeforeCloseSolution(object sender = null, EventArgs e = null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            CloseToolWindow();
        }
        private void Execute(object sender, EventArgs e)
        {
            this.package.JoinableTaskFactory.RunAsync(async delegate
            {
                Notification.ShowInStatusBar("Analyzing solution");
                bool prerequisities = Prerequisites.CheckPrerequisites(out devenvPath);
                XAEs = iXlinkerExtPackage.UpdateSolutionDetails();
                int xaesCount = XAEs.Count;
                if (xaesCount > 0)
                {
                    Notification.UseStatusBarProgressAsync("Analyzing XAE projects", 0, (uint)xaesCount);
                }

                int plcProjsCount = XAEs.Sum(plc => plc.PlcProjects.Count);
                if (prerequisities && xaesCount > 0 && plcProjsCount > 0)
                {
                    ShowToolWindow();
                }
            });
        }
        private void ShowToolWindow()
        {
            ToolWindowPane toolWindow = this.package.FindToolWindow(typeof(iXlinkerExtWindow), 0, true);

            if ((null == toolWindow) || (null == toolWindow.Frame))
            {
                throw new NotSupportedException("Cannot create window.");
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)toolWindow.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());

            iXlinkerStartWindow control = (iXlinkerStartWindow)toolWindow.Content;

            ListView xaeListView = control.xaeListView;
            xaeListView.ItemsSource = XAEs;

            ComboBox targetConfigurationPlatformView = control.targetConfigurationPlatformView;
            targetConfigurationPlatformView.ItemsSource = targetConfigurationPlatformList;

            TextBlock tbDevenvPath = control.tbDevenvPath;
            tbDevenvPath.Text = devenvPath;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Version versionExt = assembly.GetName().Version;

            TextBlock tbVersionExt = control.tbVersionExt;
            tbVersionExt.Text = versionExt.ToString();

            IVsSolution ivsSolution = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(ivsSolution.GetSolutionInfo(out string solutionDirectory, out string solutionName, out string solutionDirectory2));
            string solutionFileName = solutionDirectory + Path.GetFileName(solutionName);
            string iXlinkerExeFolder = (solutionDirectory + "\\_Vortex\\iXlinker").Replace("\\\\", "\\");
            string iXlinkerExe = (iXlinkerExeFolder + "\\" + "iXlinker.exe").Replace("\\\\", "\\");
            string versionCli = "";
            if (File.Exists(iXlinkerExe))
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(iXlinkerExe);
                versionCli = versionInfo.FileVersion;
            }

            TextBlock tbVersionCLI = control.tbVersionCLI;
            tbVersionCLI.Text = versionCli.ToString();

            CheckBox checkBoxDoNotGenerateDisabledIO = control.checkBoxDoNotGenerateDisabledIO;
            checkBoxDoNotGenerateDisabledIO.IsChecked = doNotGenerateDisabledIO;

            ComboBox maxEthercatFrameIndexView = control.maxEthercatFrameIndexView;
            maxEthercatFrameIndexView.ItemsSource = maxEthercatFrameIndexList;

            //Button generateMappings = control.generateMappings;
            //generateMappings.IsEnabled = false;

            ListView plcListView = control.plcListView;
            uint xaesCount = (uint)XAEs.Count;
            uint xaeIndex = 0;
            foreach (Object xae_item in xaeListView.Items)
            {
                Notification.UseStatusBarProgressAsync("Analyzing XAE projects", xaeIndex, xaesCount);
                xaeIndex++;
                ((INotifyPropertyChanged)xae_item).PropertyChanged += xaeListViewItem_PropertyChanged;
                XaeProjectViewModel xae = xae_item as XaeProjectViewModel;

                if (xae.IsChecked)
                {
                    XAE = xae;
                    PLCs = xae.PlcProjects;
                    plcListView.ItemsSource = PLCs;
                    foreach (Object plc_item in plcListView.Items)
                    {
                        ((INotifyPropertyChanged)plc_item).PropertyChanged += plcListViewItem_PropertyChanged;
                        if ((plc_item as PlcProjectViewModel).IsChecked)
                        {
                            PLC = plc_item as PlcProjectViewModel;
                        }
                    }
                    targetConfigurationPlatformView.SelectedItem = xae.ActiveTargetConfigurationPlatform;
                    targetConfigurationPlatform = targetConfigurationPlatformView.SelectedItem as string;
                }
            }
            maxEthercatFrameIndexView.SelectedIndex = 0;

            Notification.UseStatusBarProgressAsync("Analyzing XAE projects done", xaeIndex, xaesCount);
            Notification.ShowInStatusBar("Analyzing XAE projects done.");

            CheckIfGenerationCouldStart();
        }

        public static void CloseToolWindow()
        {
            try
            {
                ToolWindowPane toolWindow = Instance.package.FindToolWindow(typeof(iXlinkerExtWindow), 0, false);
                if (toolWindow != null)
                {
                    iXlinkerStartWindow control = (iXlinkerStartWindow)toolWindow.Content;

                    Instance.XAEs = new List<XaeProjectViewModel>();
                    ListView xaeListView = control.xaeListView;
                    xaeListView.ItemsSource = Instance.XAEs;

                    Instance.PLCs = new List<PlcProjectViewModel>();
                    ListView plcListView = control.plcListView;
                    plcListView.ItemsSource = Instance.PLCs;

                    (toolWindow.Frame as IVsWindowFrame).Hide();
                    toolWindow.Dispose();
                }
            }
            catch (Exception ex)
            {
                //todo
            }
        }

        private void xaeListViewItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            XaeProjectViewModel itemChanged = sender as XaeProjectViewModel;
            XAE = new XaeProjectViewModel();
            PLC = new PlcProjectViewModel();

            if (e.PropertyName.Equals(nameof(itemChanged.IsChecked)))
            {
                ToolWindowPane toolWindow = this.package.FindToolWindow(typeof(iXlinkerExtWindow), 0, false);
                iXlinkerStartWindow control = (iXlinkerStartWindow)toolWindow.Content;
                ComboBox targetConfigurationPlatformView = control.targetConfigurationPlatformView;
                ListView plcListView = control.plcListView;
                foreach (Object plc_item in plcListView.Items)
                {
                    ((INotifyPropertyChanged)plc_item).PropertyChanged -= plcListViewItem_PropertyChanged;
                }

                if (itemChanged.IsChecked)
                {
                    foreach (XaeProjectViewModel xae in XAEs)
                    {
                        if (xae.Equals(itemChanged))
                        {
                            XAE = xae;

                            PLCs = xae.PlcProjects;
                            plcListView.ItemsSource = PLCs;
                            foreach (Object plc_item in plcListView.Items)
                            {
                                ((INotifyPropertyChanged)plc_item).PropertyChanged += plcListViewItem_PropertyChanged;
                            }
                            targetConfigurationPlatformView.ItemsSource = targetConfigurationPlatformList;
                            targetConfigurationPlatformView.SelectedItem = xae.ActiveTargetConfigurationPlatform;
                        }
                        else
                        {
                            xae.PropertyChanged -= xaeListViewItem_PropertyChanged;
                            xae.IsChecked = false;
                            xae.PropertyChanged += xaeListViewItem_PropertyChanged;

                        }
                    }
                }
                else
                {
                    targetConfigurationPlatformView.ItemsSource = new List<string>();
                    targetConfigurationPlatformView.SelectedIndex = -1;

                    PLCs = new List<PlcProjectViewModel>();
                    plcListView.ItemsSource = PLCs;
                }
            }
            CheckIfGenerationCouldStart();
        }

        private void plcListViewItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PlcProjectViewModel itemChanged = sender as PlcProjectViewModel;

            if (e.PropertyName.Equals(nameof(itemChanged.IsChecked)))
            {
                if (itemChanged.IsChecked)
                {
                    foreach (PlcProjectViewModel plc in PLCs)
                    {
                        if (plc.Equals(itemChanged))
                        {
                            PLC = plc;
                        }
                        else
                        {
                            plc.IsChecked = false;
                        }
                    }
                }
                else
                {

                }
            }

            CheckIfGenerationCouldStart();
        }

        public static void CheckIfGenerationCouldStart()
        {
            bool xaeSelected = false;
            foreach (XaeProjectViewModel xae in Instance.XAEs)
            {
                if (!xaeSelected && xae.IsChecked)
                {
                    xaeSelected = true;
                    Instance.XAE = xae;
                }
                else if (xaeSelected && xae.IsChecked)
                {
                    xaeSelected = false;
                    break;
                }
            }

            ToolWindowPane toolWindow = Instance.package.FindToolWindow(typeof(iXlinkerExtWindow), 0, false);
            iXlinkerStartWindow control = (iXlinkerStartWindow)toolWindow.Content;
            ComboBox targetConfigurationPlatformView = control.targetConfigurationPlatformView;
            ComboBox maxEthercatFrameIndexView = control.maxEthercatFrameIndexView;
            Button generateMappings = control.generateMappings;

            bool plcSelected = false;
            foreach (PlcProjectViewModel plc in Instance.PLCs)
            {
                if (!plcSelected && plc.IsChecked)
                {
                    plcSelected = true;
                    Instance.PLC = plc;
                }
                else if (plcSelected && plc.IsChecked)
                {
                    plcSelected = false;
                    break;
                }
            }

            bool targetConfigurationPlatformSelected = targetConfigurationPlatformView.SelectedItem != null && Instance.targetConfigurationPlatformList.Contains(targetConfigurationPlatformView.SelectedItem);
            if (targetConfigurationPlatformSelected)
            {
                Instance.targetConfigurationPlatform = targetConfigurationPlatformView.SelectedItem as string;
            }

            bool maxEthercatFrameIndexSelected = maxEthercatFrameIndexView.SelectedIndex >= 0 && maxEthercatFrameIndexView.SelectedIndex < maxEthercatFrameIndexView.Items.Count;
            if (maxEthercatFrameIndexSelected)
            {
                ushort.TryParse(maxEthercatFrameIndexView.SelectedItem.ToString(), out Instance.maxEthercatFrameIndex);
            }

            if (xaeSelected && targetConfigurationPlatformSelected && plcSelected && maxEthercatFrameIndexSelected)
            {
                generateMappings.IsEnabled = true;
            }
            else
            {
                generateMappings.IsEnabled = false;
            }

        }

    }
}

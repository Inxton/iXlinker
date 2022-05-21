using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Collections.Generic;
using System.Linq;
using iXlinkerExt.WPF.ViewModels;
using Microsoft;

namespace iXlinkerExt
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(Constants.PackageGuidString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasMultipleProjects_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionHasSingleProject_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(iXlinkerExtWindow))]
    public sealed class iXlinkerExtPackage : AsyncPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await iXlinkerExtCommand.InitializeAsync(this);
        }

        public static List<XaeProjectViewModel> UpdateSolutionDetails()
        {
            IVsSolution ivsSolution = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution;
            ErrorHandler.ThrowOnFailure(ivsSolution.GetSolutionInfo(out string solutionDirectory, out string solutionName, out string solutionDirectory2));
            string solutionPath = solutionDirectory + System.IO.Path.GetFileNameWithoutExtension(solutionName);
            List<XaeProjectViewModel> XAEs = SolutionDetails.GetAllXaeProjectsInTheSolution(ivsSolution);
            if (XAEs.Count() == 1)
            {
                XAEs.FirstOrDefault().IsChecked = true;
                if (XAEs.FirstOrDefault().PlcProjects.Count == 1)
                {
                    XAEs.FirstOrDefault().PlcProjects.FirstOrDefault().IsChecked = true;
                }
            }
            return XAEs;
        }
        public static void UnloadXaeProject(Guid projectGuid)
        {
            IVsSolution4 ivsSolution4 = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution4;
            ivsSolution4.UnloadProject(projectGuid, (int)_VSProjectUnloadStatus.UNLOADSTATUS_UnloadedByUser);
        }
        public static void ReloadXaeProject(Guid projectGuid)
        {
            IVsSolution4 ivsSolution4 = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution4;
            ivsSolution4.ReloadProject(projectGuid);
        }

    }
}

using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using ViewModels;

namespace iXlinker.Utils
{
    class DialogClick
    {
        public static List<string> clickOkList = new List<string>();

        public static void Run(VisualStudioDTEViewModel vs)
        {
            clickOkList.Add("Remove all mapping infos");
            clickOkList.Add("Overlapping Sync Manager");
            clickOkList.Add("needs sync master (at least one variable linked to a task variable)");
            clickOkList.Add("does not support the data rates");
            clickOkList.Add("The build must be stopped before the project can be closed");
            clickOkList.Add("Restore variable links succeeded");

            Console.WriteLine("Listening to VS modals");
            while (true)
            {
                Thread.Sleep(100);
                ListenForModal(vs);
            }
        }

        private static void ListenForModal(VisualStudioDTEViewModel vs)
        {
            int processID = vs.ProcessID;
            Process p = Process.GetProcessById(processID);
            using var automation = new UIA3Automation();
            try
            {
                if (!p.HasExited)
                {
                    var window = new Application(p).GetMainWindow(automation, waitTimeout: TimeSpan.FromSeconds(3));
                    if (window != null)
                    {
                        ModalAutomation(window.ModalWindows, processID);
                        AutomationElement[] winChilds = window.FindAllChildren();
                        foreach (AutomationElement winChild in winChilds)
                        {
                            if (winChild.Name != null && winChild.Name.Contains("Unrestored variables links found"))
                            {
                                AutomationElement[] childs = winChild.FindAllChildren();
                                foreach (AutomationElement child in childs)
                                {
                                    if (child.Name.Equals("No"))
                                    {
                                        Console.WriteLine("{0} detected. Clicking on No", winChild.Name);
                                        var No = winChild.ConditionFactory.ByText("No");
                                        winChild.FindFirstDescendant(No).AsButton().Click();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                automation.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                automation.Dispose();
                Thread.Sleep(1000);
            }
        }

        private static void ModalAutomation(Window[] modalWindows, int processID) => modalWindows
            .ToList()
            .ForEach(modal =>
            {
                if (!modal.IsAvailable)
                    return;

                WindowHelper.BringProcessToFront(Process.GetProcessById(processID));

                if (modal.Name == "File Modification Detected")
                {
                    Console.WriteLine("File Modification Detected detected. Clicking on Ignore All");
                    var IgnoreAll = modal.ConditionFactory.ByText("Ignore All");
                    modal.FindFirstDescendant(IgnoreAll).AsButton().Click();
                }
                AutomationElement[] childs = modal.FindAllChildren();
                foreach (AutomationElement child in childs)
                {
                    foreach (string clickOk in clickOkList)
                    {
                        if (child.Name != null && child.Name.Contains(clickOk))
                        {
                            Console.WriteLine("Id:{0} Name: {1} detected. Clicking on OK", child.AutomationId, child.Name);
                            var OK = modal.ConditionFactory.ByText("OK");
                            modal.FindFirstDescendant(OK).AsButton().Click();
                            break;
                        }
                    }
                }
            }
        );

        public static class WindowHelper
        {
            public static void BringProcessToFront(Process process)
            {
                IntPtr handle = process.MainWindowHandle;
                if (IsIconic(handle))
                {
                    ShowWindow(handle, SW_RESTORE);
                }

                SetForegroundWindow(handle);
            }

            const int SW_RESTORE = 9;

            [System.Runtime.InteropServices.DllImport("User32.dll")]
            private static extern bool SetForegroundWindow(IntPtr handle);
            [System.Runtime.InteropServices.DllImport("User32.dll")]
            private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
            [System.Runtime.InteropServices.DllImport("User32.dll")]
            private static extern bool IsIconic(IntPtr handle);
        }
    }
}

using System;
using ViewModels;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private TaskViewModel FillTaskData(TcSmTaskType task)
        {
            TaskViewModel taskViewModel = new TaskViewModel();

            string name = "";
            try
            {
                if (task.Name != null)
                {
                    name = task.Name;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            string id = "";
            try
            {
                if (task.Id != null)
                {
                    id = task.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            int priority = 0;
            try
            {
                if (task.Priority != null)
                {
                    priority = (int)task.Priority;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            int cycleTime = 0;
            try
            {
                if (task.CycleTime != null)
                {
                    cycleTime = (int)task.CycleTime;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            int amsPort = 0;
            try
            {
                if (task.AmsPort != null)
                {
                    amsPort = (int)task.AmsPort;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }


            taskViewModel.Name = name;
            taskViewModel.Id = id;
            taskViewModel.Priority = priority;
            taskViewModel.CycleTime = cycleTime;
            taskViewModel.AmsPort = amsPort;


            return taskViewModel;
        }
    }
}

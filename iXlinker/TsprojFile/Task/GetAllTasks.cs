using ViewModels;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void GetAllTasks()
        {
            if (Tc.Project.System != null && Tc.Project.System.Tasks != null)
            {
                foreach (TcSmTaskType task in Tc.Project.System.Tasks)
                {
                    AddTask(task);
                }
            }
        }
    }
}

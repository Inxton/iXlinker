using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddTask(TcSmTaskType task)
        {
            TaskViewModel taskViewModel = FillTaskData(task);

            TotalNumberOfTasks++;
            Tasks.Add(taskViewModel);
        }
    }
}

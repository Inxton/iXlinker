using System;
using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        public void ClearLists()
        {
            try
            {
                // Clear lists
                Devices.Clear();
                Tasks.Clear();
                PdoEntryStructures.Clear();
                PdoStructures.Clear();
                BoxStructures.Clear();
                DeviceStructures.Clear();
                TopologyStructures.Clear();
                PlcLibraries.Clear();
                PlcStructuresInPlcLibraries.Clear();

                // Reset counters
                TotalNumberOfDevices = 0;
                TotalNumberOfBoxes = 0;
                TotalNumberOfPdos = 0;
                TotalNumberOfPdoEntries = 0;
                TotalNumberOfTasks = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                Console.ReadLine();
            }
        }
    }
}

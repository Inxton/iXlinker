using TwincatXmlSchemas.TcPlcProj;
using iXlinkerDtos;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ExportAllStructuresByNumberOfUses(string exportDir)
        {

            if (Directory.Exists(exportDir))
            {
                string[] files = Directory.GetFiles(exportDir);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                string[] dirs = Directory.GetDirectories(exportDir);
                foreach (string dir in dirs)
                {
                    Directory.Delete(dir, true);
                }
            }
            else
            {
                Directory.CreateDirectory(exportDir);
            }
            ObservableCollection<PouStructBase> allStructures = new ObservableCollection<PouStructBase>();

            using (StreamWriter sw = new StreamWriter(exportDir + "\\PdoEntryStructures.txt"))
            {
                ObservableCollection<PdoEntryStructViewModel> sorted = new ObservableCollection<PdoEntryStructViewModel>(PdoEntryStructures.OrderByDescending(i => i.NumberOfUses));
                foreach (PdoEntryStructViewModel item in sorted)
                {
                    sw.WriteLine(item.Name + ";" + item.NumberOfUses);
                    allStructures.Add(new PouStructBase() { Name = item.Name, NumberOfUses = item.NumberOfUses });
                }
            }

            using (StreamWriter sw = new StreamWriter(exportDir + "\\PdoStructures.txt"))
            {
                ObservableCollection<PdoStructViewModel> sorted = new ObservableCollection<PdoStructViewModel>(PdoStructures.OrderByDescending(i => i.NumberOfUses));
                foreach (PdoStructViewModel item in sorted)
                {
                    sw.WriteLine(item.Name + ";" + item.NumberOfUses);
                    allStructures.Add(new PouStructBase() { Name = item.Name, NumberOfUses = item.NumberOfUses });
                }
            }

            using (StreamWriter sw = new StreamWriter(exportDir + "\\BoxStructures.txt"))
            {
                ObservableCollection<BoxStructViewModel> sorted = new ObservableCollection<BoxStructViewModel>(BoxStructures.OrderByDescending(i => i.NumberOfUses));
                foreach (BoxStructViewModel item in sorted)
                {
                    sw.WriteLine(item.Name + ";" + item.NumberOfUses);
                    allStructures.Add(new PouStructBase() { Name = item.Name, NumberOfUses = item.NumberOfUses });
                }
            }

            using (StreamWriter sw = new StreamWriter(exportDir + "\\DeviceStructures.txt"))
            {
                ObservableCollection<DeviceStructViewModel> sorted = new ObservableCollection<DeviceStructViewModel>(DeviceStructures.OrderByDescending(i => i.NumberOfUses));
                foreach (DeviceStructViewModel item in sorted)
                {
                    sw.WriteLine(item.Name + ";" + item.NumberOfUses);
                    allStructures.Add(new PouStructBase() { Name = item.Name, NumberOfUses = item.NumberOfUses });
                }
            }

            using (StreamWriter sw = new StreamWriter(exportDir + "\\TopologyStructures.txt"))
            {
                ObservableCollection<TopologyStructViewModel> sorted = new ObservableCollection<TopologyStructViewModel>(TopologyStructures.OrderByDescending(i => i.NumberOfUses));
                foreach (TopologyStructViewModel item in sorted)
                {
                    sw.WriteLine(item.Name + ";" + item.NumberOfUses);
                    allStructures.Add(new PouStructBase() { Name = item.Name, NumberOfUses = item.NumberOfUses });
                }
            }

            using (StreamWriter sw = new StreamWriter(exportDir + "\\AllStructures.txt"))
            {
                ObservableCollection<PouStructBase> sorted = new ObservableCollection<PouStructBase>(allStructures.OrderByDescending(i => i.NumberOfUses));
                foreach (PouStructBase item in sorted)
                {
                    sw.WriteLine(item.Name + ";" + item.NumberOfUses);
                    allStructures.Add(new PouStructBase() { Name = item.Name, NumberOfUses = item.NumberOfUses });
                }
            }
        }
    }
}

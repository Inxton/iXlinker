using TwincatXmlSchemas.TcPlcProj;
using iXlinkerDtos;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddDeviceStructuresToPlcproj(Solution vs , ref Project tcPlcProj)
        {
            ExportDeviceStructures(vs.DutsIoDevice.FolderPathInFileSystem);

            EventLogger.Instance.Logger.Information("Adding Device structures into the PLC project!!!");

            foreach (ProjectItemGroup item in tcPlcProj.ItemGroup)
            {
                if (item.Compile != null)
                {
                    int oldCompileItemsCount = item.Compile.Length;
                    int compileItemToAdd = DeviceStructures.Count;
                    int compileIndex = 0;

                    int newCompileItemsCount = oldCompileItemsCount + compileItemToAdd;
                    ProjectItemGroupCompile[] newItemCompile = new ProjectItemGroupCompile[newCompileItemsCount];

                    foreach (ProjectItemGroupCompile itemCompile in item.Compile)
                    {
                        newItemCompile[compileIndex] = itemCompile;
                        compileIndex++;
                    }

                    foreach (DeviceStructViewModel deviceStructViewModel in DeviceStructures)
                    {
                        string structName = vs.DutsIoDevice.Path+ "\\" + deviceStructViewModel.Name + ".TcDUT";
                        ProjectItemGroupCompile itemCompile = new ProjectItemGroupCompile() { Include = structName, SubType = "Code" };
                        newItemCompile[compileIndex] = itemCompile;
                        compileIndex++;
                    }

                    item.Compile = newItemCompile;
                }
            }
            EventLogger.Instance.Logger.Information("Device structures added into the PLC project!!!");
         }
    }
}

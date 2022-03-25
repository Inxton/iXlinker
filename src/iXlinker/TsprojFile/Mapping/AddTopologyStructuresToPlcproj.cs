using TwincatXmlSchemas.TcPlcProj;
using iXlinkerDtos;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddTopologyStructuresToPlcproj(Solution vs , ref Project tcPlcProj)
        {
            ExportTopologyStructures(vs.DutsIoTopology.FolderPathInFileSystem);

            EventLogger.Instance.Logger.Information("Adding Topology structures into the PLC project!!!");

            foreach (ProjectItemGroup item in tcPlcProj.ItemGroup)
            {
                if (item.Compile != null)
                {
                    int oldCompileItemsCount = item.Compile.Length;
                    int compileItemToAdd = TopologyStructures.Count;
                    int compileIndex = 0;

                    int newCompileItemsCount = oldCompileItemsCount + compileItemToAdd;
                    ProjectItemGroupCompile[] newItemCompile = new ProjectItemGroupCompile[newCompileItemsCount];

                    foreach (ProjectItemGroupCompile itemCompile in item.Compile)
                    {
                        newItemCompile[compileIndex] = itemCompile;
                        compileIndex++;
                    }

                    foreach (TopologyStructViewModel topologyStructViewModel in TopologyStructures)
                    {
                        string structName = vs.DutsIoTopology.Path+ "\\" + topologyStructViewModel.Name + ".TcDUT";
                        ProjectItemGroupCompile itemCompile = new ProjectItemGroupCompile() { Include = structName, SubType = "Code" };
                        newItemCompile[compileIndex] = itemCompile;
                        compileIndex++;
                    }

                    item.Compile = newItemCompile;
                }
            }
            EventLogger.Instance.Logger.Information("Topology structures added into the PLC project!!!");
         }
    }
}

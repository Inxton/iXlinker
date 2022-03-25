using TwincatXmlSchemas.TcPlcProj;
using iXlinkerDtos;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddBoxStructuresToPlcproj(Solution vs , ref Project tcPlcProj)
        {
            ExportBoxStructures(vs.DutsIoBox.FolderPathInFileSystem);

            EventLogger.Instance.Logger.Information("Adding Box structures into the PLC project!!!");

            foreach (ProjectItemGroup item in tcPlcProj.ItemGroup)
            {
                if (item.Compile != null)
                {
                    int oldCompileItemsCount = item.Compile.Length;
                    int compileItemToAdd = BoxStructures.Count;
                    int compileIndex = 0;

                    int newCompileItemsCount = oldCompileItemsCount + compileItemToAdd;
                    ProjectItemGroupCompile[] newItemCompile = new ProjectItemGroupCompile[newCompileItemsCount];

                    foreach (ProjectItemGroupCompile itemCompile in item.Compile)
                    {
                        newItemCompile[compileIndex] = itemCompile;
                        compileIndex++;
                    }

                    foreach (BoxStructViewModel boxStructViewModel in BoxStructures)
                    {
                        string structName = vs.DutsIoBox.Path+ "\\" + boxStructViewModel.Name + ".TcDUT";
                        ProjectItemGroupCompile itemCompile = new ProjectItemGroupCompile() { Include = structName, SubType = "Code" };
                        newItemCompile[compileIndex] = itemCompile;
                        compileIndex++;
                    }

                    item.Compile = newItemCompile;
                }
            }
            EventLogger.Instance.Logger.Information("Box structures added into the PLC project!!!");
         }
    }
}

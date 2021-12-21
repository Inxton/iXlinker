using TwincatXmlSchemas.TcPlcProj;
using ViewModels;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddPdoEntryStructuresToPlcproj(SolutionViewModel vs , ref Project tcPlcProj)
        {
            ExportPdoEntryStructures(vs.DutsIoPdoEntry.FolderPathInFileSystem);

            System.Console.WriteLine("Adding PDO entry structures into the PLC project!!!");

            foreach (ProjectItemGroup item in tcPlcProj.ItemGroup)
            {
                if (item.Compile != null)
                {
                    int oldCompileItemsCount = item.Compile.Length;
                    int compileItemToAdd = PdoEntryStructures.Count;
                    int compileIndex = 0;

                    int newCompileItemsCount = oldCompileItemsCount + compileItemToAdd;
                    ProjectItemGroupCompile[] newItemCompile = new ProjectItemGroupCompile[newCompileItemsCount];

                    foreach (ProjectItemGroupCompile itemCompile in item.Compile)
                    {
                        newItemCompile[compileIndex] = itemCompile;
                        compileIndex++;
                    }

                    foreach (PdoEntryStructViewModel pdoEntryStructViewModel in PdoEntryStructures)
                    {
                        string structName = vs.DutsIoPdoEntry.Path+ "\\" + pdoEntryStructViewModel.Name + ".TcDUT";
                        ProjectItemGroupCompile itemCompile = new ProjectItemGroupCompile() { Include = structName, SubType = "Code" };
                        newItemCompile[compileIndex] = itemCompile;
                        compileIndex++;
                    }

                    item.Compile = newItemCompile;
                }
            }
            System.Console.WriteLine("PDO entry structures added into the PLC project!!!");
         }
    }
}

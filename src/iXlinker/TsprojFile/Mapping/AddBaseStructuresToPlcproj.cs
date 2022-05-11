using TwincatXmlSchemas.TcPlcProj;
using iXlinkerDtos;
using iXlinker.Utils;
using iXlinker.Resources;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddBaseStructuresToPlcproj(Solution vs , ref Project tcPlcProj)
        {
            ExportBaseStructures(vs.DutsIoBase.FolderPathInFileSystem);

            EventLogger.Instance.Logger.Information("Adding Base structures into the PLC project!!!");

            foreach (ProjectItemGroup item in tcPlcProj.ItemGroup)
            {
                if (item.Compile != null)
                {
                    int oldCompileItemsCount = item.Compile.Length;
                    int compileItemToAdd = StructureBasesResourceDictionary.Count;
                    int compileIndex = 0;

                    int newCompileItemsCount = oldCompileItemsCount + compileItemToAdd;
                    ProjectItemGroupCompile[] newItemCompile = new ProjectItemGroupCompile[newCompileItemsCount];

                    foreach (ProjectItemGroupCompile itemCompile in item.Compile)
                    {
                        newItemCompile[compileIndex] = itemCompile;
                        compileIndex++;
                    }

                    foreach (StructureBase structureBase in StructureBasesResourceDictionary)
                    {
                        //Exports only base structure that has an empty namespace, that means it does not exists in any PLC library used in this PLC project
                        if (string.IsNullOrEmpty(structureBase.BaseStructureNamespace))
                        {
                            if (!structureBase.BaseStructurePrefix.Equals("!"))
                            {
                                string structName = vs.DutsIoBase.Path + "\\" + structureBase.BaseStructureName + ".TcDUT";
                                ProjectItemGroupCompile itemCompile = new ProjectItemGroupCompile() { Include = structName, SubType = "Code" };
                                newItemCompile[compileIndex] = itemCompile;
                                compileIndex++;
                            }
                        }
                    }
                    item.Compile = newItemCompile;
                }
            }
            EventLogger.Instance.Logger.Information("Base structures added into the PLC project!!!");
         }
    }
}

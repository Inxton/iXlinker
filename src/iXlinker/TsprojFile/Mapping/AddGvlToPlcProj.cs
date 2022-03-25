using TwincatXmlSchemas.TcPlcProj;
using iXlinkerDtos;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddGvlToPlcProj(Solution vs, ref Project plcProj)
        {
            GenerateGvlToPlcProj(vs);
            EventLogger.Instance.Logger.Information("Adding GVL into the PLC project!!!");

            foreach (ProjectItemGroup item in plcProj.ItemGroup)
            {
                if (item.Compile != null)
                {
                    int oldCompileItemsCount = item.Compile.Length;
                    int compileItemToAdd = PdoEntryStructures.Count;
                    int compileIndex = 0;

                    int newCompileItemsCount = oldCompileItemsCount + 1;
                    ProjectItemGroupCompile[] newItemCompile = new ProjectItemGroupCompile[newCompileItemsCount];

                    foreach (ProjectItemGroupCompile itemCompile in item.Compile)
                    {
                        newItemCompile[compileIndex] = itemCompile;
                        compileIndex++;
                    }

                    newItemCompile[compileIndex] = new ProjectItemGroupCompile() { Include = vs.GvlExported.Path + "\\" + vs.GvlExported.Name + ".TcGVL", SubType = "Code" };

                    item.Compile = newItemCompile;
                }
            }
            EventLogger.Instance.Logger.Information("GVL added into the PLC project!");
         }
    }
}

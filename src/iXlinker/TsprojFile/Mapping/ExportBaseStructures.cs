using PlcprojFile;
using System;
using System.IO;
using iXlinkerDtos;
using iXlinker.Utils;
using iXlinker.Resources;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        private void ExportBaseStructures(string exportDir)
        {
            ExportBaseStructuresToDirectory(exportDir);
        }

        private void ExportBaseStructuresToDirectory(string exportDir)
        {
            EventLogger.Instance.Logger.Information(@"Exporting Base structures to the folder ""{0}""", exportDir);
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

            foreach (StructureBase structure in StructureBasesResourceDictionary)
            {
                //Exports only base structure that has an empty namespace, that means it does not exists in any PLC library used in this PLC project
                if (string.IsNullOrEmpty(structure.BaseStructureNamespace))
                {
                    if (!structure.BaseStructurePrefix.Equals("!"))
                    {
                        //Apply extension from dictionary
                        string extends = " :";
                        foreach (StructureBase structureBase in StructureBasesResourceDictionary)
                        {
                            if (structureBase.StructureType.Equals(structure.GetType().ToString()) && (structureBase.StructureName.Equals(structure.BaseStructureName) || structureBase.StructureName.Equals("*")) )
                            {
                                extends = " EXTENDS " + ValidatePlcItem.NameIncludingNamespace(structureBase.BaseStructureNamespace, structureBase.BaseStructureName) +" :";
                                break;
                            }
                        }
                        string structName = structure.BaseStructureName;
                        StreamWriter sw = new StreamWriter(exportDir + "\\" + structName + ".TcDUT");
                        try
                        {
                            sw.WriteLine("<TcPlcObject>");
                            sw.WriteLine("\t<DUT Name=" + @"""" + structName + @""">");
                            sw.WriteLine("\t\t<Declaration><![CDATA[");
                            foreach (string attribute in structure.Attributes)
                            {
                                sw.WriteLine(attribute);
                            }
                            sw.WriteLine("TYPE " + structName + extends);
                            sw.WriteLine("STRUCT");
                            sw.WriteLine("END_STRUCT");
                            sw.WriteLine("END_TYPE");
                            sw.WriteLine("]]></Declaration>");
                            sw.WriteLine("\t</DUT>");
                            sw.WriteLine("</TcPlcObject>");
                            sw.Close();
                        }
                        catch (Exception ex)
                        {
                            EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                            sw.Dispose();
                        }
                        finally
                        {
                            sw.Close();
                        }
                    }
                }
            }
            EventLogger.Instance.Logger.Information(@"Base structures exported to the folder ""{0}"" !!!", exportDir);
        }
    }
}

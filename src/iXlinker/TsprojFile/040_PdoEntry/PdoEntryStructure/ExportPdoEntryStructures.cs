using PlcprojFile;
using System;
using System.IO;
using iXlinkerDtos;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        private void ExportPdoEntryStructures(string exportDir)
        {
            ExportPdoEntryStructuresToDirectory(exportDir);
        }

        private void ExportPdoEntryStructuresToDirectory(string exportDir)
        {
            EventLogger.Instance.Logger.Information("Exporting Pdo entry structures to the folder {0}", exportDir);
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

            foreach (PdoEntryStructViewModel pdoEntryStructViewModel in PdoEntryStructures)
            {
                string structName = pdoEntryStructViewModel.Name;
                StreamWriter sw = new StreamWriter(exportDir + "\\" + structName + ".TcDUT");

                try
                {
                    string boxOrderCode = "";
                    try
                    {
                        boxOrderCode = pdoEntryStructViewModel.BoxOrderCode;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    string id = "";
                    try
                    {
                        id = pdoEntryStructViewModel.Id.Replace('{','[').Replace('}',']');
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    uint crc = 0;
                    try
                    {
                        crc = pdoEntryStructViewModel.Crc32;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }
                    uint sizeInBites = 0;
                    try
                    {
                        sizeInBites = pdoEntryStructViewModel.SizeInBites;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    double sizeInBytes = 0;
                    try
                    {
                        sizeInBytes = pdoEntryStructViewModel.SizeInBytes;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    sw.WriteLine("<TcPlcObject>");
                    sw.WriteLine("\t<DUT Name=" + @"""" + structName + @""">");
                    sw.WriteLine("\t\t<Declaration><![CDATA[{attribute addProperty BoxType \"" + boxOrderCode + "\"}");
                    sw.WriteLine("{attribute addProperty Id \"" + id + "\"}");
                    sw.WriteLine("{attribute addProperty CRC \"" + crc.ToString() + "\"}");
                    sw.WriteLine("{attribute addProperty SizeInBites \"" + sizeInBites.ToString() + "\"}");
                    sw.WriteLine("{attribute addProperty SizeInBytes \"" + sizeInBytes.ToString() + "\"}");
                    if (sizeInBites % 8 != 0)
                    {
                        sw.WriteLine("{warning 'Size of this structure is not a multiple of 8 bits!!!'}");
                    }
                    sw.WriteLine("TYPE " + structName + " :");
                    sw.WriteLine("STRUCT");

                    foreach (PdoEntryStructMemberViewModel pdoEntryStructMemberViewModel in pdoEntryStructViewModel.StructMembers)
                    {
                        foreach(string attribute in pdoEntryStructMemberViewModel.Attributes)
                        {
                            sw.WriteLine("\t" + attribute);
                        }
                        string varName = pdoEntryStructMemberViewModel.Name;
                        string varType = ValidatePlcItem.NameIncludingNamespace(pdoEntryStructMemberViewModel.TypeNamespace, pdoEntryStructMemberViewModel.Type_Value);
                        sw.WriteLine("\t" + varName + " : " + varType + ";");
                    }

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
            EventLogger.Instance.Logger.Information("PDO entry structures exported!!!");
        }
    }
}

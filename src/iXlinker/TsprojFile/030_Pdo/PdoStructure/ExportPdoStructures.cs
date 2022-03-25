using PlcprojFile;
using System;
using System.IO;
using iXlinkerDtos;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        private void ExportPdoStructures(string exportDir)
        {
            ExportPdoStructuresToDirectory(exportDir);
        }
        private void ExportPdoStructuresToDirectory(string exportDir)
        {
            EventLogger.Instance.Logger.Information(@"Exporting Pdo structures to the folder ""{0}""", exportDir);
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

            foreach (PdoStructViewModel pdoStructViewModel in PdoStructures)
            {
                string structName = pdoStructViewModel.Name;
                StreamWriter sw = new StreamWriter(exportDir + "\\" + structName + ".TcDUT");

                try
                {
                    string boxOrderCode = "";
                    try
                    {
                        boxOrderCode = pdoStructViewModel.BoxOrderCode;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    string id = "";
                    try
                    {
                        id = pdoStructViewModel.Id.Replace('{','[').Replace('}',']');
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    uint crc = 0;
                    try
                    {
                        crc = pdoStructViewModel.Crc32;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    uint sizeInBites = 0;
                    try
                    {
                        sizeInBites = pdoStructViewModel.SizeInBites;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    double sizeInBytes = 0;
                    try
                    {
                        sizeInBytes = pdoStructViewModel.SizeInBytes;
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
                    sw.WriteLine("TYPE " + structName + " :");
                    sw.WriteLine("STRUCT");

                    foreach (PdoStructMemberViewModel pdoStructMemberViewModel in pdoStructViewModel.StructMembers)
                    {
                        foreach(string attribute in pdoStructMemberViewModel.Attributes)
                        {
                            sw.WriteLine("\t" + attribute);
                        }
                        //string varName = ValidatePlcItem.Name(pdoStructMemberViewModel.Name);
                        //string varType= ValidatePlcItem.Type(pdoStructMemberViewModel.Type_Value);
                        string varName = pdoStructMemberViewModel.Name;
                        string varType = ValidatePlcItem.NameIncludingNamespace(pdoStructMemberViewModel.TypeNamespace, pdoStructMemberViewModel.Type_Value);
                        sw.WriteLine("\t" + varName + " " +  pdoStructMemberViewModel.InOutPlcProj + " : " + varType + ";");
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
            EventLogger.Instance.Logger.Information(@"PDO structures exported to the folder ""{0}"" !!!", exportDir);
        }
    }
}

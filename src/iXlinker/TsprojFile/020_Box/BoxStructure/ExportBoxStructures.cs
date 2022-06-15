using PlcprojFile;
using System;
using System.IO;
using iXlinkerDtos;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        private void ExportBoxStructures(string exportDir)
        {
            ExportBoxStructuresToDirectory(exportDir);
        }

        private void ExportBoxStructuresToDirectory(string exportDir)
        {
            EventLogger.Instance.Logger.Information(@"Exporting Box structures to the folder ""{0}""", exportDir);
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

            foreach (BoxStructViewModel boxStructViewModel in BoxStructures)
            {
                string structName = boxStructViewModel.Name;

                StreamWriter sw = new StreamWriter(exportDir + "\\" + structName + ".TcDUT");

                try
                {
                    string boxOrderCode = "";
                    try
                    {
                        boxOrderCode = boxStructViewModel.BoxOrderCode;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    string id = "";
                    try
                    {
                        id = string.IsNullOrEmpty(boxStructViewModel.Id) ? "" : boxStructViewModel.Id.Replace('{','[').Replace('}',']');
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    string extends = "";
                    try
                    {
                        extends = boxStructViewModel.Extends;
                        if (!string.IsNullOrEmpty(extends))
                        {
                            extends = " EXTENDS " + extends + " :";
                        }
                        else
                        {
                            extends = " :";
                        }
                    }
                    catch (Exception ex)
                    {
                        extends = " :";
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    uint crc = 0;
                    try
                    {
                        crc = boxStructViewModel.Crc32;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    double size = 0;
                    try
                    {
                        size = boxStructViewModel.Size;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }
                    string description = "";
                    try
                    {
                        description = string.IsNullOrEmpty(boxStructViewModel.Description) ? "" : boxStructViewModel.Description;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }
                    string physics = "";
                    try
                    {
                        physics = string.IsNullOrEmpty(boxStructViewModel.Physics) ? "" : boxStructViewModel.Physics;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }


                    sw.WriteLine("<TcPlcObject>");
                    sw.WriteLine("\t<DUT Name=" + @"""" + structName + @""">");
                    sw.WriteLine("\t\t<Declaration><![CDATA[{attribute 'GeneratedUsingTerminal: " + boxOrderCode + "'}");
                    sw.WriteLine("{attribute addProperty BoxType \"" + boxOrderCode + "\"}");
                    sw.WriteLine("{attribute addProperty Description \"" + description + "\"}");
                    sw.WriteLine("{attribute addProperty Physics \"" + physics + "\"}");
                    sw.WriteLine("{attribute addProperty Id \"" + id + "\"}");
                    sw.WriteLine("{attribute addProperty CRC \"" + crc.ToString() + "\"}");
                    sw.WriteLine("{attribute addProperty Size \"" + size.ToString() + "\"}");
                    sw.WriteLine("TYPE " + structName + extends);
                    sw.WriteLine("STRUCT");

                    foreach (BoxStructMemberViewModel boxStructMemberViewModel in boxStructViewModel.StructMembers)
                    {
                        foreach (string attribute in boxStructMemberViewModel.Attributes)
                        {
                            sw.WriteLine("\t" + attribute);
                        }
                        string varName = boxStructMemberViewModel.Name;
                        string varType = ValidatePlcItem.NameIncludingNamespace(boxStructMemberViewModel.TypeNamespace , boxStructMemberViewModel.Type_Value);
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
            EventLogger.Instance.Logger.Information(@"Box structures exported to the folder ""{0}"" !!!", exportDir);
        }
    }
}

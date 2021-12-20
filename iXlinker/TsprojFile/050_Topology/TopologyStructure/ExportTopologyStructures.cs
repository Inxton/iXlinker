using PlcprojFile;
using System;
using System.IO;
using ViewModels;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        private void ExportTopologyStructures(string exportDir)
        {
            ExportTopologyStructuresToDirectory(exportDir);
        }

        private void ExportTopologyStructures()
        {
            string exportDir = Path.GetDirectoryName(Path.GetDirectoryName(TsProjFilePath)) + "\\" + ExportSubDirName + "\\DUTs";
            ExportTopologyStructuresToDirectory(exportDir);
        }

        private void ExportTopologyStructuresToDirectory(string exportDir)
        {
            System.Console.WriteLine(@"Exporting Topology structures to the folder ""{0}""", exportDir);
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

            foreach (TopologyStructViewModel topologyStructViewModel in TopologyStructures)
            {
                string structName = topologyStructViewModel.Name;
                StreamWriter sw = new StreamWriter(exportDir + "\\" + structName + ".TcDUT");

                try
                {
                    string boxOrderCode = "";
                    try
                    {
                        boxOrderCode = topologyStructViewModel.BoxOrderCode;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    string id = "";
                    try
                    {
                        id = topologyStructViewModel.Id.Replace('{','[').Replace('}',']');
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    string extends = "";
                    try
                    {
                        extends = topologyStructViewModel.Extends;
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
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    uint crc = 0;
                    try
                    {
                        crc = topologyStructViewModel.Crc32;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    uint sizeInBites = 0;
                    try
                    {
                        sizeInBites = topologyStructViewModel.SizeInBites;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    double sizeInBytes = 0;
                    try
                    {
                        sizeInBytes = topologyStructViewModel.SizeInBytes;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                    sw.WriteLine("<TcPlcObject>");
                    sw.WriteLine("\t<DUT Name=" + @"""" + structName + @""">");
                    sw.WriteLine("\t\t<Declaration><![CDATA[{attribute addProperty BoxType \"" + boxOrderCode + "\"}");
                    sw.WriteLine("{attribute addProperty Id \"" + id + "\"}");
                    sw.WriteLine("{attribute addProperty CRC \"" + crc.ToString() + "\"}");
                    sw.WriteLine("{attribute addProperty SizeInBites \"" + sizeInBites.ToString() + "\"}");
                    sw.WriteLine("{attribute addProperty SizeInBytes \"" + sizeInBytes.ToString() + "\"}");
                    sw.WriteLine("TYPE " + structName + extends);
                    sw.WriteLine("STRUCT");

                    foreach (TopologyStructMemberViewModel topologyStructMemberViewModel in topologyStructViewModel.StructMembers)
                    {
                        foreach(string attribute in topologyStructMemberViewModel.Attributes)
                        {
                            sw.WriteLine("\t" + attribute);
                        }
                        string varName = ValidatePlcItem.Name(topologyStructMemberViewModel.Name);
                        string varType = ValidatePlcItem.Type(topologyStructMemberViewModel.Type_Value);
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
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    sw.Dispose();
                }
                finally
                {
                    sw.Close();
                }
            }
            Console.WriteLine(@"Topology structures exported to the folder ""{0}"" !!!", exportDir);
        }
    }
}

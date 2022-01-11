using System;
using System.IO;
using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void GenerateGvlToPlcProj(Solution vs)
        {
            string exportDir = vs.GvlExported.FolderPathInFileSystem;
            System.Console.WriteLine("Exporting GVL : {0} to the folder {1}", vs.GvlExported.Name, exportDir);
            if (Directory.Exists(exportDir))
            {
                string exportFile = (exportDir + "\\" + vs.GvlExported.Name).Replace("\\\\","\\");
                if (File.Exists(exportFile))
                {
                    File.Delete(exportFile);
                }
            }
            else
            {
                Directory.CreateDirectory(exportDir);
            }


            StreamWriter swGvl = new StreamWriter(vs.GvlExported.CompletePathInFileSystem);

            try
            {
                swGvl.WriteLine("<TcPlcObject>");
                swGvl.WriteLine("\t<GVL Name=" + @"""" + vs.GvlExported.Name + @""">");
                swGvl.WriteLine("\t\t<Declaration><![CDATA[{attribute 'qualified_only'}");
                swGvl.WriteLine("VAR_GLOBAL");

                foreach (DeviceViewModel deviceViewModel in Devices)
                {
                    swGvl.WriteLine("\t\t" + deviceViewModel.MapableObjectGrouped.Name + " : " + deviceViewModel.MapableObjectGrouped.Type_Value + ";");
                }

                swGvl.WriteLine("END_VAR]]></Declaration >");
                swGvl.WriteLine("\t</GVL>");
                swGvl.WriteLine("</TcPlcObject>");
                swGvl.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                swGvl.Dispose();
            }
            finally
            {
                swGvl.Close();
            }
            System.Console.WriteLine("GVL {0} exported to the folder {1}", vs.GvlExported.Name, vs.GvlExported.FolderPathInFileSystem);
        }
    }
}

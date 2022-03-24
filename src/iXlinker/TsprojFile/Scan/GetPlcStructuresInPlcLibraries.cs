using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void GetPlcStructuresInPlcLibraries(Solution vs)
        {
            const string plcDutGUID = "{2db5746d-d284-4425-9f7f-2663a34b0ebc}";

            foreach (PlcLibrary plcLibrary in PlcLibraries)
            {
                if (Directory.Exists(plcLibrary.Path))
                {
                    string browsercache = (plcLibrary.Path + "\\browsercache").Replace("\\\\", "\\");
                    if (File.Exists(browsercache))
                    {
                        try
                        {
                            var xml = XDocument.Load(browsercache);
                            var query = from c in xml.Root.Descendants("Node") where c.Attribute("TypeGUID").Value.Equals(plcDutGUID) select c.Attribute("Name").Value as String;

                            foreach (string plcStructName in query)
                            {
                                PlcStruct plcStruct = new PlcStruct() { NameSpace = plcLibrary.NameSpace, Name = plcStructName.ToString() };
                                PlcStructuresInPlcLibraries.Add(plcStruct);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                            Console.ReadLine();
                        }

                    }
                }
            }
        }
    }
}

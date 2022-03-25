using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.Generic;
using System.IO;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ValidateVarNamesUniqueness(ref TcSmVarGrpDef varGrp, PdoViewModel pdoViewModel)
        {
            List<string> varNames = new List<string>();
            List<string> varNamesDuplicities = new List<string>();

            int sameNameIndex = 1;

            if (varGrp.Var != null)
            {
                foreach (TcSmVarDef varItem in varGrp.Var)
                {
                    if (varItem.Name != null && !varItem.Name.Contains("__") && varNames.Contains(varItem.Name))
                    {
                        if (!varNamesDuplicities.Contains(varItem.Name))
                        {
                            sameNameIndex = 1;
                            varNamesDuplicities.Add(varItem.Name);
                        }
                        EventLogger.Instance.Logger.Information("Not unique variable name {0} found in the variable group name {1}, in the box type {2}!!!", varItem.Name, varGrp.Name, pdoViewModel.BoxOrderCode);
                        if (exportDuplicities)
                        {
                            using (StreamWriter sw = new StreamWriter(@"D:\Inxton\iXlinker\VarNamesDuplicities.txt", true))
                            {
                                sw.WriteLine("{0} ; {1} ; {2}", pdoViewModel.BoxOrderCode, varGrp.Name, varItem.Name);
                            }
                        }
                        varItem.Name = varItem.Name + "_" + sameNameIndex.ToString();
                        sameNameIndex++;
                        EventLogger.Instance.Logger.Information("\t Renamed to {0}!!!", varItem.Name);
                        varNames.Add(varItem.Name);
                    }
                    else
                    {
                        varNames.Add(varItem.Name);
                    }
                }
            }
        }
    }
}

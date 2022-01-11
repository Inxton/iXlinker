using System;
using ViewModels;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.Generic;
using System.IO;

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
                        Console.WriteLine("Not unique variable name {0} found in the variable group name {1}, in the box type {2}!!!", varItem.Name, varGrp.Name, pdoViewModel.BoxOrderCode);
                        if (exportDuplicities)
                        {
                            using (StreamWriter sw = new StreamWriter(@"D:\Inxton\iXlinker\VarNamesDuplicities.txt", true))
                            {
                                sw.WriteLine("{0} ; {1} ; {2}", pdoViewModel.BoxOrderCode, varGrp.Name, varItem.Name);
                            }
                        }
                        varItem.Name = varItem.Name + "_" + sameNameIndex.ToString();
                        sameNameIndex++;
                        Console.WriteLine("\t Renamed to {0}!!!", varItem.Name);
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

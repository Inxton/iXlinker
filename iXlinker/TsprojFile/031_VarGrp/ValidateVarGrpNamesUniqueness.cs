using System;
using ViewModels;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.Generic;
using System.IO;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool ValidateVarGrpNamesUniqueness(ref IBox box)
        {
            return ValidateVarGrpNamesUniquenessBox(ref box);
        }
        private bool ValidateVarGrpNamesUniqueness(ref TcSmTermDef box)
        {
            return ValidateVarGrpNamesUniquenessTerm(ref box);
        }
        private bool ValidateVarGrpNamesUniquenessBox(ref IBox box)
        {
            List<string> varGrpNames = new List<string>();
            List<string> varGrpNamesDuplicities = new List<string>();

            int sameNameIndex = 1;
            bool ret = true;

            if (box.Vars != null)
            {
                foreach (TcSmVarGrpDef varGrp in box.Vars)
                {
                    if (varGrpNames.Contains(varGrp.Name))
                    {
                        if (!varGrpNamesDuplicities.Contains(varGrp.Name))
                        {
                            sameNameIndex = 1;
                            varGrpNamesDuplicities.Add(varGrp.Name);
                        }
                        Console.WriteLine("Not unique varGrp name {0} found in box name {1}!!!", varGrp.Name, box.Name);
                        if (exportDuplicities)
                        {
                            using (StreamWriter sw = new StreamWriter(@"D:\Inxton\iXlinker\VarGrpNamesDuplicities.txt", true))
                            {
                                sw.WriteLine("{0} ; {1} ; {2}", box.Name, varGrp.Name);
                            }
                        }
                        varGrp.Name = varGrp.Name + "_" + sameNameIndex.ToString();
                        sameNameIndex++;
                        Console.WriteLine("\t Renamed to {0}!!!", varGrp.Name);
                        varGrpNames.Add(varGrp.Name);
                    }
                    else
                    {
                        varGrpNames.Add(varGrp.Name);
                    }
                }
            }
            return ret;
        }
        private bool ValidateVarGrpNamesUniquenessTerm(ref TcSmTermDef box)
        {
            List<string> varGrpNames = new List<string>();
            List<string> varGrpNamesDuplicities = new List<string>();
            int sameNameIndex = 1;
            bool ret = true;

            if (box.Vars != null)
            {
                foreach (TcSmVarGrpDef varGrp in box.Vars)
                {
                    if (varGrpNames.Contains(varGrp.Name))
                    {
                        if (!varGrpNamesDuplicities.Contains(varGrp.Name))
                        {
                            sameNameIndex = 1;
                            varGrpNamesDuplicities.Add(varGrp.Name);
                        }
                        Console.WriteLine("Not unique varGrp name {0} found in box name {1}!!!", varGrp.Name, box.Name);
                        if (exportDuplicities)
                        {
                            using (StreamWriter sw = new StreamWriter(@"D:\Inxton\iXlinker\VarGrpsDuplicities.txt", true))
                            {
                                sw.WriteLine("{0} ; {1} ; {2}", box.Name, varGrp.Name);
                            }
                        }
                        varGrp.Name = varGrp.Name + "_" + sameNameIndex.ToString();
                        sameNameIndex++;
                        Console.WriteLine("\t Renamed to {0}!!!", varGrp.Name);
                        varGrpNames.Add(varGrp.Name);
                    }
                    else
                    {
                        varGrpNames.Add(varGrp.Name);
                    }
                }
            }
            return ret;
        }
    }
}

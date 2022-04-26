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
                        EventLogger.Instance.Logger.Information("Not unique varGrp name {0} found in box name {1}!!!", varGrp.Name, box.Name);
                        varGrp.Name = varGrp.Name + "_" + sameNameIndex.ToString();
                        sameNameIndex++;
                        EventLogger.Instance.Logger.Information("\t Renamed to {0}!!!", varGrp.Name);
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
                        EventLogger.Instance.Logger.Information("Not unique varGrp name {0} found in box name {1}!!!", varGrp.Name, box.Name);
                        varGrp.Name = varGrp.Name + "_" + sameNameIndex.ToString();
                        sameNameIndex++;
                        EventLogger.Instance.Logger.Information("\t Renamed to {0}!!!", varGrp.Name);
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

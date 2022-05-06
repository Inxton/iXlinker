using System;
using System.IO;
using iXlinker.Resources;
using iXlinker.Utils;
using iXlinkerDtos;
using TwincatXmlSchemas.TcPlcProj;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void FillPdoStructuresReplacementDictionary()
        {
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("AI_Standard_4A394481",            "AI_Standard_Channel_4A394481"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("RTD_4A394481",                    "AI_Standard_Channel_4A394481"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("RTD_Inputs_Channel_4A394481",     "AI_Standard_Channel_4A394481"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("RTD_RTDInputs_Channel_4A394481",  "AI_Standard_Channel_4A394481"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("TC_Inputs_4A394481",              "AI_Standard_Channel_4A394481"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("TC_Inputs_Channel_4A394481",      "AI_Standard_Channel_4A394481"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("", ""));

            PdoStructureDuplicates.Add(new PdoStructureDuplicate("AO_Outputs_Channel_BA7EBB7C",     "AO_Output_Channel_BA7EBB7C"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("AO_Outputs_BA7EBB7C",             "AO_Output_Channel_BA7EBB7C"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("AO_RxPDO_Map_Channel_BA7EBB7C",   "AO_Output_Channel_BA7EBB7C"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("AO_Standard_Channel_BA7EBB7C",    "AO_Output_Channel_BA7EBB7C"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("AO_X01_Pin5_Standard_BA7EBB7C",   "AO_Output_Channel_BA7EBB7C"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("AO_X02_Pin5_Standard_BA7EBB7C",   "AO_Output_Channel_BA7EBB7C"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("AO_X05_Pin5_Standard_BA7EBB7C",   "AO_Output_Channel_BA7EBB7C"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("AO_X06_Pin5_Standard_BA7EBB7C",   "AO_Output_Channel_BA7EBB7C"));
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("", ""));
           
        }
    }
}

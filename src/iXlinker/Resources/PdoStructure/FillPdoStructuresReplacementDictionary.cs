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
            PdoStructureDuplicates.Add(new PdoStructureDuplicate("AI_Standard_4A394481", "AI_Standard_4A394481"));
            //PdoStructureDuplicates.Add(new PdoStructureDuplicate("AI_Standard_4A394481", "AI_Standard_Channel_4A394481"));
        }
    }
}

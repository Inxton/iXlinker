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
        private void FillSpecialPlcTypesReplacementDictionary()
        {
            SpecialPlcTypes.Add(new SpecialPlcType("AMSADDR", "TcoAmsAddr", PlcStructuresInPlcLibraries));
            SpecialPlcTypes.Add(new SpecialPlcType("AMSNETID", "TcoAmsNetId", PlcStructuresInPlcLibraries));
        }
    }
}
